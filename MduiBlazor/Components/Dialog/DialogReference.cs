using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class DialogReference
    {
        private readonly TaskCompletionSource<DialogResult> _resultCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly Action<DialogResult> _closed;

        internal Guid InstanceId { get; }
        internal RenderFragment Instance { get; }
        internal DialogInstance? InstanceRef { get; set; }

        public DialogReference(Guid instanceId, RenderFragment instance)
        {
            InstanceId = instanceId;
            Instance = instance;
            _closed = HandleClosed;
        }

        public Task<DialogResult> Result => _resultCompletion.Task;

        internal void Dismiss(DialogResult result) => _closed.Invoke(result);

        private void HandleClosed(DialogResult obj) => _resultCompletion.TrySetResult(obj);
    }
}
