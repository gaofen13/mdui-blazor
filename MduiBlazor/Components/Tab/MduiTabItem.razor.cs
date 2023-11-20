using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTabItem : MduiComponentBase, IDisposable
    {
        private bool _active;

        private string Classname =>
            new ClassBuilder("mdui-tab-content")
            .AddClass("mdui-tab-active", _active)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass(Class)
            .Build();

        public Guid Id { get; } = Guid.NewGuid();

        [CascadingParameter(Name = "Tab")]
        private MduiTab? Tab { get; set; }

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
                ActiveItem();
            }
            Tab?.AddItem(this);
        }

        public void ActiveItem()
        {
            _active = true;
            StateHasChanged();
            if (OnActived.HasDelegate)
            {
                OnActived.InvokeAsync();
            }
        }

        public void DisactiveItem()
        {
            _active = false;
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            Tab?.RemoveItem(this);
            GC.SuppressFinalize(this);
        }
    }
}
