using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiIconButton : MduiButtonBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn-icon")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconColor { get; set; }
    }
}
