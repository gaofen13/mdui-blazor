namespace MduiBlazor
{
    internal interface IPicker<TValue>
    {
        bool MultiSelection { get; set; }
        bool Searchable { get; set; }
        bool DisableRipple { get; set; }
        TValue? Value { get; set; }
        string? DisplayValue { get; set; }
        IEnumerable<TValue?>? SelectedValues { get; set; }
        void AddItem(MduiPickerItem<TValue> item);
        void RemoveItem(MduiPickerItem<TValue> item);
        void SetValue(MduiPickerItem<TValue> item);
        void AddSelectedValue(TValue? value);
        void RemoveSelectedValue(TValue? value);
    }
}