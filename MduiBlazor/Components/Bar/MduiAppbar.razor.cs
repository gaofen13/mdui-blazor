using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiAppbar : IDisposable
    {
        private int _toolbarCount;

        protected string Classname =>
            new ClassBuilder("mdui-appbar")
            .AddClass("mdui-appbar-fixed", Fixed)
            .AddClass("mdui-appbar-scroll-hide", ScrollHide)
            .AddClass("mdui-appbar-scroll-toolbar-hide", ScrollToolbarHide)
            .AddClass($"mdui-shadow-{Shadow}", Shadow >= 0 && Shadow <= 24)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        public MduiLayout? Layout { get; set; }

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public bool ScrollHide { get; set; }

        [Parameter]
        public bool ScrollToolbarHide { get; set; }

        [Parameter]
        public int? Shadow { get; set; }

        public bool HasToolbar => _toolbarCount > 0;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Layout?.AddAppbar(this);
            }
            base.OnAfterRender(firstRender);
        }

        public void AddToolbar()
        {
            _toolbarCount++;
        }

        public void RemoveToolbar()
        {
            _toolbarCount--;
        }

        void IDisposable.Dispose()
        {
            Layout?.RemoveAppbar(this);
            GC.SuppressFinalize(this);
        }
    }
}
