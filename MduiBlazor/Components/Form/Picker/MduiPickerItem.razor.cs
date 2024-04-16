using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPickerItem<TValue> : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-picker-item")
            .AddClass("mdui-picker-item-active", Actived)
            .AddClass("mdui-ripple", Picker?.DisableRipple == false)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string Stylelist =>
            new StyleBuilder()
            .AddStyle("display", "none", !Show)
            .AddStyle(Style)
            .Build();

        private bool Actived => Picker is not null && ((Value == null && Picker.Value == null) || Value?.Equals(Picker.Value) == true);

        private bool Checked => Picker?.MultiSelection == true && Picker.SelectedValues?.Contains(Value) == true;

        private bool Show
        {
            get
            {
                if (Picker?.Searchable == false)
                {
                    return true;
                }
                if (string.IsNullOrEmpty(Picker?.DisplayValue) || DisplayString?.Contains(Picker.DisplayValue) == true)
                {
                    return true;
                }
                return false;
            }
        }

        [CascadingParameter]
        private MduiPicker<TValue>? Picker { get; set; }

        [Parameter, EditorRequired]
        public TValue? Value { get; set; }

        [Parameter, EditorRequired]
        public string? DisplayString { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
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

        void IDisposable.Dispose()
        {
            Picker?.RemoveItem(this);
            GC.SuppressFinalize(this);
        }
    }
}