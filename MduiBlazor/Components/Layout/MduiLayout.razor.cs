using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiLayout : MduiComponentBase
    {
        private bool _locked;
        private bool _firstLoaded;
        private MduiDrawer? _currentLeftDrawer;
        private MduiDrawer? _currentRightDrawer;

        protected string Classname =>
            new ClassBuilder("mdui-layout")
            .AddClass("mdui-locked", _locked)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-loaded", _firstLoaded)
            .AddClass($"mdui-theme-primary-{PrimaryColor.ToDescriptionString()}")
            .AddClass($"mdui-theme-accent-{AccentColor.ToDescriptionString()}")
            .AddClass("mdui-drawer-body-left", _currentLeftDrawer?.Persistent == true && _currentLeftDrawer.Opened)
            .AddClass("mdui-drawer-body-right", _currentRightDrawer?.Persistent == true && _currentRightDrawer.Opened)
            .AddClass("mdui-appbar-with-tab", AppbarFixedWithTab && !AppbarFixedWithToolbar)
            .AddClass("mdui-appbar-with-toolbar", AppbarFixedWithToolbar && !AppbarFixedWithTab)
            .AddClass("mdui-appbar-with-tab-larger", AppbarFixedWithTab && AppbarFixedWithToolbar)
            .AddClass(IsDarkTheme ? "mdui-theme-layout-dark" : "mdui-theme-layout-light")
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool AppbarFixedWithToolbar { get; set; }

        [Parameter]
        public bool AppbarFixedWithTab { get; set; }

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

        public void AddDarwer(MduiDrawer drawer)
        {
            if (drawer.RightSide)
            {
                _currentRightDrawer = drawer;
            }
            else
            {
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

        public void LockOverflow()
        {
            if (!_locked)
            {
                _locked = true;
                StateHasChanged();
            }
        }

        public void UnlockOverflow()
        {
            if (_locked)
            {
                _locked = false;
                StateHasChanged();
            }
        }
    }
}
