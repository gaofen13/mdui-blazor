using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiIconButton : MduiButtonBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn-icon")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? IconName { get; set; }
    }
}
