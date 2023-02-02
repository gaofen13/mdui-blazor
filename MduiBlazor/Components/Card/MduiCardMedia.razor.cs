using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiCardMedia : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-card-media")
            .AddClass(Class)
            .Build();

        private string CoveredClassname =>
            new ClassBuilder("mdui-card-media-covered")
            .AddClass("mdui-card-media-covered-top", CoveredTop)
            .AddClass("mdui-card-media-covered-gradient", CoveredGradient)
            .AddClass("mdui-card-media-covered-transparent", CoveredTransparent)
            .Build();

        private string ActionsClassname =>
            new ClassBuilder("mdui-card-actions")
            .AddClass("mdui-card-actions-stacked", ActionsStacked)
            .Build();

        [Parameter]
        public string? Image { get; set; }

        [Parameter]
        public bool Covered { get; set; }

        [Parameter]
        public bool CoveredTop { get; set; }

        [Parameter]
        public bool CoveredTransparent { get; set; }

        [Parameter]
        public bool CoveredGradient { get; set; }

        [Parameter]
        public bool ActionsStacked { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? SubTitle { get; set; }

        [Parameter]
        public RenderFragment? MenusContent { get; set; }

        [Parameter]
        public RenderFragment? ActionsContent { get; set; }
    }
}
