using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiCollapseItem
    {
        protected string Classname =>
            new ClassBuilder("mdui-collapse-item")
            .AddClass("mdui-collapse-item-open", Open)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string HeaderClassname =>
            new ClassBuilder("mdui-collapse-item-header mdui-list-item")
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();

        private string BodyClassname =>
            new ClassBuilder("mdui-collapse-item-body mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .Build();

        private string BodyStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", $"{ChildItemCount * (Dense ? 40 : 48)}px", Open)
            .Build();

        private int ChildItemCount { get; set; }

        private string HtmlTag { get; set; } = "li";

        private string BodyHtmlTag { get; set; } = "ul";

        [CascadingParameter]
        private MduiList? MduiList { get; set; }

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public bool Dense { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool CustomIcon { get; set; }

        protected override void OnInitialized()
        {
            if (MduiList?.NavMenu == true)
            {
                HtmlTag = "div";
                BodyHtmlTag = "div";
            }
            base.OnInitialized();
        }

        private void OnTitleClicked()
        {
            Open = !Open;
        }

        public void AddItem()
        {
            ChildItemCount++;
        }

        public void RemoveItem()
        {
            ChildItemCount--;
        }
    }
}
