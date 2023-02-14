using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class DialogInstance
    {
        private DialogOptions _options = new();

        protected string Classname =>
          new ClassBuilder("mdui-dialog")
            .AddClass($"mdui-dialog-{_options.DialogType.ToDescriptionString()}", _options.DialogType != DialogType.Dialog)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiDialogProvider? DialogProvider { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Message { get; set; }

        [Parameter]
        public Guid DialogId { get; set; }

        [Parameter]
        public DialogOptions? Options { get; set; }

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
            DialogProvider?.DismissInstance(DialogId, modalResult);
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