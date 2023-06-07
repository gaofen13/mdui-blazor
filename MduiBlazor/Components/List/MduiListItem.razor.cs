﻿using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-list-item-active", Active)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public bool Active { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? IconName { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }
    }
}
