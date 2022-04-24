using Dapper.Contrib.Extensions;
using System;

namespace PioneerMobileApp.Models
{
    /// <summary>
    /// Author: Cristina Damian - Roehampton University - Faculty of Computing
    /// 
    /// <para>Class entity to SQL Server PioneerEvent Table</para>
    /// </summary>
    [Table("[dbo].PioneerEvent")] // Define a name of a physical SQL Server Table
    public class PioneerEvent : PioneerUser // Inheritance to PioneerUser (relation n:1)
    {
        [ExplicitKey] public new int Id { get; set; }

        public DateTime EventDate { get; set; }

        public string EventTitle { get; set; }

        public string EventDescription { get; set; }
    }
}
