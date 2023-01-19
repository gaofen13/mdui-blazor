using System.Linq.Expressions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiField : MduiComponentBase
    {
        public bool Invalid { get; private set; }

        protected string Classname =>
          new ClassBuilder("mdui-textfield")
            .AddClass("mdui-textfield-has-bottom", Invalid || !string.IsNullOrWhiteSpace(HelperText))
            .AddClass("mdui-textfield-invalid", Invalid)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public Expression<Func<object>>? For { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public bool LabelRequired { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? ErrorText { get; set; }

        [Parameter]
        public string? HelperText { get; set; }

        public void SetInvalid()
        {
            Invalid = true;
            StateHasChanged();
        }

        public void RemoveInvalid()
        {
            Invalid = false;
            StateHasChanged();
        }
    }
}
