using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDialog : MduiComponentBase
    {
        private DialogOptions _options = new();

        [CascadingParameter] private MduiDialogProvider? DialogProvider { get; set; }
        [CascadingParameter] private DialogInstance? DialogInstance { get; set; }

        [Parameter] public string? Message { get; set; }
        [Parameter] public RenderFragment? ActionsContent { get; set; }
        [Parameter] public DialogOptions? Options { get; set; }
        [Parameter] public bool Show { get; set; }
        [Parameter] public EventCallback<bool> ShowChanged { get; set; }

        protected override void OnInitialized()
        {
            if (Options is not null)
            {
                _options = Options;
            }
            else if (DialogInstance?.Options is not null)
            {
                _options = DialogInstance.Options;
                Show = true;
            }
            else if (DialogProvider?.Options is not null)
            {
                _options = DialogProvider.Options;
            }
            base.OnInitialized();
        }
    }
}
