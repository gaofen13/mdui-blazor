using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiRadio<TValue>
    {
        [CascadingParameter(Name = "RadioGroup")]
        private MduiRadioGroup<TValue>? RadioGroup { get; set; }

        [Parameter]
        public string? Id { get; set; }

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string? Label { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (RadioGroup != null)
            {
                Checked = EqualityComparer<TValue>.Default.Equals(Value, RadioGroup.Value);
            }
        }

        private void OnRadioChanged(ChangeEventArgs args)
        {
            RadioGroup?.OnCheckedRadioChanged(this);
        }
    }
}
