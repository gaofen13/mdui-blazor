﻿using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiList : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .AddClass(Class)
            .Build();

        private string HtmlTag { get; set; } = "ul";

        [Parameter]
        public bool NavMenu { get; set; }

        [Parameter]
        public bool Dense { get; set; }

        protected override void OnInitialized()
        {
            if (NavMenu)
            {
                HtmlTag = "div";
            }
            base.OnInitialized();
        }
    }
}
