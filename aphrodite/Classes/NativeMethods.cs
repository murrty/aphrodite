using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace aphrodite {
    class NativeMethods {

        #region TextBox Hint
        [DllImport("user32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, string lParam);
        #endregion

        #region Ini File Controller
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        #endregion

        #region System Hand Cursor for LinkLabelHand and Other Controls
        public static IntPtr IDC_HAND = (IntPtr)32649;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetCursor(IntPtr hCursor);
        public static readonly IntPtr SystemHand = LoadCursor(IntPtr.Zero, IDC_HAND);
        public static readonly Cursor SystemHandCursor = new(LoadCursor(IntPtr.Zero, IDC_HAND));
        #endregion

        #region Vista Visuals for AeroListBox
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 SetWindowTheme(IntPtr hWnd, String textSubAppName, String textSubIdList);
        #endregion

        #region ExtendedProgressBar State, Button UAC Shield, TextBox Text Margins for ButtonTextBox, Auto-Scrolling for ExtendedRichTextBox
        
        #region UAC Shield for Buttons
        public const int BCM_FIRST = 0x1600;
        public const int BCM_SETSHIELD = (BCM_FIRST + 0x000c);
        #endregion

        #region TextBox Text Margins
        public const int EM_SETMARGINS = 0xd3;
        public const int EC_RIGHTMARGIN = 2;
        public const int EC_LEFTMARGIN = 1;
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion
    }
}
