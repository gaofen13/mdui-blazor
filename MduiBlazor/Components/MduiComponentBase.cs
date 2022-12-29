using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class MduiComponentBase : ComponentBase
    {
        public ElementReference Element { get; protected set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// if true its child element be use mduitypo
        /// </summary>
        [Parameter]
        public bool UseMduiTypo { get; set; }

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Style { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}
