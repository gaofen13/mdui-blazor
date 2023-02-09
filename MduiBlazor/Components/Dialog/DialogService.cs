using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class DialogService
    {
        internal event Action<DialogReference>? OnInstanceAdded;
        internal event Action<DialogReference, DialogResult>? OnCloseRequested;

        public DialogReference Show<T>() where T : IComponent
            => Show<T>(false, new ComponentParameters());

        public DialogReference Show<T>(bool modal) where T : IComponent
            => Show<T>(modal, new ComponentParameters());

        public DialogReference Show<T>(bool modal, ComponentParameters parameters) where T : IComponent
            => Show(typeof(T), modal, parameters);

        public DialogReference Show(Type contentComponent, bool modal)
            => Show(contentComponent, modal, new ComponentParameters());

        public DialogReference Show(Type contentComponent, bool modal, ComponentParameters parameters)
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
                builder.OpenComponent<MduiDialog>(0);
                builder.SetKey("DialogInstance_" + instanceId);
                builder.AddAttribute(1, "InstanceId", instanceId);
                builder.AddAttribute(2, "IsShow", true);
                builder.AddAttribute(3, "Modal", modal);
                builder.AddAttribute(4, "ChildContent", content);
                builder.AddComponentReferenceCapture(5, compRef => reference!.InstanceRef = (MduiDialog)compRef);
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
