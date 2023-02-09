using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.ObjectModel;

namespace MduiBlazor
{
    public partial class MduiDialogProvider
    {
        [Inject] private NavigationManager? NavigationManager { get; set; }
        [Inject] private DialogService DialogService { get; set; } = default!;
        [Parameter] public DialogOptions Options { get; set; } = new();

        private readonly Collection<DialogReference> _dialogs = new();
        private bool _haveActiveDialogs;

        protected override void OnInitialized()
        {
            if (DialogService == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a cascading parameter of type {nameof(MduiBlazor.DialogService)}.");
            }

            DialogService.OnInstanceAdded += Update;
            DialogService.OnCloseRequested += CloseInstance;
            NavigationManager!.LocationChanged += CancelModals;
        }

        internal void CloseInstance(DialogReference? dialog, DialogResult result)
        {
            if (dialog?.InstanceRef != null)
            {
                // Gracefully close the modal
                dialog.InstanceRef.Close(result);
                if (!_dialogs.Any())
                {
                    ClearBodyStyles();
                }
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
                if (!_dialogs.Any())
                {
                    ClearBodyStyles();
                }
                StateHasChanged();
            }
        }

        private void CancelModals(object? sender, LocationChangedEventArgs e)
        {
            foreach (var dialog in _dialogs.ToList())
            {
                dialog.Dismiss(DialogResult.Cancel());
            }

            _dialogs.Clear();
            ClearBodyStyles();
            StateHasChanged();
        }

        private void Update(DialogReference dialog)
        {
            _dialogs.Add(dialog);

            if (!_haveActiveDialogs)
            {
                _haveActiveDialogs = true;
            }

            StateHasChanged();
        }

        private DialogReference? GetDialogReference(Guid id) => _dialogs.SingleOrDefault(x => x.InstanceId == id);

        private void ClearBodyStyles()
        {
            _haveActiveDialogs = false;
        }
    }
}
