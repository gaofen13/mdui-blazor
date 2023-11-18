using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class SnackbarService
    {
        public event Action<RenderFragment, string?, SnackbarOptions?>? OnShow;

        public event Action<Type, ComponentParameters?, SnackbarOptions?>? OnShowComponent;

        public event Action? OnClearAll;

        public void ShowSnackbar(string message)
            => ShowSnackbar(message, null, null);

        public void ShowSnackbar(string message, string color)
            => ShowSnackbar(message, color, null);

        public void ShowSnackbar(string message, SnackbarOptions options)
            => ShowSnackbar(message, null, options);

        public void ShowSnackbar(string message, string? color, SnackbarOptions? options)
            => ShowSnackbar(builder => builder.AddContent(0, message), color, options);

        public void ShowSnackbar(RenderFragment message, string? color, SnackbarOptions? options)
            => OnShow?.Invoke(message, color, options);

        public void ShowSnackbar<TComponent>(ComponentParameters parameters, SnackbarOptions options) where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), parameters, options);

        public void ShowSnackbar<TComponent>(ComponentParameters parameters) where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), parameters, null);

        public void ShowSnackbar<TComponent>(SnackbarOptions options) where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), null, options);

        public void ShowSnackbar<TComponent>() where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), null, null);

        public void ShowSnackbar(Type snackbarComponent, ComponentParameters? parameters, SnackbarOptions? options)
        {
            if (!typeof(IComponent).IsAssignableFrom(snackbarComponent))
            {
                throw new ArgumentException($"{snackbarComponent.FullName} must be a Blazor Component");
            }
            OnShowComponent?.Invoke(snackbarComponent, parameters, options);
        }

        public void ClearAll() => OnClearAll?.Invoke();
    }
}
