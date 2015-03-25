using System;

namespace Xwt.Backends
{
    public interface IComboBoxCellViewFrontend : ICellViewFrontend
    {
        ComboBoxCellViewElement[] Items { get; }
        ComboBoxCellViewElement Selected { get; }
        bool AllowEmpty { get; }
        
        IDataField<ComboBoxCellViewElement> ValueField { get; }
        
        event EventHandler<ComboBoxCellViewValueChangedEventArgs> ValueChanged;

        bool RaiseValueChanged(int row, string value);
    }
    
    public class ComboBoxCellViewElement
    {
        public ComboBoxCellViewElement(object value)
        {
            this.value = value;
        }
        
        public ComboBoxCellViewElement(string title, object value) : this(value)
        {
            this.title = title;
        }
        
        public override string ToString()
        {
            return title ?? (value != null ? value.ToString() : null);
        }
        
        private readonly string title;
        private readonly object value;
    }
    
    public class ComboBoxCellViewValueChangedEventArgs : WidgetEventArgs
    {
        public string NewValue { get; set; }
        public int Row { get; set; }
    }
}

