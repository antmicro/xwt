// 
// ComboBoxCellView.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Xwt.Drawing;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xwt.Backends;
using System.Linq;

namespace Xwt {
    public sealed class ComboBoxCellView: CellView, IComboBoxCellViewFrontend
    {
        public ComboBoxCellViewElement[] Items {
            get {
                return GetValue(options, new ComboBoxCellViewElement[0]).ToArray();
            }
        }

        ComboBoxCellViewElement IComboBoxCellViewFrontend.Selected {
            get {
                var v = GetValue(ValueField);
                return v == null ? null : new ComboBoxCellViewElement(v);
            }
        }
        
        public IDataField<ComboBoxCellViewElement> ValueField { get; private set; }

        public bool AllowEmpty { get; set; }
        
        public ComboBoxCellView(IDataField<ComboBoxCellViewElement> valueField, IDataField<ComboBoxCellViewElement[]> optionsField)
        {
            ValueField = valueField;
            options = optionsField;
        }
        
        public event EventHandler<ComboBoxCellViewValueChangedEventArgs> ValueChanged;

        bool IComboBoxCellViewFrontend.RaiseValueChanged (int row, string value)
        {
            if (ValueChanged != null) {
                var args = new ComboBoxCellViewValueChangedEventArgs { Row = row, NewValue = value } ;
                ValueChanged (this, args);
                return args.Handled;
            }
            return false;
        }
        
        private readonly IDataField<ComboBoxCellViewElement[]> options;
    }
}
