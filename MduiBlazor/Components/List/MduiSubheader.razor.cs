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

        [Parameter]
        public bool Inset { get; set; }
    }
}
