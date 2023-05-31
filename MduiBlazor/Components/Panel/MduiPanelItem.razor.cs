using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiPanelItem : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-panel-item")
            .AddClass("mdui-panel-item-open", Open)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiPanel? Panel { get; set; }

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

        protected override void OnInitialized()
        {
            Panel?.AddItem(this);
            base.OnInitialized();
        }

        private async Task OnClickHeaderAsync(MouseEventArgs args)
        {
            if (OnHeaderClicked.HasDelegate)
            {
                await OnHeaderClicked.InvokeAsync(args);
            }
            else
            {
                if (Open)
                {
                    ClosePanel();
                }
                else
                {
                    if (Panel?.Accordion == true)
                    {
                        Panel.CloseAllItems();
                    }
                    OpenPanel();
                }
            }
        }

        internal void OpenPanel()
        {
            Open = true;
            OpenChanged.InvokeAsync(true);
        }

        internal void ClosePanel()
        {
            Open = false;
            OpenChanged.InvokeAsync(false);
        }

        internal void ReRender()
        {
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            Panel?.RemoveItem(this);
            GC.SuppressFinalize(this);
        }
    }
}