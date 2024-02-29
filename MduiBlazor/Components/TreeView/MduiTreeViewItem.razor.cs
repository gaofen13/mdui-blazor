using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiTreeViewItem<TValue> : MduiComponentBase
    {
        private bool _opened;

        private bool _actived;

        private bool _checked;

        private string TitleClassname =>
            new ClassBuilder("mdui-treeview-item-content")
            .AddClass("mdui-ripple", !TreeRoot.DisableRipple)
            .AddClass("mdui-treeview-item-checked", _checked)
            .AddClass("mdui-treeview-item-active", _actived)
            .Build();

        public bool Actived => _actived;

        public bool Checked => _checked;

        private bool HasChildren => ChildContent is not null;

        [CascadingParameter]
        private MduiTreeView<TValue> TreeRoot { get; set; } = default!;

        [CascadingParameter]
        private MduiTreeViewItem<TValue>? Parent { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public RenderFragment? TitleContent { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (TreeRoot.MultiSelection)
            {
                if (TreeRoot.SelectedValues.Contains(Value))
                {
                    _checked = true;
                    if (Parent is not null)
                    {
                        await Parent.OpenAsync();
                    }
                }
            }
            else
            {
                if (Value?.Equals(TreeRoot.SelectedValue) == true)
                {
                    _actived = true;
                    TreeRoot.SetActivedItem(this);
                    if (Parent is not null)
                    {
                        await Parent.OpenAsync();
                    }
                }
            }
        }

        private void OnOpenChanged(MouseEventArgs args)
        {
            if (HasChildren)
            {
                _opened = !_opened;
            }
        }

        private async Task OnClickItemAsync(MouseEventArgs args)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }
            if (TreeRoot.MultiSelection)
            {
                _checked = !_checked;
            }
            else
            {
                SetActive(!_actived);
            }
            await TreeRoot.OnActivedItemChangedAsync(this);
        }

        internal void SetActive(bool actived)
        {
            if (_actived != actived)
            {
                _actived = actived;
                StateHasChanged();
            }
        }

        public async Task OpenAsync()
        {
            if (!_opened)
            {
                _opened = true;
                await InvokeAsync(StateHasChanged);
                if (Parent != null)
                {
                    await Parent.OpenAsync();
                }
            }
        }
    }
}