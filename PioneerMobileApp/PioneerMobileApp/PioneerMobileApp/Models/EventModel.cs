using System;

namespace PioneerMobileApp.Models
{
    /// <summary>
    /// Author: Cristina Damian - Roehampton University - Faculty of Computing
    /// 
    /// <para>Class model for Calender event</para>
    /// </summary>
    public class EventModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Starting { get; set; }
    }
}
