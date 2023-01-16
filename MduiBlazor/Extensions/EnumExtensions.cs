using System.ComponentModel;

namespace MduiBlazor.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            if (field != null)
            {
                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0
                    ? attributes[0].Description
                    : val.ToString().ToLower();
            }
            return val.ToString().ToLower();
        }
    }
}
