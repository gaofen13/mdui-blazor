using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiChip : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-chip")
            .AddClass(Class)
            .Build();

        private string IconClassname =>
            new ClassBuilder("mdui-chip-icon")
            .Build();

        [Parameter, EditorRequired]
        public string? Title { get; set; }

        [Parameter]
        public string? Image { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconLetter { get; set; }

        [Parameter]
        public bool DeleteButton { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnDeleteButtonClicked { get; set; }
    }
}