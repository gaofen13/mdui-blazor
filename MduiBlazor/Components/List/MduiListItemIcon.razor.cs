using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListItemIcon : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item-icon")
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool Custom { get; set; }
    }
}
