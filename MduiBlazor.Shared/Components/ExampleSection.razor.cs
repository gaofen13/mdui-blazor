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

        protected override void OnInitialized()
        {
            CodeContents = Generators.DemoSnippets.GetRazor(Component.Name);
            base.OnInitialized();
        }

        protected void SetCodeContents()
        {
            try
            {
                CodeContents = Generators.DemoSnippets.GetRazor(Component.Name);
            }
            catch
            {
                //Do Nothing
            }
        }
    }
}
