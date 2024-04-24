using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTreePickerItem<TValue> : MduiComponentBase, IDisposable
    {
        private bool _isExpanded;
        protected string Classname =>
            new ClassBuilder("mdui-picker-item")
            .AddClass("mdui-picker-item-active", Actived)
            .AddClass("mdui-ripple", Picker?.DisableRipple == false)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private bool Actived => Picker is not null && ((Value == null && Picker.Value == null) || Value?.Equals(Picker.Value) == true);

        private bool Checked => Picker?.MultiSelection == true && Picker.SelectedValues?.Contains(Value) == true;

        [CascadingParameter]
        private ITreePicker<TValue> Picker { get; set; } = default!;

        [CascadingParameter]
        private MduiTreePickerItem<TValue>? Parent { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public string? DisplayString { get; set; }

        [Parameter]
        public RenderFragment? OptionContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Picker.MultiSelection)
            {
                if (Picker.SelectedValues?.Contains(Value) == true)
                {
                    if (Parent is not null)
                    {
                        await Parent.OpenAsync();
                    }
                }
            }
            else
            {
                if (Value?.Equals(Picker.Value) == true)
                {
                    if (Parent is not null)
                    {
                        await Parent.OpenAsync();
                    }
                }
            }
        }

        protected override void OnInitialized()
        {
            Picker?.AddItem(this);
        }

        private void OnClickedItem()
        {
            if (Picker?.MultiSelection == true)
            {
                if (Checked)
                {
                    OnCheckedChanged(false);
                }
                else
                {
                    OnCheckedChanged(true);
                }
            }
            else
            {
                Picker?.SetValue(this);
            }
        }

        private void OnCheckedChanged(bool @checked)
        {
            if (@checked)
            {
                Picker?.AddSelectedValue(Value);
            }
            else
            {
                Picker?.RemoveSelectedValue(Value);
            }
        }

        internal async Task OpenAsync()
        {
            if (!_isExpanded)
            {
                _isExpanded = true;
                await InvokeAsync(StateHasChanged);
                if (Parent != null)
                {
                    await Parent.OpenAsync();
                }
            }
        }

        void IDisposable.Dispose()
        {
            Picker?.RemoveItem(this);
            GC.SuppressFinalize(this);
        }
    }
}