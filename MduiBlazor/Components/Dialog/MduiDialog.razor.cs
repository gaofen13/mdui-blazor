using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDialog : MduiComponentBase
    {
        private DialogOptions _options = new();

        protected string Classname =>
          new ClassBuilder("mdui-dialog")
            .AddClass("mdui-dialog-open", DialogInstance != null || Open)
            .AddClass($"mdui-dialog-{_options.DialogType.ToDescriptionString()}", _options.DialogType != DialogType.Dialog)
            .AddClass(Class)
            .Build();

        private string ActionsClassname =>
            new ClassBuilder("mdui-dialog-actions")
            .AddClass("mdui-dialog-actions-stacked", _options.ActionsStack)
            .Build();

        [CascadingParameter]
        private MduiDialogProvider? DialogProvider { get; set; }

        [CascadingParameter]
        private DialogInstance? DialogInstance { get; set; }

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        [Parameter]
        public DialogOptions? Options { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Message { get; set; }

        [Parameter]
        public RenderFragment? ActionsContent { get; set; }

        protected override void OnInitialized()
        {
            if (Options is not null)
            {
                _options = Options;
            }
            else if (DialogProvider?.Options is not null)
            {
                _options = DialogProvider.Options;
            }
            base.OnInitialized();
        }

        public void Ok()
        {
            Close(DialogResult.Ok());
        }

        public void Cancel()
        {
            Close(DialogResult.Cancel());
        }

        public void Close(DialogResult modalResult)
        {
            if (DialogInstance != null)
            {
                DialogInstance.Close(modalResult);
            }
            else
            {
                Open = false;
                _ = OpenChanged.InvokeAsync(false);
            }
        }

        private void OnClickBackground()
        {
            if (_options.Modal)
            {
                Cancel();
            }
        }
    }
}
