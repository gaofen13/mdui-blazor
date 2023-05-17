using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Shared.Components
{
    public partial class PageContent
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}