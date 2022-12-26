using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDrawer
    {
        protected string Classname =>
            new ClassBuilder("mdui-drawer")
            .AddClass(Opened ? "mdui-drawer-open" : "mdui-drawer-close")
            .AddClass("mdui-drawer-right", RightSide)
            .AddClass("mdui-drawer-full-height mdui-appbar-inset", FullHeight)
            .AddClass($"mdui-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Opened { get; set; }

        [Parameter]
        public EventCallback<bool> OpenedChanged { get; set; }

        [Parameter]
        public Color? Color { get; set; }

        [Parameter]
        public bool RightSide { get; set; }

        [Parameter]
        public bool FullHeight { get; set; }

        [Parameter]
        public bool Overlay { get; set; }

        private void OnOverlayClicked()
        {
            if (Overlay)
            {
                Overlay = false;
                Opened = false;
                OpenedChanged.InvokeAsync(false);
            }
        }
    }
}
