using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace PioneerMobileApp.Models
{
    /// <summary>
    /// Author: Cristina Damian - Roehampton University - Faculty of Computing
    /// 
    /// <para>Class entity to SQL Server PioneerUser Table</para>
    /// </summary>
    [Table("[dbo].PioneerUser")] // Define a name of a physical SQL Server Table
    public class PioneerUser
    {
        [ExplicitKey] public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public UserType UserTypeId { get; set; }
        public string OfficeDepartment { get; set; }

        public Dictionary<DateTime, List<EventModel>> Events { get; set; }

        /// <summary>
        /// Class constructor for PioneerUser
        /// </summary>
        public PioneerUser()
        {
            Events = new Dictionary<DateTime, List<EventModel>>();
        }
    }
}
