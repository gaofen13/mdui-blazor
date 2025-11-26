using MduiBlazor.Generators;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Shared.Components
{
    public partial class ExampleSection
    {
        private bool _showCode;

        private string Classname =>
            new ClassBuilder("example")
            .AddClass("example-hide-example", HideExample)
            .AddClass("example-code-fix", FixCode)
            .Build();

        private string CodeClassname =>
            new ClassBuilder("mdui-collapse")
            .AddClass("mdui-collapse-open", _showCode || FixCode || HideExample)
            .Build();

        [Parameter, EditorRequired]
        public Type Component { get; set; } = default!;

        [Parameter]
        public IDictionary<string, object>? ComponentParameters { get; set; }

        [Parameter]
        public bool FixCode { get; set; }

        [Parameter]
        public bool HideExample { get; set; }

        [Parameter]
        public string Language { get; set; } = "language-cshtml-razor";

        private string? CodeContents { get; set; }

        protected override void OnInitialized()
        {
            SetCodeContents();
            base.OnInitialized();
        }

        protected void SetCodeContents()
        {
            try
            {
                CodeContents = DemoSnippets.GetRazor(Component.Name);
            }
            catch
            {
                //Do Nothing
            }
        }
    }
}
