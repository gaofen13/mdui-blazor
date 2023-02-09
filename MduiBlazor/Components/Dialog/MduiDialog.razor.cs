using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDialog : MduiComponentBase
    {
        private DialogOptions _options = new DialogOptions();

        private string Classname =>
          new ClassBuilder("mdui-dialog")
            .AddClass("mdui-dialog-open", IsShow)
            .AddClass($"mdui-dialog-{_options.Type.ToDescriptionString()}", _options.Type != DialogType.Dialog)
            .AddClass(Class)
            .Build();

        [CascadingParameter] private MduiDialogProvider? DialogProvider { get; set; }

        [Parameter] public string? Title { get; set; }
        [Parameter] public string? Message { get; set; }
        [Parameter] public DialogOptions? Options { get; set; }
        [Parameter] public bool IsShow { get; set; }
        [Parameter] public EventCallback<bool> IsShowChanged { get; set; }
        [Parameter] public Guid InstanceId { get; set; }

        protected override void OnInitialized()
        {
            if (Options is not null)
            {
                _options = Options;
            }
            else if (DialogProvider is not null)
            {
                _options = DialogProvider.Options;
            }
            base.OnInitialized();
        }

        public void Close()
        {
            Close(DialogResult.Ok());
            // if (OnConfirm.HasDelegate)
            // {
            //     OnConfirm.InvokeAsync();
            // }
            IsShow = false;
            IsShowChanged.InvokeAsync(false);
        }

        public void Close(DialogResult modalResult)
        {
            DialogProvider?.DismissInstance(InstanceId, modalResult);
        }

        public void Cancel()
        {
            Close(DialogResult.Cancel());
            // if (OnCancel.HasDelegate)
            // {
            //     OnCancel.InvokeAsync();
            // }
            IsShow = false;
            IsShowChanged.InvokeAsync(false);
        }

        private async Task OnClickBackgroundAsync()
        {
            if (_options.Modal)
            {
                Cancel();
                if (IsShow)
                {
                    IsShow = false;
                    await IsShowChanged.InvokeAsync(IsShow);
                }
            }
        }
    }
}
