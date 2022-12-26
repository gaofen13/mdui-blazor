using System.ComponentModel;

namespace MduiBlazor
{
    public enum Color
    {
        [Description("theme")]
        Primary,
        [Description("theme-accent")]
        Accent,
        [Description("blue")]
        Info,
        [Description("green")]
        Success,
        [Description("orange")]
        Warning,
        [Description("red")]
        Error,
        [Description("black")]
        Black,
        [Description("white")]
        White
    }
}
