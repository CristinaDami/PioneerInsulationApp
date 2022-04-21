using System.ComponentModel;

namespace PioneerMobileApp.Models
{
    public enum UserType
    {        
        [Description("Admin user")]
        Admin = 0,
        [Description("Admin office user")]
        AdminOffice = 1,
        [Description("Operative user")]
        Operative = 2,
    }
}
