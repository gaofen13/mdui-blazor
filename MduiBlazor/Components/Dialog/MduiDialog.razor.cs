using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDialog : MduiComponentBase
    {
        private DialogOptions _options = new();

        private bool ShowDialog => DialogContainer is not null || Show;

        private string Classname =>
          new ClassBuilder("mdui-dialog")
            .AddClass($"mdui-dialog-{_options.DialogType.ToDescriptionString()}", _options.DialogType != DialogType.Dialog)
            .AddClass(Class)
            .Build();

        [CascadingParameter] private MduiDialogProvider? DialogProvider { get; set; }
        [CascadingParameter] private DialogContainer? DialogContainer { get; set; }

        [Parameter] public string? Title { get; set; }
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
            else if (DialogContainer?.Options is not null)
            {
                _options = DialogContainer.Options;
                Show = true;
            }
            else if (DialogProvider?.Options is not null)
            {
                _options = DialogProvider.Options;
            }
            base.OnInitialized();
        }

        private void Cancel()
        {
            if (DialogContainer is null)
            {
                Show = false;
                ShowChanged.InvokeAsync(false);
            }
            else
            {
                DialogContainer.Cancel();
            }
        }

        private void Confirm()
        {
            if (DialogContainer is null)
            {
                Show = false;
                ShowChanged.InvokeAsync(false);
            }
            else
            {
                DialogContainer.Ok();
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
