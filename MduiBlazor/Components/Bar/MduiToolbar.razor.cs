using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiToolbar : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        public MduiAppbar? Appbar { get; set; }

        [Parameter]
        public string? Color { get; set; }

        protected override void OnInitialized()
        {
            Appbar?.AddToolbar();
            base.OnInitialized();
        }

        void IDisposable.Dispose()
        {
            Appbar?.RemoveToolbar();
            GC.SuppressFinalize(this);
        }
    }
}
