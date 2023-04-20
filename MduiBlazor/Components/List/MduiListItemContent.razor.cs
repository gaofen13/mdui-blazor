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
            .AddClass($"mdui-list-item-{TitleLineSpan}-line")
            .Build();

        private string TextClassname =>
            new ClassBuilder("mdui-list-item-text")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-list-item-{TextLineSpan}-line")
            .Build();

        [Parameter]
        public RenderFragment? TitleContent { get; set; }

        [Parameter]
        public RenderFragment? TextContent { get; set; }

        [Parameter]
        public LineSpan TitleLineSpan { get; set; }

        [Parameter]
        public LineSpan TextLineSpan { get; set; }
    }

    public enum LineSpan
    {
        one,
        two,
        three
    }
}
