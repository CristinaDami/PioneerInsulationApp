using Dapper.Contrib.Extensions;
using System;

namespace PioneerMobileApp.Models
{
    [Table("[dbo].PioneerEvent")]
    public class UserEvent
    {
        [ExplicitKey] public int Id { get; set; }

        public DateTime EventDate { get; set; }

        public string EventTitle { get; set; }

        public string EventDescription { get; set; }
    }
}
