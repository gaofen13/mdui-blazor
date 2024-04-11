namespace MduiBlazor
{
    internal interface ITreeTable
    {
        bool HasTreeData { get; }

        int IndentSize { get; set; }
    }
}