using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Shared.Components
{
    public partial class CodeSnippet
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter]
        public string Language { get; set; } = "language-cshtml-razor";

        [Parameter]
        public string? Class { get; set; } = null;

        [Parameter]
        public string? Style { get; set; } = null;

        private string Classname =>
            new ClassBuilder("snippet mdui-typo")
            .AddClass(Class)
            .Build();
    }
}
