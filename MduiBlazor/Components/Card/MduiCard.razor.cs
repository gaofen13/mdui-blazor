using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiCard : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-card")
            .AddClass(Class)
            .Build();

        private string ActionsClassname =>
            new ClassBuilder("mdui-card-actions")
            .AddClass("mdui-card-actions-stacked", ActionsStacked)
            .Build();

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? SubTitle { get; set; }

        [Parameter]
        public bool ActionsStacked { get; set; }

        [Parameter]
        public RenderFragment? HeaderContent { get; set; }

        [Parameter]
        public RenderFragment? MediaContent { get; set; }

        [Parameter]
        public RenderFragment? ActionsContent { get; set; }
    }
}
