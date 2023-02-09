using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class DialogReference
    {
        private readonly TaskCompletionSource<DialogResult> _resultCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly Action<DialogResult> _closed;
        private readonly DialogService _dialogService;

        internal Guid InstanceId { get; }
        internal RenderFragment Instance { get; }
        internal MduiDialog? InstanceRef { get; set; }

        public DialogReference(Guid instanceId, RenderFragment instance, DialogService dialogService)
        {
            InstanceId = instanceId;
            Instance = instance;
            _closed = HandleClosed;
            _dialogService = dialogService;
        }

        public void Close() => _dialogService.Close(this);

        public void Close(DialogResult result) => _dialogService.Close(this, result);

        public Task<DialogResult> Result => _resultCompletion.Task;

        internal void Dismiss(DialogResult result) => _closed.Invoke(result);

        private void HandleClosed(DialogResult obj) => _resultCompletion.TrySetResult(obj);
    }
}
