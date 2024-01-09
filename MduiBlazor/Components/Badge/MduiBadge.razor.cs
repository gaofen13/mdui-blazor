using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiBadge : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-badge")
            .AddClass(Class)
            .Build();

        protected string ContentClassname =>
            new ClassBuilder("mdui-badge-content")
            .AddClass($"mdui-text-color-{TextColor}", !string.IsNullOrWhiteSpace(TextColor))
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass($"mdui-badge-{HorizontalPosition.ToDescriptionString()}")
            .AddClass($"mdui-badge-{VerticalPosition.ToDescriptionString()}")
            .AddClass("mdui-badge-dot", Dot)
            .Build();

        [Parameter]
        public bool Dot { get; set; }

        [Parameter]
        public bool Visible { get; set; } = true;

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public string? TextColor { get; set; }

        [Parameter]
        public string? Content { get; set; }

        [Parameter]
        public HorizontalPosition HorizontalPosition { get; set; }

        [Parameter]
        public VerticalPosition VerticalPosition { get; set; }
    }
}