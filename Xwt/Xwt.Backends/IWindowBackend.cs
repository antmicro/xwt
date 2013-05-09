// 
// IWindowBackend.cs
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
using Xwt;

namespace Xwt.Backends
{
	public interface IWindowBackend: IWindowFrameBackend
	{
		void SetChild (IWidgetBackend child);
		void SetMainMenu (IMenuBackend menu);
		void SetPadding (double left, double top, double right, double bottom);

		/// <summary>
		/// Gets the minimum size of the window, without including the content
		/// </summary>
		/// <remarks>
		/// This property should return the minimum size of the decorations around
		/// the content widget. It should include for example the minimum size required
		/// by the menu bar, dialog button bar, etc. Do not include content padding.
		/// </remarks>
		Size ImplicitMinSize { get; }

		/// <summary>
		/// Sets the minimum size of the window
		/// </summary>
		void SetMinSize (Size size);
	}
	
	public interface IWindowEventSink: IWindowFrameEventSink
	{
	}
}

