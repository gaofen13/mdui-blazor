using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDrawer : MduiComponentBase
    {
        private bool _opened;

        protected string Classname =>
            new ClassBuilder("mdui-drawer")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-drawer-right", RightSide)
            .AddClass("mdui-drawer-persistent", Persistent)
            .AddClass("mdui-drawer-full-height", FullHeight)
            .AddClass(_opened ? "mdui-drawer-open" : "mdui-drawer-close")
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiLayout? Layout { get; set; }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public bool Opened
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _opened;
            set
            {
                if (value != _opened)
                {
                    _opened = value;
                    OpenedChanged.InvokeAsync(value);
                    ConfigLayout(value);
                }
            }
        }

        [Parameter]
        public EventCallback<bool> OpenedChanged { get; set; }

        [Parameter]
        public bool Persistent { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public bool RightSide { get; set; }

        [Parameter]
        public bool FullHeight { get; set; }

        private Task OnOverlayClickedAsync()
        {
            Opened = false;
            return Task.CompletedTask;
        }

        public void ConfigLayout(bool drawerOpened)
        {
            if (drawerOpened)
            {
                Layout?.AddDarwer(this);
            }
            else
            {
                Layout?.RemoveDarwer(this);
            }
        }
    }
}
