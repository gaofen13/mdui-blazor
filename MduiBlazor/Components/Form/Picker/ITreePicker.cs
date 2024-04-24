namespace MduiBlazor
{
    internal interface ITreePicker<TValue>
    {
        bool MultiSelection { get; set; }
        bool DisableRipple { get; set; }
        TValue? Value { get; set; }
        string? DisplayValue { get; set; }
        string Indent { get; set; }
        IEnumerable<TValue?>? SelectedValues { get; set; }
        void AddItem(MduiTreePickerItem<TValue> item);
        void RemoveItem(MduiTreePickerItem<TValue> item);
        void SetValue(MduiTreePickerItem<TValue> item);
        void AddSelectedValue(TValue? value);
        void RemoveSelectedValue(TValue? value);
    }
}