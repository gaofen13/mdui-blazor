using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiFabMenu : MduiComponentBase
    {
        private bool _isFocus;

        private string FabBtnClassname =>
            new ClassBuilder("mdui-fab")
            .AddClass("mdui-fab-opened", _isFocus)
            .AddClass("mdui-ripple", Ripple)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("mdui-fab-mini", Mini)
            .AddClass("mdui-fab-hide", Hide)
            .Build();

        private string DialClassname =>
            new ClassBuilder("mdui-fab-dial")
            .AddClass("mdui-fab-dial-show", _isFocus)
            .Build();

        protected string Classname =>
            new ClassBuilder("mdui-fab-wrapper")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Color { get; set; }

        [Parameter, EditorRequired]
        public string? Icon { get; set; }

        [Parameter]
        public string? OpenedIcon { get; set; }

        [Parameter]
        public bool Mini { get; set; }

        [Parameter]
        public bool Hide { get; set; }

        [Parameter]
        public bool Ripple { get; set; } = true;

        private void OnFabClicked()
        {
            _isFocus = !_isFocus;
        }

        private void OnBlur()
        {
            _isFocus = false;
        }
    }
}
