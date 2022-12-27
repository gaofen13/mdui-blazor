using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiToolbar : IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar")
            .AddClass($"mdui-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        public MduiAppbar? Appbar { get; set; }

        [Parameter]
        public Color? Color { get; set; }

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
