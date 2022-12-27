using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor.Shared.Shared
{
    public partial class MainLayout
    {
        bool open;
        ErrorBoundary? errorBoundary;

        protected override void OnParametersSet()
        {
            errorBoundary?.Recover();
            base.OnParametersSet();
        }
    }
}
