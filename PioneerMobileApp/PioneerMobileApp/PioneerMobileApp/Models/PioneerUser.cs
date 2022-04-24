using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace PioneerMobileApp.Models
{
    [Table("[dbo].PioneerUser")]
    public class PioneerUser
    {
        [ExplicitKey] public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public UserType UserType { get; set; }
        public string OfficeDepartment { get; set; }

        public Dictionary<DateTime, List<EventModel>> Events { get; set; }

        public PioneerUser()
        {

        }

        public PioneerUser(int id, string firstName, string lastName, string userName, string password, UserType userType, string officeDepartment)
        {
            Id = id;
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
