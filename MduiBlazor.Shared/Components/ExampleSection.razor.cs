using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MduiBlazor.Shared.Components
{
    public partial class ExampleSection
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter]
        public string? Label { get; set; }

        [Parameter, EditorRequired]
        public string? ExampleFile { get; set; }

        [Parameter]
        public bool ShowCode { get; set; }

        private MarkupString? CodeContents { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(ExampleFile))
                {
                    await SetCodeContents();
                }
            }
        }

        protected async Task SetCodeContents()
        {
            try
            {
                var codeString = Generators.DemoSnippets.GetRazor(ExampleFile);
                var res = await JSRuntime.InvokeAsync<string>("HighlightCode", codeString);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    CodeContents = new MarkupString(res);
                    StateHasChanged();
                }
            }
            catch
            {
                //Do Nothing
            }
        }
    }
}
