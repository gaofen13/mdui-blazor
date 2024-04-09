namespace MduiBlazor
{
    public interface ITable<TItem>
    {
        bool MultiSelection { get; set; }
        
        IEnumerable<TItem>? Items { get; set; }

        IEnumerable<TItem> SelectedItems { get; set; }

        void AddSelectedItem(TItem item);

        void RemoveSelectedItem(TItem item);

        void SelectAllItems();

        void ClearSelectedItems();
    }
}
