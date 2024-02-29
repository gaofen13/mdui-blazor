using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListCollapse : MduiComponentBase, IDisposable
    {
        private readonly List<MduiListCollapse> _subItems = [];

        private string HeaderClassname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-list-item-has-icon", Icon is not null || IconContent is not null)
            .AddClass("mdui-ripple", !DisableRipple)
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

        private bool HasIcon => Icon is not null || IconContent is not null;

        [CascadingParameter]
        private MduiList? List { get; set; }

        [CascadingParameter]
        private MduiListCollapse? Parent { get; set; }

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
                List?.AddSubitem(this);
            }
            base.OnInitialized();
        }

        private void OnTitleClicked()
        {
            if (!Open && List?.Accordion == true)
            {
                if (HasParent)
                {
                    Parent?.CloseAllSubitems();
                }
                else
                {
                    List.CloseAllSubitems();
                }
            }
            Open = !Open;
        }

        public void Close()
        {
            Open = false;
        }

        public void AddSubitem(MduiListCollapse item)
        {
            if (!_subItems.Contains(item))
            {
                _subItems.Add(item);
            }
        }

        public void RemoveSubitem(MduiListCollapse item)
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
                List?.RemoveSubitem(this);
            }
            GC.SuppressFinalize(this);
        }
    }
}
