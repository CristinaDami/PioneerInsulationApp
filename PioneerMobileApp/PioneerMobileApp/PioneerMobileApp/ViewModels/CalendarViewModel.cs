using Newtonsoft.Json;
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
using Xamarin.Plugin.Calendar.Enums;

namespace PioneerMobileApp.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public ICommand TodayCommand => new Command(() =>
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
        });

        
        public ICommand DayTappedCommand => new Command<DateTime>(async (date) => await DayTapped(date));
        public ICommand SwipeLeftCommand => new Command(() => ChangeShownUnit(1));
        public ICommand SwipeRightCommand => new Command(() => ChangeShownUnit(-1));
        public ICommand SwipeUpCommand => new Command(() => { ShownDate = DateTime.Today; });

        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));

        public CalendarViewModel() : base()
        {
            //Device.BeginInvokeOnMainThread(async () => await App.Current.MainPage.DisplayAlert("Info", "Loading events with delay, and changeing current view.", "Ok"));

            var user = Task.Run(() => SecureStorage.GetAsync(ApplicationConstants.CurrentUser)).Result;
            var pioneerUser = JsonConvert.DeserializeObject<PioneerUser>(user);

            //

            var pioneerRepository = new PioneerRepository();
            var events = Enumerable.Empty<PioneerEvent>();

            if (pioneerUser.UserType == UserType.Admin)
            {
                events = pioneerRepository.GetAllEvents();
            }
            else
            {
                events = pioneerRepository.GetEventsByUserId(pioneerUser.Id);
            }
            
            var eventsGrouped = events
                .GroupBy(x => x.EventDate)
                .Select(x => new { x.Key, Events = x.Select(y => new EventModel() { 
                    Name = string.Concat(y.FirstName, " ", y.LastName, " - ", y.EventTitle), Description = y.EventDescription
                }) }).OrderBy(x => x.Key).ThenBy(x => x.Events.OrderBy(y => y.Name));

            var colorUsed = new List<Color>();

            Events = new EventCollection();
            foreach (var @event in eventsGrouped)
            {
                Color color = Color.Default;
                var foundColor = true;
                while (foundColor)
                {
                    Random rnd = new Random();
                    color = Color.FromRgb(255, rnd.Next(256), rnd.Next(256));
                    if (!colorUsed.Any(x => x.Equals(color)))
                    {
                        foundColor = false;
                        colorUsed.Add(color);
                    }
                }

                var dayEvent = new DayEventCollection<EventModel>(@event.Events)
                {
                    EventIndicatorColor = color,
                    EventIndicatorSelectedColor = color
                };

                Events.Add(@event.Key, dayEvent);
            }
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

        private DateTime _shownDate = DateTime.Today;

        public DateTime ShownDate
        {
            get => _shownDate;
            set => SetProperty(ref _shownDate, value);
        }

        private WeekLayout _calendarLayout = WeekLayout.Month;

        public WeekLayout CalendarLayout
        {
            get => _calendarLayout;
            set => SetProperty(ref _calendarLayout, value);
        }

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

        private static async Task DayTapped(DateTime date)
        {
            var message = $"Received tap event from date: {date}";
            await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");
        }

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is EventModel eventModel)
            {
                await App.Current.MainPage.DisplayAlert(eventModel.Name, eventModel.Description, "Ok");
            }
        }

        private void ChangeShownUnit(int amountToAdd)
        {
            switch (CalendarLayout)
            {
                case WeekLayout.Week:
                case WeekLayout.TwoWeek:
                    ChangeShownWeek(amountToAdd);
                    break;

                case WeekLayout.Month:
                default:
                    ChangeShownMonth(amountToAdd);
                    break;
            }
        }

        private void ChangeShownMonth(int monthsToAdd)
        {
            ShownDate.AddMonths(monthsToAdd);
        }

        private void ChangeShownWeek(int weeksToAdd)
        {
            ShownDate.AddDays(weeksToAdd * 7);
        }

    }
}
