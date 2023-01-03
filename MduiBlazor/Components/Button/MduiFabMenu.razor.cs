using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiFabMenu
    {
        private string? FabIconName => Show ? (!string.IsNullOrWhiteSpace(OpenedIconName) ? OpenedIconName : IconName) : IconName;

        private string FabIconClassname =>
        new ClassBuilder()
         .AddClass("mdui-fab-opened", Show && !string.IsNullOrWhiteSpace(OpenedIconName))
         .Build();

        private string FabBtnClassname =>
        new ClassBuilder()
        .AddClass("mdui-fab-opened", Show)
        .Build();

        private string DialClassname =>
         new ClassBuilder("mdui-fab-dial")
         .AddClass("mdui-fab-dial-show", Show)
         .Build();

        private string DialStyle =>
        new StyleBuilder()
        .AddStyle("height", Show ? "auto" : "0")
        .Build();

        protected string Classname =>
            new ClassBuilder("mdui-fab-wrapper")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Show { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter, EditorRequired]
        public string? IconName { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public string? OpenedIconName { get; set; }

        private void OnClickFabBtn()
        {
            Show = !Show;
        }
    }
}
