using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using HookManager;

using WindowsInput;

#if !NO_GAME
using input = Microsoft.Xna.Framework.Input;
using PinkyGame;
#endif

namespace PinkyTwirl
{
    public partial class WindowFunctions
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        public static Point GetWindowTL()
        {
            IntPtr handle = GetForegroundWindow();

            Rectangle rect = new Rectangle();
            GetWindowRect(handle, out rect);

            return new Point(rect.Left, rect.Top);
        }

        public static Point GetWindowTR()
        {
            IntPtr handle = GetForegroundWindow();

            Rectangle rect = new Rectangle();
            GetWindowRect(handle, out rect);

            return new Point(rect.Right, rect.Top);
        }

        public static void ShiftWindow(int x, int y, int width, int height)
        {
            IntPtr handle = GetForegroundWindow();

            Rectangle rect = new Rectangle();
            GetWindowRect(handle, out rect);

            MoveWindow(handle, rect.X + x, rect.Y + y, rect.Width - rect.X + width, rect.Height - rect.Y + height, true);
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public static Tuple<string, string> GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            string description;
            try
            {
                int processId = 0;
                int threadId = GetWindowThreadProcessId(handle, out processId);
                var proc = System.Diagnostics.Process.GetProcessById(processId);
                description = proc.MainModule.FileVersionInfo.FileDescription;
            }
            catch (Exception)
            {
                description = "";
            }

            if (GetWindowText(handle, Buff, nChars) > 0)
                return new Tuple<string, string>(Buff.ToString(), description);
            else
                return new Tuple<string, string>(description, description); ;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);
    }
}