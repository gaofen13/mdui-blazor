namespace MduiBlazor
{
    public interface ITreeTableNode
    {
        ITreeTableNode? Parent { get; set; }
        
        bool HasChildren { get; }

        int Level { get; set; }

        bool IsExpanded { get; set; }

        void OnExpandedChanged(bool isExpanded);
    }
}