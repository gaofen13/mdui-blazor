using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDivider : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder($"mdui-divider{(Inset ? "-inset" : "")}")
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Inset { get; set; }
    }
}
