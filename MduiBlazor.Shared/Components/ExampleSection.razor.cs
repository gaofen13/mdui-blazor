using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MduiBlazor.Shared.Components
{
    public partial class ExampleSection
    {
        private bool _showCode;

        private string Classname =>
            new ClassBuilder("example")
            .AddClass("example-showcode", _showCode || FixCode)
            .Build();

        [Parameter]
        public string Label { get; set; } = "Example";

        [Parameter, EditorRequired]
        public Type Component { get; set; } = default!;

        [Parameter]
        public IDictionary<string, object>? ComponentParameters { get; set; }

        [Parameter]
        public bool FixCode { get; set; }

        [Parameter]
        public bool HideExample { get; set; }

        private string? CodeContents { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(Component.Name))
                {
                    SetCodeContents();
                }
            }
        }

        protected void SetCodeContents()
        {
            try
            {
                CodeContents = Generators.DemoSnippets.GetRazor(Component.Name);
                StateHasChanged();
            }
            catch
            {
                //Do Nothing
            }
        }
    }
}
