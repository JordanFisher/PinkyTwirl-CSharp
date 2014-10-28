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
    public partial class PinkyTwirlForm : Form
    {
        public static PinkyTwirlForm TheForm;
        public PinkyTwirlForm() : base()
        {
            InitializeComponent();

            TheForm = this;
            this.Visible = true;

#if WITH_GAME
            FormClosed += PinkyTwirlForm_FormClosed;
#endif

            Semantics.Initialize();
            Mappings.Inititalize();

            this.ActiveCheckbox.Checked = true;

#if WITH_GAME
            PinkyGame.Manager.OnRightJoystickMove += Manager_OnRightJoystickMove;
            PinkyGame.Manager.OnButtonPress += Manager_OnButtonPress;
            PinkyGame.Manager.OnButtonRelease += Manager_OnButtonRelease;
#endif
        }

#if WITH_GAME
        void Manager_OnButtonRelease(input.GamePadState obj)
        {
            if (input.Buttons.RightShoulder.Released())
            {
                Functions.EndCtrlTab();
            }
        }

        void Manager_OnButtonPress(input.GamePadState obj)
        {
            if (input.Buttons.RightShoulder.Down())
            {
                if (input.Buttons.LeftShoulder.Down()) 
                {
                    if (input.Buttons.Y.Pressed())
                    {
                        SendKeys.Send("{F5}");
                        SendKeys.Flush();
                    }
                    if (input.Buttons.B.Pressed())
                    {
                        Semantics.EndApplication.Do();
                    }
                }
                else
                {
                    if (input.Buttons.B.Pressed()) Functions.StartAltTab();
                    if (input.Buttons.Y.Pressed()) Functions.StartAltShiftTab();
                    if (input.Buttons.A.Pressed()) Mouse.MouseLeftClick();
                    if (input.Buttons.X.Pressed()) Mouse.MouseRightClick();
                }
            }
        }

        void Manager_OnRightJoystickMove(input.GamePadState state)
        {
            if (input.Buttons.RightShoulder.Down())
            {
                var v = state.ThumbSticks.Right * 10;
                if (v.X > 0 && v.X < 1) v.X = 1;
                if (v.X < 0 && v.X > -1) v.X = -1;
                if (v.Y > 0 && v.Y < 1) v.Y = 1;
                if (v.Y < 0 && v.Y > -1) v.Y = -1;

                WindowFunctions.ShiftWindow((int)v.X, -(int)v.Y, 0, 0);
            }
        }

        void PinkyTwirlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PinkyGame.Manager.End();
        }
#endif

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        new void Activate()
        {
            IoHooks.KeyDown += Base.App.HookManager_KeyDown;
            IoHooks.KeyUp += Base.App.HookManager_KeyUp;
        }

        new void Deactivate()
        {
            IoHooks.KeyDown -= Base.App.HookManager_KeyDown;
            IoHooks.KeyUp -= Base.App.HookManager_KeyUp;
        }

        public void Log(string str)
        {
            textBoxLog.AppendText(str);
            textBoxLog.AppendText("\n");
            textBoxLog.ScrollToCaret();
        }

        private void textBoxLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void Active_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ActiveCheckbox.Checked)
                Activate();
            else
                Deactivate();
        }

        public bool DoLog = false;
        private void LogCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            DoLog = this.LogCheckbox.Checked;
        }

        private void GamepadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
#if WITH_GAME
            if (this.GamepadCheckbox.Checked)
            {
                PinkyGame.Manager.Start();
            }
            else
            {
                PinkyGame.Manager.End();
            }
#endif
        }
    }
}