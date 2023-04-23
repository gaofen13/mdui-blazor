using MduiBlazor.Shared.Data;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;

namespace MduiBlazor.Shared.Pages
{
    public partial class MaterialIcons
    {
        private bool _show;
        private string? _selectedIcon;
        private IEnumerable<MaterialIcon> _showIcons = Enumerable.Empty<MaterialIcon>();
        private IEnumerable<MaterialIcon> _icons = Enumerable.Empty<MaterialIcon>();

        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            HttpClient.BaseAddress = new Uri(Navigation.BaseUri);
            var icons = await HttpClient.GetFromJsonAsync<MaterialIcon[]>("_content/MduiBlazor.Shared/data/material_icon.json");
            if (icons != null)
            {
                _icons = icons;
                _showIcons = icons;
            }
        }

        private void OnSearchChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _showIcons = _icons;
            }
            else
            {
                _showIcons = _icons.Where(i => i.Name?.Contains(value.Trim()) == true);
            }
            StateHasChanged();
        }

        private void OnClickIconItem(string? name)
        {
            _show = true;
            _selectedIcon = name;
        }
    }
}