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
            .AddClass("mdui-ripple", !Root.DisableRipple)
            .AddClass("mdui-treeview-item-checked", _checked)
            .AddClass("mdui-treeview-item-active", _actived)
            .Build();

        public bool Actived => _actived;

        public bool Checked => _checked;

        private bool HasChildren => ChildContent is not null;

        [CascadingParameter]
        private MduiTreeView<TValue> Root { get; set; } = default!;

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
            if (Root.MultiSelection)
            {
                if (Root.SelectedValues.Contains(Value))
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
                if (Value?.Equals(Root.SelectedValue) == true)
                {
                    _actived = true;
                    await Root.SetActivedItemAsync(this);
                    if (Parent is not null)
                    {
                        await Parent.OpenAsync();
                    }
                }
            }
        }

        private async void OnCheckedChanged(bool @checked)
        {
            _checked = !_checked;
            await Root.OnCheckedItemsChangedAsync(this);
        }

        private async Task OnClickItemAsync(MouseEventArgs args)
        {
            if (!Root.MultiSelection)
            {
                SetActive(!_actived);
                await Root.SetActivedItemAsync(this);
            }
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