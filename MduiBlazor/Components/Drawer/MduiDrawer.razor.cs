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
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

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
                        OpenDrawer();
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

        /// <summary>
        /// When Variant == DrawerVariant.Responsive the breakpoint(px, default 1024) on Temporary and Persistent
        /// </summary>
        [Parameter]
        public int Breakpoint { get; set; } = 1024;

        [Parameter]
        public string? Color { get; set; }

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

        public void OpenDrawer()
        {
            _opened = true;
            OpenedChanged.InvokeAsync(true);
            Layout?.AddDarwer(this);
        }

        public void CloseDrawer()
        {
            _opened = false;
            OpenedChanged.InvokeAsync(false);
            Layout?.RemoveDarwer(this);
        }

        protected override void OnInitialized()
        {
            Navigation.LocationChanged += Navigation_LocationChanged;
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Variant == DrawerVariant.Responsive)
                {
                    _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                     "./_content/MduiBlazor/Components/Drawer/MduiDrawer.razor.js");
                    //bool isMobile = await _jsModule!.InvokeAsync<bool>("isDevice");
                    var width = await _jsModule!.InvokeAsync<int>("getWindowWidth");

                    if (width >= Breakpoint)
                    {
                        _variant = DrawerVariant.Persistent;
                        OpenDrawer();
                    }
                }
                else if (Variant == DrawerVariant.Persistent)
                {
                    _variant = DrawerVariant.Persistent;
                    OpenDrawer();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void Navigation_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            if (_variant == DrawerVariant.Temporary && _opened)
            {
                CloseDrawer();
            }
        }
    }
}
