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
using Xamarin.Plugin.Calendar.Models;
using Xamarin.Plugin.Calendar.Enums;

namespace PioneerMobileApp.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public CalendarViewModel() : base()
        {
            // Retrieving PioneerUser authenticated and stored securely in a memory storage (SecureStorage)
            var user = Task.Run(() => SecureStorage.GetAsync(ApplicationConstants.CurrentUser)).Result;

            // Object is stored as a serialized JSON string: it requires to be deserialized as PioneerUser object
            var pioneerUser = JsonConvert.DeserializeObject<PioneerUser>(user);

            // Pioneer database repository access layer
            var pioneerRepository = new PioneerRepository();

            // Empty collection of PioneerEvent
            var events = Enumerable.Empty<PioneerEvent>();

            // 
            if (pioneerUser.UserTypeId == UserType.Admin || pioneerUser.UserTypeId == UserType.AdminOffice)
            {
                events = pioneerRepository.GetAllEvents(); // Retieve all events only if Admin
            }
            else
            {
                events = pioneerRepository.GetEventsByUserId(pioneerUser.Id); // Operative retrieves its own events
            }

            // Events are grouped by Event Date and ordered in ascending order
            var eventsGrouped = events
                .GroupBy(x => x.EventDate)
                .Select(x => new
                {
                    x.Key,
                    Events = x.Select(y => new EventModel()
                    {
                        Name = string.Concat(y.FirstName, " ", y.LastName, " - ", y.EventTitle),
                        Description = y.EventDescription
                    })
                }).OrderBy(x => x.Key).ThenBy(x => x.Events.OrderBy(y => y.Name));

            // Empty collection of Colour
            var colorUsed = new List<Color>();

            // Parsing PioneerEvents into a collection event that Calendar can handle
            Events = new EventCollection();

            // Iterates a Events grouped by Date
            foreach (var @event in eventsGrouped)
            {
                Color color = Color.Default;
                var foundColor = true;
                while (foundColor) // Assigns a Colour for each Key/Value Date/Events pair
                {
                    Random rnd = new Random();
                    color = Color.FromRgb(255, rnd.Next(256), rnd.Next(256));// Generates a random Colour
                    if (!colorUsed.Any(x => x.Equals(color))) // Check to assign an unique color 
                    {
                        foundColor = false;
                        colorUsed.Add(color);
                    }
                }

                var dayEvent = new DayEventCollection<EventModel>(@event.Events) // Creates a Day event collection of Type Event model
                {
                    EventIndicatorColor = color,
                    EventIndicatorSelectedColor = color
                };

                Events.Add(@event.Key, dayEvent); // Add pair Event date / collection of Events to the collection of events
            }
        }

        // Collection of properties and fields
        #region Attributes, Field and Events

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

        #endregion

    }
}
