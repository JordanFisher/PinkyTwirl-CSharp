using System;
using System.Windows.Forms;
using System.Collections.Generic;
using WindowsInput;

namespace PinkyTwirl
{
    public class PinkyTwirlApp : Base
    {
        public bool Skip = false;

        KeyMap CurrentMap = null;
        List<Key> AmbiguousKeys = new List<Key>();

        bool DoLog { get { return PinkyTwirlForm.TheForm.DoLog; } }
        void Log(string s) { PinkyTwirlForm.TheForm.Log(s); }
        void Log(Exception e) { Log("\nError uncaught!" + e.ToString() + '\n'); }

        public void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (Skip) return;

            //if (e.KeyCode == Keys.I) Console.Write("");

            try
            {
                // Shift-Shift = CapsLock
                if (CheckForShiftShift(e)) return;

                // Log event info
                if (DoLog) Log(string.Format("Window name is {0}", WindowFunctions.GetActiveWindowTitle()));
                if (DoLog) Log(string.Format("KeyDown {0} ({1}) {2}{3}{4}", e.KeyCode, (char)e.KeyValue,
                    Key.Shift.IsDown ? " Shift" : "",
                    Key.Alt.IsDown   ? " Alt"   : "",
                    Key.Ctrl.IsDown  ? " Cntrl" : ""));

                if (CurrentMap == null)
                {
                    CurrentMap = Base.ActiveContextMap;
                }

                if (CurrentMap.ContainsKey(e.Key()))
                {
                    var result = CurrentMap[e.Key()];
                    if (result.MyAction == null)
                    {
                        CurrentMap = result.MyMap;
                        AmbiguousKeys.Add(e.Key());

                        Suppress(e);
                    }
                    else
                    {
                        Skip = true;
                        result.MyAction.Execute();
                        Skip = false;

                        Suppress(e);
                        Reset();
                    }
                }
                else
                {
                    SendAmbiguousKeys();
                    Reset();
                }
            }
            catch (Exception exc)
            {
                Log(exc);
            }
        }

        public void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (Skip) return;

            try
            {
                if (CurrentMap != null)
                {
                     keep full sequence of key presses and walk tree each time? this way can back out of multi key chord and still have other chords available
                    only reset if first key is let up? or any key in the tree leading up to this besides action (child) node keys
                    SendAmbiguousKeys();
                    Reset();
                }
            }
            catch (Exception exc)
            {
                Log(exc);
            }
        }

        private void SendAmbiguousKeys()
        {
            if (AmbiguousKeys.Count > 0)
            {
                Skip = true;
                foreach (var k in AmbiguousKeys)
                {
                    k.DoPress();
                }
                Skip = false;
            }
        }

        private static void Suppress(KeyEventArgs e)
        {
            e.Handled = true; e.SuppressKeyPress = true;
        }

        private void Reset()
        {
            CurrentMap = null;
            AmbiguousKeys.Clear();
        }

        private bool CheckForShiftShift(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey && Key.RShift.IsDown && Key.LShift.IsUp ||
                e.KeyCode == Keys.RShiftKey && Key.LShift.IsDown && Key.RShift.IsUp)
            {
                Skip = true;
                Key.CapsLock.DoPress();
                Skip = false;

                return true;
            }

            return false;
        }
    }
}