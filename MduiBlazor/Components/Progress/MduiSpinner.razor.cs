using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiSpinner : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-spinner")
            .AddClass("mdui-spinner-colorful", Colorful)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Colorful { get; set; }

        private static string SpinnerLayer(int? number = null)
        {
            var numberLayer = number == null ? "" : (" mdui-spinner-layer-" + number);
            return $"<div class=\"mdui-spinner-layer{numberLayer}\"><div class=\"mdui-spinner-circle-clipper mdui-spinner-left\"><div class=\"mdui-spinner-circle\"></div></div><div class=\"mdui-spinner-gap-patch\"><div class=\"mdui-spinner-circle\"></div></div><div class=\"mdui-spinner-circle-clipper mdui-spinner-right\"><div class=\"mdui-spinner-circle\"></div></div></div>";
        }
    }
}