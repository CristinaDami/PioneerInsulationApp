using System.ComponentModel;

namespace PioneerMobileApp.Models
{
    /// <summary>
    /// Author: Cristina Damian - Roehampton University - Faculty of Computing
    /// 
    /// <para>Enum User Type</para>
    /// </summary>
    public enum UserType
    {        
        [Description("Admin user")]
        Admin = 1,
        [Description("Admin office user")]
        AdminOffice = 2,
        [Description("Operative user")]
        Operative = 3,
    }
}
