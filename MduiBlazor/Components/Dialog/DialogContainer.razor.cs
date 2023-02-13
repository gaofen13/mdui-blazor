using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class DialogContainer
    {
        [CascadingParameter]
        private MduiDialogProvider? DialogProvider { get; set; }

        [Parameter]
        public Guid DialogId { get; set; }

        [Parameter]
        public DialogOptions? Options { get; set; }

        public void Ok()
        {
            Close(DialogResult.Ok());
        }

        public void Close(DialogResult modalResult)
        {
            DialogProvider?.DismissInstance(DialogId, modalResult);
        }

        public void Cancel()
        {
            Close(DialogResult.Cancel());
        }

    }
}