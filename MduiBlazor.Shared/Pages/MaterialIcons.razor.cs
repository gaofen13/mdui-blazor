using MduiBlazor.Shared.Data;
using System.Text.Json;

namespace MduiBlazor.Shared.Pages
{
    public partial class MaterialIcons
    {
        private IEnumerable<MaterialIcon> _showIcons = Enumerable.Empty<MaterialIcon>();
        private IEnumerable<MaterialIcon> _icons = Enumerable.Empty<MaterialIcon>();

        protected override void OnInitialized()
        {
            var iconstring = File.ReadAllText("material_icon.json");

            var icons = JsonSerializer.Deserialize<MaterialIcon[]>(iconstring, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
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
    }
}