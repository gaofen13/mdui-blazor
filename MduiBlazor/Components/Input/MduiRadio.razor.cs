using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiRadio<TValue> : MduiComponentBase
    {
        protected string Classname =>
          new ClassBuilder("mdui-radio")
            .AddClass(Class)
            .Build();

        [CascadingParameter(Name = "Field")]
        protected MduiField? Field { get; set; }

        [CascadingParameter(Name = "RadioGroup")]
        private MduiRadioGroup<TValue>? RadioGroup { get; set; }

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public bool Readonly { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (RadioGroup != null)
            {
                Checked = EqualityComparer<TValue>.Default.Equals(Value, RadioGroup.Value);
            }
        }

        protected override void OnInitialized()
        {
            if (RadioGroup?.Disabled == true)
            {
                Disabled = true;
            }
            base.OnInitialized();
        }

        private void OnRadioChanged(ChangeEventArgs args)
        {
            RadioGroup?.OnCheckedRadioChanged(this);
        }

        private void OnInputFocus(FocusEventArgs args)
        {
            Field?.SetFocus();
            if (OnFocus.HasDelegate)
            {
                OnFocus.InvokeAsync(args);
            }
        }

        private void OnInputBlur(FocusEventArgs args)
        {
            Field?.SetBlur();
            if (OnBlur.HasDelegate)
            {
                OnBlur.InvokeAsync(args);
            }
        }
    }
}
