using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
