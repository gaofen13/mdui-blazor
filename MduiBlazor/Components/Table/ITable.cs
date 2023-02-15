namespace MduiBlazor
{
    public interface ITable<TItem>
    {
        void AddSelectedItem(TItem item);

        void RemoveSelectedItem(TItem item);

        void SelectAllItems();

        void ClearSelectedItems();
    }
}
