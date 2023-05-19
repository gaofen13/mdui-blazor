using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiRow : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-row")
            .AddClass("mdui-row-gapless", Gapless)
            .AddClass($"mdui-row-{BreakpointXs?.ToDescriptionString()}", BreakpointXs is not null)
            .AddClass($"mdui-row-{BreakpointSm?.ToDescriptionString()}", BreakpointSm is not null)
            .AddClass($"mdui-row-{BreakpointMd?.ToDescriptionString()}", BreakpointMd is not null)
            .AddClass($"mdui-row-{BreakpointLg?.ToDescriptionString()}", BreakpointLg is not null)
            .AddClass($"mdui-row-{BreakpointXl?.ToDescriptionString()}", BreakpointXl is not null)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Gapless { get; set; }

        [Parameter]
        public Breakpoint.Xs? BreakpointXs { get; set;}

        [Parameter]
        public Breakpoint.Sm? BreakpointSm { get; set;}

        [Parameter]
        public Breakpoint.Md? BreakpointMd { get; set;}

        [Parameter]
        public Breakpoint.Lg? BreakpointLg { get; set;}

        [Parameter]
        public Breakpoint.Xl? BreakpointXl { get; set;}
    }
}