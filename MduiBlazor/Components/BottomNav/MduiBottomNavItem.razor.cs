﻿using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiBottomNavItem : MduiComponentBase, IDisposable
    {
        private bool _active;
        protected string Classname =>
            new ClassBuilder()
            .AddClass("mdui-bottom-nav-active", _active)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiBottomNav? Nav { get; set; }

        [Parameter]
        public bool Default { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public EventCallback OnActived { get; set; }

        protected override void OnInitialized()
        {
            Nav?.AddItem(this);
            base.OnInitialized();
        }

        public void Active()
        {
            _active = true;
            OnActived.InvokeAsync();
            StateHasChanged();
        }

        public void Disactive()
        {
            _active = false;
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            Nav?.RemoveItem(this);
            GC.SuppressFinalize(this);
        }
    }
}
