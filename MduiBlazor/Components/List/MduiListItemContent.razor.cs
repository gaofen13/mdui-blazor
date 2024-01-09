using System.ComponentModel;
using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListItemContent : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item-content")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string TitleClassname =>
            new ClassBuilder("mdui-list-item-title")
            .AddClass($"mdui-list-item-{TitleLineSpan?.ToDescriptionString()}", TitleLineSpan is not null)
            .Build();

        private string TextClassname =>
            new ClassBuilder("mdui-list-item-text")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-list-item-{TextLineSpan?.ToDescriptionString()}", TextLineSpan is not null)
            .Build();

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public ListItemLineSpan? TitleLineSpan { get; set; }

        [Parameter]
        public ListItemLineSpan? TextLineSpan { get; set; }
    }

    public enum ListItemLineSpan
    {
        [Description("one-line")]
        One,
        [Description("two-line")]
        Two,
        [Description("three-line")]
        Three
    }
}
