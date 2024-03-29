using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

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
            .AddClass($"mdui-color-{IconColor}", !string.IsNullOrWhiteSpace(IconColor))
            .Build();

        [Parameter]
        public object? Value { get; set; }

        [Parameter, EditorRequired]
        public string? Title { get; set; }

        [Parameter]
        public string? ImgSrc { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        [Parameter]
        public string? IconLetter { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool DeleteButton { get; set; }

        [Parameter]
        public EventCallback<MduiChip> OnDeleteButtonClicked { get; set; }

        private void OnClickedDeleteButton()
        {
            if (OnDeleteButtonClicked.HasDelegate)
            {
                OnDeleteButtonClicked.InvokeAsync(this);
            }
        }
    }
}