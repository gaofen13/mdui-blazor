using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiAppbar : MduiComponentBase, IDisposable
    {
        private int _toolbarCount;
        private int _tabCount;

        protected string Classname =>
            new ClassBuilder("mdui-appbar")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-appbar-fixed", Fixed)
            .AddClass($"mdui-shadow-{Shadow}", Shadow >= 0 && Shadow <= 24)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        public MduiLayout? Layout { get; set; }

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public int? Shadow { get; set; }

        public bool HasToolbar => _toolbarCount > 0;
        public bool HasTab => _tabCount > 0;

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

        public void AddTab()
        {
            _tabCount++;
        }

        public void RemoveTab()
        {
            _tabCount--;
        }

        void IDisposable.Dispose()
        {
            Layout?.RemoveAppbar(this);
            GC.SuppressFinalize(this);
        }
    }
}
