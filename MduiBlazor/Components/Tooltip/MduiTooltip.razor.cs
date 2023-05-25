using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTooltip : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-tooltip")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-tooltip-{Position.ToDescriptionString()}")
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Message { get; set; }

        [Parameter]
        public Position Position { get; set; }
    }
}
