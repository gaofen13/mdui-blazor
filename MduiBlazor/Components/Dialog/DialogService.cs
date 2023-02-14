using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class DialogService
    {
        internal event Action<DialogReference>? OnInstanceAdded;
        internal event Action<DialogReference, DialogResult>? OnCloseRequested;

        public DialogReference Show<T>(string? title) where T : IComponent
            => Show<T>(title, null, new ComponentParameters());

        public DialogReference Show<T>(string? title, DialogOptions? options) where T : IComponent
            => Show<T>(title, options, new ComponentParameters());

        public DialogReference Show<T>(string? title, DialogOptions? options, ComponentParameters parameters) where T : IComponent
            => Show(typeof(T), title, options, parameters);

        public DialogReference Show(Type contentComponent, string? title, DialogOptions? options)
            => Show(contentComponent, title, options, new ComponentParameters());

        public DialogReference Show(Type contentComponent, string? title, DialogOptions? options, ComponentParameters parameters)
        {
            if (!typeof(IComponent).IsAssignableFrom(contentComponent))
            {
                throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
            }

            DialogReference? reference = null;
            var instanceId = Guid.NewGuid();
            var content = new RenderFragment(builder =>
            {
                var i = 0;
                builder.OpenComponent(i++, contentComponent);
                foreach (var (name, value) in parameters.Parameters)
                {
                    builder.AddAttribute(i++, name, value);
                }
                builder.CloseComponent();
            });
            var instance = new RenderFragment(builder =>
            {
                builder.OpenComponent<DialogInstance>(0);
                builder.SetKey("DialogInstance_" + instanceId);
                builder.AddAttribute(1, "DialogId", instanceId);
                builder.AddAttribute(2, "Title", title);
                builder.AddAttribute(3, "Options", options);
                builder.AddAttribute(4, "ChildContent", content);
                builder.AddComponentReferenceCapture(5, compRef => reference!.InstanceRef = (DialogInstance)compRef);
                builder.CloseComponent();
            });
            reference = new DialogReference(instanceId, instance, this);

            OnInstanceAdded?.Invoke(reference);

            return reference;
        }

        public DialogReference Alert(string? message, string? title = null)
            => Show(message, title, new DialogOptions { Modal = true, ShowConfirmButton = true, DialogType = DialogType.Alert });

        public DialogReference Confirm(string? message, string? title = null)
            => Show(message, title, new DialogOptions { ShowCancelButton = true, ShowConfirmButton = true, DialogType = DialogType.Confirm });

        public DialogReference Show(string? message, string? title = null, DialogOptions? options = null)
        {
            DialogReference? reference = null;
            var instanceId = Guid.NewGuid();
            var instance = new RenderFragment(builder =>
            {
                builder.OpenComponent<DialogInstance>(0);
                builder.SetKey("DialogInstance_" + instanceId);
                builder.AddAttribute(1, "DialogId", instanceId);
                builder.AddAttribute(2, "Title", title);
                builder.AddAttribute(3, "Message", message);
                builder.AddAttribute(4, "Options", options);
                builder.AddComponentReferenceCapture(5, compRef => reference!.InstanceRef = (DialogInstance)compRef);
                builder.CloseComponent();
            });
            reference = new DialogReference(instanceId, instance, this);

            OnInstanceAdded?.Invoke(reference);

            return reference;
        }

        internal void Close(DialogReference dialog) => Close(dialog, DialogResult.Ok());

        internal void Close(DialogReference dialog, DialogResult result) => OnCloseRequested?.Invoke(dialog, result);
    }
}
