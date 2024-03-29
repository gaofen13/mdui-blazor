﻿using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiFabMenu : MduiComponentBase
    {
        private bool _isFocus;
        private string? _openedIcon;

        private string FabBtnClassname =>
            new ClassBuilder("mdui-fab")
            .AddClass("mdui-fab-opened", _isFocus)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass($"mdui-text-color-{TextColor}", !string.IsNullOrWhiteSpace(TextColor))
            .AddClass("mdui-fab-mini", Mini)
            .Build();

        private string DialClassname =>
            new ClassBuilder("mdui-fab-dial")
            .AddClass("mdui-fab-dial-show", _isFocus)
            .Build();

        protected string Classname =>
            new ClassBuilder("mdui-fab-wrapper")
            .AddClass("mdui-fab-hide", Hide)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string DialStylelist =>
            new StyleBuilder()
            .AddStyle("opacity", "1", _isFocus)
            .AddStyle("height", "auto", _isFocus)
            .Build();

        protected override void OnInitialized()
        {
            if (string.IsNullOrWhiteSpace(OpenedIcon))
            {
                _openedIcon = Icon;
            }
            else
            {
                _openedIcon = OpenedIcon;
            }
        }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        [Parameter]
        public string? OpenedIcon { get; set; }

        [Parameter]
        public RenderFragment? OpenedIconContent { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public string? TextColor { get; set; }

        [Parameter]
        public bool Mini { get; set; }

        [Parameter]
        public bool Hide { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        private void OnFabClicked()
        {
            _isFocus = !_isFocus;
        }

        private void OnBlur()
        {
            _isFocus = false;
        }
    }
}
