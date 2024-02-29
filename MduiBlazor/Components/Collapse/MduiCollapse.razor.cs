using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiCollapse : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-collapse")
            .AddClass("mdui-collapse-open", Open)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string HeaderClassname =>
            new ClassBuilder("mdui-collapse-header")
            .AddClass(HeaderClass, !string.IsNullOrWhiteSpace(HeaderClass))
            .Build();

        private string BodyClassname =>
            new ClassBuilder("mdui-collapse-body")
            .AddClass(BodyClass, !string.IsNullOrWhiteSpace(BodyClass))
            .Build();

        private string BodyHtmlTag => HtmlTag == "li" ? "ul" : "div";

        [Parameter]
        public string HtmlTag { get; set; } = "li";

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public string? HeaderClass { get; set; }

        [Parameter]
        public string? BodyClass { get; set; }

        [Parameter]
        public RenderFragment? HeaderContent { get; set; }
    }
}
