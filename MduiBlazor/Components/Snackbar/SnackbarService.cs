using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public class SnackbarService
    {
        public event Action<RenderFragment, SnackbarOptions?>? OnShow;

        public event Action<Type, ComponentParameters?, SnackbarOptions?>? OnShowComponent;

        public event Action? OnClearAll;

        public void ShowSnackbar(string message, SnackbarOptions? options = null)
            => ShowSnackbar(builder => builder.AddContent(0, message), options);

        public void ShowSnackbar(RenderFragment message, SnackbarOptions? options = null)
            => OnShow?.Invoke(message, options);

        public void ShowSnackbar<TComponent>(ComponentParameters? parameters = null, SnackbarOptions? options = null) where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), parameters, options);

        public void ShowSnackbar<TComponent>(ComponentParameters? parameters = null) where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), parameters);

        public void ShowSnackbar<TComponent>(SnackbarOptions? options = null) where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent), null, options);

        public void ShowSnackbar<TComponent>() where TComponent : IComponent
            => ShowSnackbar(typeof(TComponent));

        public void ShowSnackbar(Type snackbarComponent, ComponentParameters? parameters = null, SnackbarOptions? options = null)
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
