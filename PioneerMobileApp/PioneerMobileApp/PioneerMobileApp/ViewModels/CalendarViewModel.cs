﻿using Newtonsoft.Json;
using PioneerMobileApp.Common;
using PioneerMobileApp.Models;
using PioneerMobileApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Plugin.Calendar.Models;

namespace PioneerMobileApp.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public ICommand TodayCommand => new Command(() =>
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
        });

        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));

        public CalendarViewModel() : base()
        {
            //Device.BeginInvokeOnMainThread(async () => await App.Current.MainPage.DisplayAlert("Info", "Loading events with delay, and changeing current view.", "Ok"));

            var user = Task.Run(() => SecureStorage.GetAsync(ApplicationConstants.CurrentUser)).Result;
            var pioneerUser = JsonConvert.DeserializeObject<PioneerUser>(user);


            var pioneerRepository = new PioneerRepository();
            var events = Task.Run(() => pioneerRepository.GetEventsByUserId(pioneerUser.Id)).Result;
            // testing all kinds of adding events
            // when initializing collection

            var eventsGrouped = events
                .GroupBy(x => x.EventDate)
                .Select(x => new { x.Key, Events = x.Select(y => new EventModel() { Name = y.EventTitle, Description = y.EventDescription }) });

            Events = new EventCollection();
            eventsGrouped.ForEach(x => Events.Add(x.Key, x.Events.ToList() ));

            //Events = new EventCollection
            //{
            //    [DateTime.Now.AddDays(-3)] = new List<EventModel>(GenerateEvents(10, "Cool")),
            //    [DateTime.Now.AddDays(4)] = new List<EventModel>(GenerateEvents(2, "Simple2")),
            //    [DateTime.Now.AddDays(2)] = new List<EventModel>(GenerateEvents(1, "Simple1")),
            //    [DateTime.Now.AddDays(1)] = new List<EventModel>(GenerateEvents(3, "Simple3")),
            //};

            //// with add method
            //Events.Add(DateTime.Now.AddDays(-1), new List<EventModel>(GenerateEvents(5, "Cool")));

            //// with indexer
            //Events[DateTime.Now] = new List<EventModel>(GenerateEvents(2, "Boring"));

            //Task.Delay(5000).ContinueWith(_ =>
            //{
            //    // indexer - update later
            //    Events[DateTime.Now] = new ObservableCollection<EventModel>(GenerateEvents(10, "Cool"));

            //    // add later
            //    Events.Add(DateTime.Now.AddDays(3), new List<EventModel>(GenerateEvents(5, "Cool")));

            //    // indexer later
            //    Events[DateTime.Now.AddDays(10)] = new List<EventModel>(GenerateEvents(10, "Boring"));

            //    // add later
            //    Events.Add(DateTime.Now.AddDays(15), new List<EventModel>(GenerateEvents(10, "Cool")));

            //    Month += 1;

            //    Task.Delay(3000).ContinueWith(t =>
            //    {
            //        // get observable collection later
            //        var todayEvents = Events[DateTime.Now] as ObservableCollection<EventModel>;

            //        // insert/add items to observable collection
            //        todayEvents.Insert(0, new EventModel { Name = "Cool event insert", Description = "This is Cool event's description!" });
            //        todayEvents.Add(new EventModel { Name = "Cool event add", Description = "This is Cool event's description!" });

            //        Month += 1;
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private IEnumerable<EventModel> GenerateEvents(int count, string name)
        {
            return Enumerable.Range(1, count).Select(x => new EventModel
            {
                Name = $"{name} event{x}",
                Description = $"This is {name} event{x}'s description!"
            });
        }

        public EventCollection Events { get; }

        private int _month = DateTime.Today.Month;

        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private int _year = DateTime.Today.Year;

        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        private DateTime? _selectedDate = DateTime.Today;

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private DateTime _minimumDate = new DateTime(2019, 4, 29);

        public DateTime MinimumDate
        {
            get => _minimumDate;
            set => SetProperty(ref _minimumDate, value);
        }

        private DateTime _maximumDate = DateTime.Today.AddMonths(5);

        public DateTime MaximumDate
        {
            get => _maximumDate;
            set => SetProperty(ref _maximumDate, value);
        }

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is EventModel eventModel)
            {
                await App.Current.MainPage.DisplayAlert(eventModel.Name, eventModel.Description, "Ok");
            }
        }

    }
}
