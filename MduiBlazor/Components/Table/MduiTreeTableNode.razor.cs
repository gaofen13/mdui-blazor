using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTreeTableNode<TItem> : MduiComponentBase, ITreeTableNode
    {
        private IEnumerable<TItem>? _children;

        private string Stylelist =>
            new StyleBuilder()
            .AddStyle("display", "none", Parent is not null && IsHideState(Parent))
            .Build();

        public bool HasChildren => _children?.Any() == true;

        public bool IsExpanded { get; set; }

        [CascadingParameter]
        private ITable<TItem> Table { get; set; } = default!;

        [CascadingParameter]
        public ITreeTableNode? Parent { get; set; }

        [Parameter, EditorRequired]
        public TItem Item { get; set; } = default!;

        [Parameter]
        public int Level { get; set; }

        protected override void OnInitialized()
        {
            _children = Table?.TreeChildren?.Invoke(Item);
            base.OnInitialized();
        }

        public void OnExpandedChanged(bool isExpanded)
        {
            if (IsExpanded != isExpanded)
            {
                IsExpanded = isExpanded;
                StateHasChanged();
            }
        }

        private static bool IsHideState(ITreeTableNode parent)
        {
            if(!parent.IsExpanded)
            {
                return true;
            }
            else if (parent.Parent is not null)
            {
                return IsHideState(parent.Parent);
            }
            return false;
        }
    }
}