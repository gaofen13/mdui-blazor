using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiNavMenuCollapse : MduiComponentBase, IDisposable
    {
        private readonly List<MduiNavMenuCollapse> _subItems = [];

        private string HeaderClassname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();

        private string HeaderStylelist =>
            new StyleBuilder()
            .AddStyle("padding-left", $"{Level * 36}px", Parent is not null)
            .Build();

        private string BodyClassname =>
            new ClassBuilder("mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .Build();

        internal int Level { get; set; }

        private bool HasParent => Parent is not null;

        [CascadingParameter]
        private MduiNavMenu? NavMenu { get; set; }

        [CascadingParameter]
        private MduiNavMenuCollapse? Parent { get; set; }

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
        public RenderFragment? IconContent { get; set; }

        protected override void OnInitialized()
        {
            if (HasParent)
            {
                Parent?.AddSubitem(this);
                Level = Parent!.Level + 1;
            }
            else
            {
                NavMenu?.AddSubitem(this);
            }
            base.OnInitialized();
        }

        private void OnTitleClicked()
        {
            if (!Open && NavMenu?.Accordion == true)
            {
                if (HasParent)
                {
                    Parent?.CloseAllSubitems();
                }
                else
                {
                    NavMenu.CloseAllSubitems();
                }
            }
            Open = !Open;
        }

        public void SetOpen()
        {
            if (!Open)
            {
                Open = true;
                InvokeAsync(StateHasChanged);
            }
        }

        public void Close()
        {
            Open = false;
        }

        public void AddSubitem(MduiNavMenuCollapse item)
        {
            if (!_subItems.Contains(item))
            {
                _subItems.Add(item);
            }
        }

        public void RemoveSubitem(MduiNavMenuCollapse item)
        {
            _subItems.Remove(item);
        }

        public void CloseAllSubitems()
        {
            var openedItems = _subItems.Where(i => i.Open);
            if (openedItems.Any())
            {
                foreach (var item in openedItems)
                {
                    item.Close();
                }
                StateHasChanged();
            }
        }

        void IDisposable.Dispose()
        {
            if (HasParent)
            {
                Parent?.RemoveSubitem(this);
            }
            else
            {
                NavMenu?.RemoveSubitem(this);
            }
            GC.SuppressFinalize(this);
        }
    }
}
