using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiNavMenuSubheader : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder()
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-subheader{(Inset ? "-inset" : "")}")
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Inset { get; set; }
    }
}
