using Microsoft.AspNetCore.Components;
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

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        protected override void OnParametersSet()
        {
            errorBoundary?.Recover();
            base.OnParametersSet();
        }

        private void ReflashPage()
        {
            Navigation.NavigateTo(Navigation.Uri, true, true);
        }
    }
}
