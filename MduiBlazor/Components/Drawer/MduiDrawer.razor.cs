using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MduiBlazor
{
    public partial class MduiDrawer
    {
        private bool _opened;
        private DrawerVariant _variant = DrawerVariant.Temporary;
        private IJSObjectReference? _jsModule;

        protected string Classname =>
            new ClassBuilder("mdui-drawer")
            .AddClass(_opened ? "mdui-drawer-open" : "mdui-drawer-close")
            .AddClass("mdui-drawer-right", RightSide)
            .AddClass($"mdui-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass(Class)
            .Build();

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

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
                    if (value)
                    {
                        _opened = true;
                        OpenedChanged.InvokeAsync(true);
                        Layout?.AddDarwer(this);
                    }
                    else
                    {
                        CloseDrawer();
                    }
                }
            }
        }

        [Parameter]
        public EventCallback<bool> OpenedChanged { get; set; }

        [Parameter]
        public DrawerVariant Variant { get; set; } = DrawerVariant.Responsive;

        [Parameter]
        public Color? Color { get; set; }

        [Parameter]
        public bool RightSide { get; set; }

        [Parameter]
        public bool FullHeight { get; set; }

        private Task OnOverlayClickedAsync()
        {
            if (_opened)
            {
                CloseDrawer();
            }
            return Task.CompletedTask;
        }

        public void CloseDrawer()
        {
            _opened = false;
            OpenedChanged.InvokeAsync(false);
            Layout?.RemoveDarwer(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                 "./_content/MduiBlazor/Components/Drawer/MduiDrawer.razor.js");
                //bool isMobile = await _jsModule!.InvokeAsync<bool>("isDevice");
                var width = await _jsModule!.InvokeAsync<int>("getWindowWidth");

                if (width >= 1024 && Variant == DrawerVariant.Responsive)
                {
                    _variant = DrawerVariant.Persistent;
                    _opened = true;
                    await OpenedChanged.InvokeAsync(true);
                }

                Layout?.AddDarwer(this);
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
