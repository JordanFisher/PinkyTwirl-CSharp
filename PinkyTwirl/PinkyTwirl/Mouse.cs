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
    public class Mouse
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void ClickConsoleMenu()
        {
            var TL = WindowFunctions.GetWindowTL();
            
            var MenuPos = TL;
            MenuPos.X += 16;
            MenuPos.Y += 16;

            var AppBarPos = TL;
            AppBarPos.X += 60;
            AppBarPos.Y += 16;

            var HoldPos = Cursor.Position;

            Cursor.Position = AppBarPos;
            MouseLeftClick();

            Cursor.Position = MenuPos;
            MouseLeftClick();

            Cursor.Position = HoldPos;
        }

        public static void MouseLeftClick() { MouseLeftClick(Cursor.Position); }
        public static void MouseLeftClick(Point pos)
        {
            DoLeftMouseClick_Down(pos);
            DoLeftMouseClick_Up(pos);
        }

        public static void DoLeftMouseClick_Down(Point pos)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)pos.X, (uint)pos.Y, 0, 0);
        }

        public static void DoLeftMouseClick_Up(Point pos)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)pos.X, (uint)pos.Y, 0, 0);
        }

        public static void MouseRightClick()
        {
            DoRightMouseClick_Down();
            DoRightMouseClick_Up();
        }

        public static void DoRightMouseClick_Down()
        {
            // Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN, X, Y, 0, 0);
        }

        public static void DoRightMouseClick_Up()
        {
            // Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }
    }
}