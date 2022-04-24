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
            Events = new Dictionary<DateTime, List<EventModel>>();
        }
    }
}
