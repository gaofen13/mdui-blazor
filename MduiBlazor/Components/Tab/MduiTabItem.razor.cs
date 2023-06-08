using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTabItem : IDisposable
    {
        private bool _active;

        private string Classname =>
            new ClassBuilder()
            .AddClass(Class)
            .Build();

        public string TitleClass =>
            new ClassBuilder("mdui-tab-content")
            .AddClass("mdui-tab-active", _active)
            .AddClass("mdui-ripple", !DisableRipple)
            .Build();

        [CascadingParameter]
        private MduiTab MduiTab { get; set; } = default!;

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        [Parameter]
        public bool Default { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback OnActived { get; set; }

        protected override void OnInitialized()
        {
            if (Default && !Disabled)
            {
                ActiveTab();
            }
            MduiTab.AddTab(this);
            base.OnInitialized();
        }

        public void ActiveTab()
        {
            _active = true;
            OnActived.InvokeAsync();
        }

        public void DisactiveTab()
        {
            _active = false;
        }

        void IDisposable.Dispose()
        {
            MduiTab.RemoveTab(this);
            GC.SuppressFinalize(this);
        }
    }
}
