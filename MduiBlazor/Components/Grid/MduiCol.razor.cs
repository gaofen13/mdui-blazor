using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiCol : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-col")
            .AddClass($"mdui-col-{BreakpointXs?.ToDescriptionString()}", BreakpointXs is not null)
            .AddClass($"mdui-col-{BreakpointSm?.ToDescriptionString()}", BreakpointSm is not null)
            .AddClass($"mdui-col-{BreakpointMd?.ToDescriptionString()}", BreakpointMd is not null)
            .AddClass($"mdui-col-{BreakpointLg?.ToDescriptionString()}", BreakpointLg is not null)
            .AddClass($"mdui-col-{BreakpointXl?.ToDescriptionString()}", BreakpointXl is not null)
            .AddClass($"mdui-col-offset-{OffsetBreakpointXs?.ToDescriptionString()}", OffsetBreakpointXs is not null)
            .AddClass($"mdui-col-offset-{OffsetBreakpointSm?.ToDescriptionString()}", OffsetBreakpointSm is not null)
            .AddClass($"mdui-col-offset-{OffsetBreakpointMd?.ToDescriptionString()}", OffsetBreakpointMd is not null)
            .AddClass($"mdui-col-offset-{OffsetBreakpointLg?.ToDescriptionString()}", OffsetBreakpointLg is not null)
            .AddClass($"mdui-col-offset-{OffsetBreakpointXl?.ToDescriptionString()}", OffsetBreakpointXl is not null)
            .AddClass(Class)
            .Build();

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

        [Parameter]
        public Breakpoint.Xs? OffsetBreakpointXs { get; set;}

        [Parameter]
        public Breakpoint.Sm? OffsetBreakpointSm { get; set;}

        [Parameter]
        public Breakpoint.Md? OffsetBreakpointMd { get; set;}

        [Parameter]
        public Breakpoint.Lg? OffsetBreakpointLg { get; set;}

        [Parameter]
        public Breakpoint.Xl? OffsetBreakpointXl { get; set;}
    }
}