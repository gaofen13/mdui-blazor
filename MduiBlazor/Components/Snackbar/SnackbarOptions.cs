using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public class SnackbarOptions
    {
        public int TimeOut { get; set; } = 4000;

        public PositionX PositionX { get; set; }

        public PositionY PositionY { get; set; }

        public bool ShowCloseButton { get; set; } = true;

        public string? CloseBtnColor { get; set; }
    }
}
