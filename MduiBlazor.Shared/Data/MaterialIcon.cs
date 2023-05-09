namespace MduiBlazor.Shared.Data
{
    public class MaterialIcon
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
    }

    public class MaterialIconList
    {
        public IEnumerable<MaterialIcon>? IconList { get; set; }
    }
}