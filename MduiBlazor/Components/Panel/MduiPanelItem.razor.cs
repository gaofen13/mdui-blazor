using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiPanelItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-panel-item")
            .AddClass("mdui-panel-item-open", Open)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public IEnumerable<string>? Summary { get; set; }

        [Parameter]
        public RenderFragment? ActionsContent { get; set; }

        [Parameter]
        public string ArrowIcon { get; set; } = "keyboard_arrow_down";

        [Parameter]
        public EventCallback<MouseEventArgs> OnHeaderClicked { get; set; }

        private async Task OnClickHeaderAsync(MouseEventArgs args)
        {
            if (OnHeaderClicked.HasDelegate)
            {
                await OnHeaderClicked.InvokeAsync(args);
            }
            else
            {
                Open = !Open;
                await OpenChanged.InvokeAsync(Open);
            }
        }
    }
}