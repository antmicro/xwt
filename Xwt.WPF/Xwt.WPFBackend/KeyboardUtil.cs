// 
// KeyboardUtil.cs
//  
// Author:
//       Carlos Alberto Cortez <calberto.cortez@gmail.com>
// 
// Copyright (c) 2011 Carlos Alberto Cortez
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

using WpfKey = System.Windows.Input.Key;

namespace Xwt.WPFBackend
{
	using Keyboard = System.Windows.Input.Keyboard;

	public static class KeyboardUtil
	{
		// Windows API for getting the actual character from keyboard layout (works for ALL keyboard layouts)
		[DllImport("user32.dll")]
		private static extern int ToUnicode(
			uint wVirtKey,
			uint wScanCode,
			byte[] lpKeyState,
			[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder pwszBuff,
			int cchBuff,
			uint wFlags);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		public static ModifierKeys GetModifiers ()
		{
			var modifiers = ModifierKeys.None;
			var wpfModifiers = Keyboard.Modifiers;
			if ((wpfModifiers & System.Windows.Input.ModifierKeys.Alt) > 0)
				modifiers |= ModifierKeys.Alt;
			if ((wpfModifiers & System.Windows.Input.ModifierKeys.Control) > 0)
				modifiers |= ModifierKeys.Control;
			if ((wpfModifiers & System.Windows.Input.ModifierKeys.Shift) > 0)
				modifiers |= ModifierKeys.Shift;

			return modifiers;
		}

		private static string GetCharFromKeyboardLayout(WpfKey key)
		{
			if (key == WpfKey.None)
				return string.Empty;

			try
			{
				var virtualKey = KeyInterop.VirtualKeyFromKey(key);
				var keyboardState = new byte[256];
				
				// Get current keyboard state
				if ((Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != 0)
					keyboardState[0x11] = 0x80; // VK_CONTROL
				if ((Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Alt) != 0)
					keyboardState[0x12] = 0x80; // VK_MENU (Alt)
				if ((Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) != 0)
					keyboardState[0x10] = 0x80; // VK_SHIFT
				if (Keyboard.IsKeyToggled(WpfKey.CapsLock))
					keyboardState[0x14] = 0x01; // VK_CAPITAL

				var scanCode = MapVirtualKey((uint)virtualKey, 0);
				var buffer = new StringBuilder(64);
				
				var result = ToUnicode((uint)virtualKey, scanCode, keyboardState, buffer, 64, 0);
				
				if (result > 0)
					return buffer.ToString(0, result);
			}
			catch
			{
				// Silently fail
			}
			
			return string.Empty;
		}

		public static string GetCharacterFromKey(WpfKey key)
		{
			return GetCharFromKeyboardLayout(key);
		}

		const int D_Pos = (int)Key.K0 - (int)WpfKey.D0;
		const int U_Pos = (int)Key.A - (int)WpfKey.A;
		const int L_Pos = (int)Key.a - (int)WpfKey.A;
		const int N_Pos = (int)Key.NumPad0 - (int)WpfKey.NumPad0;
		const int F_Pos = (int)Key.F1 - (int)WpfKey.F1;

		public static Key TranslateToXwtKey (WpfKey key)
		{
			// Special non-printable keys - direct mapping
			// F- keys
			if (key >= WpfKey.F1 && key <= WpfKey.F10)
				return (Key)(key + F_Pos);
			
			// Numpad- keys
			if (key >= WpfKey.NumPad0 && key <= WpfKey.NumPad9)
				return (Key)(key + N_Pos);

			switch (key) {
				case WpfKey.Cancel: return Key.Cancel;
				case WpfKey.Back: return Key.BackSpace;
				case WpfKey.Tab: return Key.Tab;
				case WpfKey.LineFeed: return Key.LineFeed;
				case WpfKey.Clear: return Key.Clear;
				case WpfKey.Return: return Key.Return;
				case WpfKey.Pause: return Key.Pause;
				case WpfKey.CapsLock: return Key.CapsLock;
				case WpfKey.Space: return Key.Space;
				case WpfKey.Escape: return Key.Escape;
				case WpfKey.PageDown: return Key.PageDown;
				case WpfKey.PageUp: return Key.PageUp;
				case WpfKey.End: return Key.End;
				case WpfKey.Home: return Key.Home;
				case WpfKey.Left: return Key.Left;
				case WpfKey.Up: return Key.Up;
				case WpfKey.Right: return Key.Right;
				case WpfKey.Down: return Key.Down;
				case WpfKey.Select: return Key.Select;
				case WpfKey.Print: return Key.Print;
				case WpfKey.Execute: return Key.Execute;
				case WpfKey.Delete: return Key.Delete;
				case WpfKey.Help: return Key.Help;
				case WpfKey.Insert: return Key.Insert;
				case WpfKey.Scroll: return Key.ScrollLock;
				case WpfKey.NumLock: return Key.NumLock;
				case WpfKey.LeftShift: return Key.ShiftLeft;
				case WpfKey.RightShift: return Key.ShiftRight;
				case WpfKey.LeftCtrl: return Key.ControlLeft;
				case WpfKey.RightCtrl: return Key.ControlRight;
				case WpfKey.LeftAlt: return Key.AltLeft;
				case WpfKey.RightAlt: return Key.AltRight;
                
				case WpfKey.Multiply: return Key.NumPadMultiply;
				case WpfKey.Add: return Key.NumPadAdd;
				case WpfKey.Subtract: return Key.NumPadSubtract;
				case WpfKey.Divide: return Key.NumPadDivide;
				case WpfKey.Decimal: return Key.NumPadDecimal;
			}
			
			// For printable keys (letters, digits, symbols), use ToUnicode to get the actual character
			// from the current keyboard layout
			var charStr = GetCharFromKeyboardLayout(key);
			if (!string.IsNullOrEmpty(charStr))
			{
				var ch = charStr[0];
				// Map character to corresponding Key enum value
				// Some characters like @ # etc. have specific Key enum values that don't match ASCII
				switch (ch)
				{
					case '!': return Key.Exclamation;
					case '@': return Key.At;
					case '#': return Key.Hash;
					case '$': return Key.Dollar;
					case '%': return Key.Percent;
					case '^': return Key.Caret;
					case '&': return Key.Ampersand;
					case '*': return Key.Asterisk;
					case '(': return Key.LeftBracket;
					case ')': return Key.RightBracket;
					case '-': return Key.Minus;
					case '_': return Key.Underscore;
					case '=': return Key.Equal;
					case '+': return Key.Plus;
					case '[': return Key.OpenSquareBracket;
					case '{': return Key.OpenCurlyBracket;
					case ']': return Key.CloseSquareBracket;
					case '}': return Key.CloseCurlyBracket;
					case '\\': return Key.Backslash;
					case '|': return Key.Pipe;
					case ';': return Key.Semicolon;
					case ':': return Key.Colon;
					case '\'': return Key.SingleQuote;
					case '"': return Key.Quote;
					case ',': return Key.Comma;
					case '<': return Key.Less;
					case '.': return Key.Period;
					case '>': return Key.Greater;
					case '/': return Key.Slash;
					case '?': return Key.Question;
					case '`': return Key.BackQuote;
					case '~': return Key.Tilde;
				}
				
				// For other printable ASCII characters (letters, digits, space)
				if (ch >= 32 && ch <= 126)
					return (Key)ch;
			}
			
			// Fallback for letters if ToUnicode failed
			if (key >= WpfKey.A && key <= WpfKey.Z) {
				bool upperCase = Keyboard.IsKeyToggled (WpfKey.CapsLock);
				if ((Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Shift) > 0)
					upperCase = !upperCase;
				return upperCase ? (Key)(key + U_Pos) : (Key)(key + L_Pos);
			}
			
			// Fallback for digits if ToUnicode failed
			if (key >= WpfKey.D0 && key <= WpfKey.D9)
				return (Key)(key + D_Pos);

			return (Key)0;
		}
	}
}
