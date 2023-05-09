using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class DialogService
    {
        internal event Action<DialogReference>? OnInstanceAdded;
        internal event Action<DialogReference, DialogResult>? OnCloseRequested;

        public DialogReference Show<T>(ComponentParameters? parameters = null) where T : IComponent
            => Show(typeof(T), parameters);

        public DialogReference Show(Type contentComponent, ComponentParameters? parameters = null)
        {
            if (!typeof(IComponent).IsAssignableFrom(contentComponent))
            {
                throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
            }

            var content = new RenderFragment(builder =>
            {
                var i = 0;
                builder.OpenComponent(i++, contentComponent);
                if (parameters != null)
                {
                    foreach (var (name, value) in parameters.Parameters)
                    {
                        builder.AddAttribute(i++, name, value);
                    }
                }
                builder.CloseComponent();
            });

            DialogReference? reference = null;
            var instanceId = Guid.NewGuid();
            var instance = new RenderFragment(builder =>
            {
                builder.OpenComponent<DialogInstance>(0);
                builder.SetKey("DialogInstance_" + instanceId);
                builder.AddAttribute(1, "DialogId", instanceId);
                builder.AddAttribute(2, "ChildContent", content);
                builder.AddComponentReferenceCapture(3, compRef => reference!.InstanceRef = (DialogInstance)compRef);
                builder.CloseComponent();
            });
            reference = new DialogReference(instanceId, instance);

            OnInstanceAdded?.Invoke(reference);

            return reference;
        }

        public DialogReference Alert(string? message, string? title = null)
            => Show(message, title, new DialogOptions { Modal = true, ShowConfirmButton = true, DialogType = DialogType.Alert });

        public DialogReference Confirm(string? message, string? title = null)
            => Show(message, title, new DialogOptions { ShowCancelButton = true, ShowConfirmButton = true, DialogType = DialogType.Confirm });

        public DialogReference Show(string? message, string? title = null, DialogOptions? options = null)
        {
            var parameters = new ComponentParameters();
            if (!string.IsNullOrWhiteSpace(message))
            {
                parameters.Add("Message", message);
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                parameters.Add("Title", title);
            }
            if (options != null)
            {
                parameters.Add("Options", options);
            }

            return Show(typeof(MduiDialog), parameters);
        }

        internal void Close(DialogReference dialog) => Close(dialog, DialogResult.Ok());

        internal void Close(DialogReference dialog, DialogResult result) => OnCloseRequested?.Invoke(dialog, result);
    }
}
