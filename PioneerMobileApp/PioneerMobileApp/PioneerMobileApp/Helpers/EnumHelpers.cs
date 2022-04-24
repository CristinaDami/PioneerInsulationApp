using System;
using System.ComponentModel;
using System.Reflection;

namespace PioneerMobileApp.Helpers
{
    /// <summary>
    /// Author: Cristina Damian - Roehampton University - Faculty of Computing
    /// 
    /// <para>Helper class for getting value from Description Enum attribute</para>
    /// </summary>
    public static class EnumHelpers
    {
        public static string GetEnumDescription(this Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
