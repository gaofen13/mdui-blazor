using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class MduiComponentBase : ComponentBase
    {
        public ElementReference Element { get; protected set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Style { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}
