using System.ComponentModel;

namespace MduiBlazor
{
    public enum DateInputType
    {
        [Description("date")]
        Date,
        [Description("time")]
        Time,
        [Description("datetime-local")]
        DateTime,
        [Description("month")]
        Month
    }
}
