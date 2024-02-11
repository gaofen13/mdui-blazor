using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiCollapse : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-collapse-item")
            .AddClass("mdui-collapse-item-open", Open)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string HeaderClassname =>
            new ClassBuilder("mdui-collapse-item-header")
            .AddClass(HeaderClass, !string.IsNullOrWhiteSpace(HeaderClass))
            .AddClass("mdui-ripple", !DisableRipple)
            .Build();

        private string BodyClassname =>
            new ClassBuilder("mdui-collapse-item-body")
            .AddClass(BodyClass, !string.IsNullOrWhiteSpace(BodyClass))
            .Build();

        private string BodyStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", "100vh", Open)
            .AddStyle("overflow-y", "auto", Open)
            .Build();

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? HeaderClass { get; set; }

        [Parameter]
        public string? BodyClass { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public RenderFragment? HeaderContent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClickHeader { get; set; }

        private void OnTitleClicked(MouseEventArgs args)
        {
            Open = !Open;
            if (OnClickHeader.HasDelegate)
            {
                OnClickHeader.InvokeAsync();
            }
        }
    }
}
