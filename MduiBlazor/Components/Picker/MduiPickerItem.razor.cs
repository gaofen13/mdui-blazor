using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPickerItem<TValue> : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-picker-item")
            .AddClass("mdui-picker-item-active", Actived)
            .AddClass("mdui-ripple", !Picker.DisableRipple)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private bool Actived => Value!.Equals(Picker.Value);

        private bool Show
        {
            get
            {
                if (!Picker.Searchable)
                {
                    return true;
                }
                if (string.IsNullOrEmpty(Picker.DisplayValue) || Label?.Contains(Picker.DisplayValue) == true)
                {
                    return true;
                }
                return false;
            }
        }

        [CascadingParameter]
        private MduiPicker<TValue> Picker { get; set; } = default!;

        [Parameter, EditorRequired]
        public TValue Value { get; set; } = default!;

        [Parameter, EditorRequired]
        public string? Label { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Picker.AddItem(this);
        }

        private void OnClickedItem()
        {
            Picker.SetValue(this);
        }

        void IDisposable.Dispose()
        {
            Picker.RemoveItem(this);
            GC.SuppressFinalize(this);
        }
    }
}