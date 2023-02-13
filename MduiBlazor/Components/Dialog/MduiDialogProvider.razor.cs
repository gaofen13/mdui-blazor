using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;

namespace MduiBlazor
{
    public partial class MduiDialogProvider
    {
        private readonly Collection<DialogReference> _dialogs = new();

        [Inject] private NavigationManager? NavigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; } = default!;

        [Parameter] public DialogOptions? Options { get; set; }

        protected override void OnInitialized()
        {
            if (DialogService == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a cascading parameter of type {nameof(MduiBlazor.DialogService)}.");
            }

            DialogService.OnInstanceAdded += Update;
            DialogService.OnCloseRequested += DismissInstance;
            NavigationManager!.LocationChanged += CancelDialogs;
        }

        internal void CloseInstance(DialogReference? dialog, DialogResult result)
        {
            if (dialog?.Container != null)
            {
                dialog.Container.Close(result);
            }
            else
            {
                DismissInstance(dialog, result);
            }
        }

        internal void DismissInstance(Guid id, DialogResult result)
        {
            var reference = GetDialogReference(id);
            DismissInstance(reference, result);
        }

        internal void DismissInstance(DialogReference? dialog, DialogResult result)
        {
            if (dialog != null)
            {
                dialog.Dismiss(result);
                _dialogs.Remove(dialog);
                StateHasChanged();
            }
        }

        private void CancelDialogs(object? sender, LocationChangedEventArgs e)
        {
            foreach (var dialog in _dialogs.ToList())
            {
                dialog.Dismiss(DialogResult.Cancel());
            }

            _dialogs.Clear();
            StateHasChanged();
        }

        private void Update(DialogReference dialog)
        {
            _dialogs.Add(dialog);

            StateHasChanged();
        }

        private DialogReference? GetDialogReference(Guid id) => _dialogs.SingleOrDefault(x => x.InstanceId == id);
    }
}
