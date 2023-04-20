using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiSubheader : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder($"mdui-subheader{(Inset ? "-inset" : "")}")
            .AddClass(Class)
            .Build();

        private string HtmlTag { get; set; } = "li";

        [CascadingParameter]
        private MduiList? MduiList { get; set; }

        [Parameter]
        public bool Inset { get; set; }

        protected override void OnInitialized()
        {
            if (MduiList?.NavMenu == true)
            {
                HtmlTag = "div";
            }
            base.OnInitialized();
        }
    }
}
