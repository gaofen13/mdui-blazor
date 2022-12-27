using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiLayout
    {
        private bool _appbarWithToolbar;
        private MduiDrawer? _currentLeftDrawer;
        private MduiDrawer? _currentRightDrawer;

        protected string Classname =>
            new ClassBuilder("mdui-loaded")
            .AddClass($"mdui-theme-primary-{PrimaryColor}")
            .AddClass($"mdui-theme-{AccentColor}")
            .AddClass("mdui-appbar-with-toolbar", _appbarWithToolbar)
            .AddClass("mdui-drawer-body-left", _currentLeftDrawer is not null)
            .AddClass("mdui-drawer-body-right", _currentRightDrawer is not null)
            .AddClass(IsDarkTheme? "mdui-theme-layout-dark" : "mdui-theme-layout-light")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string PrimaryColor { get; set; } = "deep-purple";

        [Parameter]
        public string AccentColor { get; set; } = "pink";

        [Parameter]
        public bool IsDarkTheme { get; set; }

        public void AddAppbar(MduiAppbar bar)
        {
            if (bar.HasToolbar)
            {
                if (!_appbarWithToolbar)
                {
                    _appbarWithToolbar = true;
                    StateHasChanged();
                }
            }
        }

        public void AddDarwer(MduiDrawer drawer)
        {
            if (drawer.RightSide)
            {
                if (_currentRightDrawer is not null)
                {
                    _currentRightDrawer.CloseDrawer();
                }
                _currentRightDrawer = drawer;
            }
            else
            {
                if (_currentLeftDrawer is not null)
                {
                    _currentLeftDrawer.CloseDrawer();
                }
                _currentLeftDrawer = drawer;
            }
            StateHasChanged();
        }


        public void RemoveDarwer(MduiDrawer drawer)
        {
            if (drawer.RightSide)
            {
                if (_currentRightDrawer is not null)
                {
                    _currentRightDrawer = null;
                }
            }
            else
            {
                if (_currentLeftDrawer is not null)
                {
                    _currentLeftDrawer = null;
                }
            }
            StateHasChanged();
        }
    }
}
