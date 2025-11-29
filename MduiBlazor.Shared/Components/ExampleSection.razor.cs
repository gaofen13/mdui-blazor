using MduiBlazor.Generators;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Shared.Components
{
    public partial class ExampleSection
    {
        private string Classname =>
            new ClassBuilder("example")
            .AddClass("example-hide-example", HideExample || Component is null)
            .AddClass("example-code-fix", FixCode)
            .Build();

        private string CodeClassname =>
            new ClassBuilder("mdui-collapse")
            .AddClass("mdui-collapse-open", ShowCode || FixCode || HideExample || Component is null)
            .Build();

        [Parameter]
        public Type? Component { get; set; }

        [Parameter]
        public IDictionary<string, object>? ComponentParameters { get; set; }

        [Parameter]
        public bool FixCode { get; set; }

        [Parameter]
        public bool ShowCode { get; set; }

        [Parameter]
        public bool HideExample { get; set; }

        [Parameter]
        public string? CodeContents { get; set; }

        [Parameter]
        public string Language { get; set; } = "language-cshtml-razor";

        protected override void OnInitialized()
        {
            if (string.IsNullOrWhiteSpace(CodeContents))
            {
                SetCodeContents();
                base.OnInitialized();
            }
        }

        protected void SetCodeContents()
        {
            try
            {
                CodeContents = DemoSnippets.GetRazor(Component!.Name);
            }
            catch
            {
                //Do Nothing
            }
        }
    }
}
