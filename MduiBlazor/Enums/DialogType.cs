using System.ComponentModel;

namespace MduiBlazor
{
    public enum DialogType
    {
        [Description("dialog")]
        Dialog,
        [Description("alert")]
        Alert,
        [Description("confirm")]
        Confirm,
        [Description("prompt")]
        Prompt
    }
}
