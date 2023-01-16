using System.ComponentModel;

namespace MduiBlazor
{
    public enum ButtonType
    {
        [Description("button")]
        Button,
        [Description("submit")]
        Submit,
        [Description("reset")]
        Reset
    }
}
