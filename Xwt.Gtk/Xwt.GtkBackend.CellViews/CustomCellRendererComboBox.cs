using System;
using Xwt.Backends;
using System.Linq;

namespace Xwt.GtkBackend
{
    public class CustomCellRendererComboBox : CellViewBackend
    {
        Gtk.CellRendererCombo cellRenderer;
        
        public CustomCellRendererComboBox()
        {
            CellRenderer = cellRenderer = new Gtk.CellRendererCombo ();
            cellRenderer.Edited += HandleEdit;
        }
        
        protected override void OnLoadData ()
        {
            var view = (IComboBoxCellViewFrontend) Frontend;
            
            var store = new Gtk.ListStore(typeof(string));
            if(view.AllowEmpty)
            {
                store.AppendValues(EmptyText);
            }
            
            Array.ForEach(view.Items, x => store.AppendValues(x.ToString()));
            
            cellRenderer.Model = store;
            cellRenderer.HasEntry = true;
            cellRenderer.TextColumn = 0;
            cellRenderer.Editable = true;
            
            if(view.Selected != null)
            {
                cellRenderer.Text = view.Selected.ToString();
            }
            else
            {
                Gtk.TreeIter firstIter;
                if(store.GetIterFirst(out firstIter))
                {
                    cellRenderer.Text = (string)store.GetValue(firstIter, 0);
                }
            }
        }
        
        private void HandleEdit (object o, Gtk.EditedArgs args)
        {
            var view = (IComboBoxCellViewFrontend) Frontend;
            
            if(view.Items.All(x => x.ToString() != args.NewText) && !(view.AllowEmpty && args.NewText == EmptyText))
            {
                // skip values not included in Items
                return;
            }
            
            SetCurrentEventRow ();
            Gtk.TreeIter iter;
            if (TreeModel.GetIterFromString (out iter, args.Path))
            {
                if (!view.RaiseValueChanged(int.Parse(args.Path), args.NewText == EmptyText ? null : args.NewText))
                {
                    TreeModel.SetValue(iter, view.ValueField.Index, new ComboBoxCellViewElement(args.NewText));
                }
            }
        }
        
        private const string EmptyText = "-";
    }
}

