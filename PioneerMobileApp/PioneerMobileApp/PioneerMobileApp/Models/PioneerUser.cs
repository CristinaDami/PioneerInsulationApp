using System;
using System.Collections.Generic;

namespace PioneerMobileApp.Models
{
    public class PioneerUser
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public string UserName { get; private set; }
        public string Password { get; private set; }

        public UserType UserType { get; private set; }
        public string OfficeDepartment { get; private set; }

        public Dictionary<DateTime, List<EventModel>> Events { get; private set; }

        public PioneerUser(string firstName, string lastName, string userName, string password, UserType userType, string officeDepartment)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Password = password;
            UserType = userType;
            OfficeDepartment = officeDepartment;

            Events = new Dictionary<DateTime, List<EventModel>>();
        }

        public void SetEvents((DateTime dateTime, List<EventModel> events) @event)
        {
            Events.Add(@event.dateTime, @event.events);
        }
    }
}
