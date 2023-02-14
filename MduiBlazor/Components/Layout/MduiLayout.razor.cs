using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiLayout : MduiComponentBase
    {
        private bool _firstLoaded;
        private bool _appbarWithTab;
        private bool _appbarWithToolbar;
        private MduiDrawer? _currentLeftDrawer;
        private MduiDrawer? _currentRightDrawer;

        protected string Classname =>
            new ClassBuilder()
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-loaded", _firstLoaded)
            .AddClass($"mdui-theme-primary-{PrimaryColor.ToDescriptionString()}")
            .AddClass($"mdui-theme-accent-{AccentColor.ToDescriptionString()}")
            .AddClass("mdui-drawer-body-left", _currentLeftDrawer is not null)
            .AddClass("mdui-drawer-body-right", _currentRightDrawer is not null)
            .AddClass("mdui-appbar-with-tab", _appbarWithTab && !_appbarWithToolbar)
            .AddClass("mdui-appbar-with-toolbar", _appbarWithToolbar && !_appbarWithTab)
            .AddClass("mdui-appbar-with-tab-larger", _appbarWithTab && _appbarWithToolbar)
            .AddClass(IsDarkTheme ? "mdui-theme-layout-dark" : "mdui-theme-layout-light")
            .AddClass(Class)
            .Build();

        [Parameter]
        public PrimaryColor PrimaryColor { get; set; } = PrimaryColor.DeepPurple;

        [Parameter]
        public AccentColor AccentColor { get; set; } = AccentColor.Pink;

        [Parameter]
        public bool IsDarkTheme { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _firstLoaded = true;
            }
            base.OnAfterRender(firstRender);
        }

        public void AddAppbar(MduiAppbar bar)
        {
            _appbarWithTab = bar.HasTab;
            _appbarWithToolbar = bar.HasToolbar;
            if (bar.HasTab || bar.HasToolbar)
            {
                StateHasChanged();
            }
        }

        public void RemoveAppbar(MduiAppbar bar)
        {
            _appbarWithTab = false;
            _appbarWithToolbar = false;
            if (bar.HasTab || bar.HasToolbar)
            {
                StateHasChanged();
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
