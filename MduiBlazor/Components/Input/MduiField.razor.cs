using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiField
    {
        private bool _invalid;

        protected string Classname =>
          new ClassBuilder("mdui-textfield")
            .AddClass("mdui-textfield-has-bottom", !string.IsNullOrWhiteSpace(HelperText))
            .AddClass("mdui-textfield-invalid", _invalid)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? ErrorText { get; set; }

        [Parameter]
        public string? HelperText { get; set; }
    }
}
