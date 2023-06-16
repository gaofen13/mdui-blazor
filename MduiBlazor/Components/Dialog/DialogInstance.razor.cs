using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class DialogInstance
    {
        [CascadingParameter]
        private MduiDialogProvider? DialogProvider { get; set; }

        [Parameter]
        public Guid DialogId { get; set; }

        internal void Close(DialogResult modalResult)
        {
            DialogProvider?.DismissInstance(DialogId, modalResult);
        }
    }
}