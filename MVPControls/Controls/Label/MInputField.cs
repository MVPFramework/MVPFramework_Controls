using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MVPControls.Controls.Label
{
    public partial class MInputField : Control
    {
        public MInputField()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }

    //private class BaseTextBox:TextBox
    //{
    //    private const int EM_SETCUEBANNER = 0x1501;
    //    private const char EmptyChar = (char)0;
    //    private const char VisualStylePasswordChar = '\u25CF';
    //    private const char NonVisualStylePasswordChar = '\u002A';

    //    private string hint = string.Empty;
    //    public string Hint
    //    {
    //        get { return hint; }
    //        set
    //        {
    //            hint = value;
    //            Win32.SendMessage(Handle, EM_SETCUEBANNER, (int)IntPtr.Zero, Hint);
    //        }
    //    }

    //    private char _passwordChar = EmptyChar;
    //    public new char PasswordChar
    //    {
    //        get { return _passwordChar; }
    //        set
    //        {
    //            _passwordChar = value;
    //            SetBasePasswordChar();
    //        }
    //    }

    //    private char _useSystemPasswordChar = EmptyChar;
    //    public new bool UseSystemPasswordChar
    //    {
    //        get { return _useSystemPasswordChar != EmptyChar; }
    //        set
    //        {
    //            if (value)
    //            {
    //                _useSystemPasswordChar = Application.RenderWithVisualStyles? VisualStylePasswordChar : NonVisualStylePasswordChar;
    //            }
    //        }
    //    }

    //    private void SetBasePasswordChar()
    //    {
    //        //base.PasswordChar = 
    //    }
    //}
}
