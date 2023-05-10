using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MduiBlazor.Shared.Components
{
    public partial class CodeSnippet
    {
        private ElementReference _codeElement;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("hljs.highlightElement", _codeElement);
            }
        }
    }
}
