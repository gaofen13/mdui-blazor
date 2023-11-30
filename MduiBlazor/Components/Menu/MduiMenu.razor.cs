using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiMenu : MduiComponentBase
    {
        private bool _isFocus;

        protected string Classname =>
            new ClassBuilder("mdui-menu")
            .AddClass("mdui-menu-open", _isFocus)
            .AddClass("mdui-menu-end", AlignEnd)
            .AddClass($"mdui-menu-{Position.ToDescriptionString()}")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string MenuListClass =>
            new ClassBuilder("mdui-menu-content")
            .Build();

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public RenderFragment? ActivatorContent { get; set; }

        [Parameter]
        public bool AlignEnd { get; set; }

        [Parameter]
        public Position Position { get; set; } = Position.Bottom;

        private void OnActivatorClicked()
        {
            _isFocus = !_isFocus;
        }

        public void Active(MouseEventArgs args)
        {
            _isFocus = !_isFocus;
            StateHasChanged();
        }

        public void DisActive()
        {
            _isFocus = false;
            StateHasChanged();
        }
    }
}
