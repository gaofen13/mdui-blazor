using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiLayout : MduiComponentBase
    {
        private bool _locked;
        private bool _firstLoaded;
        private bool _persistentLeftDrawerOpened;
        private bool _persistentRightDrawerOpened;

        protected string Classname =>
            new ClassBuilder("mdui-layout")
            .AddClass("mdui-locked", _locked)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-loaded", _firstLoaded)
            .AddClass($"mdui-theme-primary-{PrimaryColor.ToDescriptionString()}")
            .AddClass($"mdui-theme-accent-{AccentColor.ToDescriptionString()}")
            .AddClass("mdui-drawer-body-left", _persistentLeftDrawerOpened)
            .AddClass("mdui-drawer-body-right", _persistentRightDrawerOpened)
            .AddClass("mdui-appbar-with-tab", AppbarWithTab)
            .AddClass("mdui-appbar-with-toolbar", AppbarWithToolbar)
            .AddClass("mdui-appbar-with-tab-larger", AppbarWithTabLarger)
            .AddClass($"mdui-theme-layout-{DarkThemeMode.ToDescriptionString()}")
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool AppbarWithToolbar { get; set; }

        [Parameter]
        public bool AppbarWithTab { get; set; }

        [Parameter]
        public bool AppbarWithTabLarger { get; set; }

        [Parameter]
        public PrimaryColor PrimaryColor { get; set; } = PrimaryColor.DeepPurple;

        [Parameter]
        public AccentColor AccentColor { get; set; } = AccentColor.Pink;

        [Parameter]
        public DarkThemeMode DarkThemeMode { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _firstLoaded = true;
            }
        }

        public void ConfigLeftDarwer(bool persistentLeftDarwerOpened)
        {
            _persistentLeftDrawerOpened = persistentLeftDarwerOpened;
            StateHasChanged();
        }

        public void ConfigRightDarwer(bool persistentRightDarwerOpened)
        {
            _persistentRightDrawerOpened = persistentRightDarwerOpened;
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
