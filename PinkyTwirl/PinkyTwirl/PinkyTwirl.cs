using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using WindowsInput;

namespace PinkyTwirl
{
    public class PinkyTwirlApp : Base
    {
        public bool Skip = false;

        Context HoldContext = null;
        List<Key> PressedKeys = new List<Key>();
        bool Ambiguous = false;

        bool DoLog { get { return PinkyTwirlForm.TheForm.DoLog; } }
        void Log(string s) { PinkyTwirlForm.TheForm.Log(s); }
        void Log(Exception e) { Log("\nError uncaught!" + e.ToString() + '\n'); }

        public void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (Skip)
            {
                if (DoLog) Log("Skip Down " + e.Key().ToString());
                return;
            }

            //Skip = true;
            //Base.CurrentContext = Base.ActiveContext;
            ////InputSimulator.Keys.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
            ////InputSimulator.Keys.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_8);
            ////InputSimulator.Keys.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
            ////InputSimulator.Keys.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_8);
            ////D8.Do();
            ////(Ctrl + D8).Do();
            //Semantics.ViewProjectExplorer.Do();
            //Skip = false;
            //Suppress(e);
            //return;
            //Semantics.ViewProjectExplorer.Do();

            //if (e.KeyCode == Keys.I) Console.Write("");

            if (PressedKeys.Count > 0)
            {
                var Chord = PressedKeys.Aggregate("", (s, key) => s + key.Value + ',');
                if (DoLog) Log("Chord " + Chord + PressedKeys.Count.ToString());
            }

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

                KeyMap CurrentMap = null;
                if (HoldContext == null)
                {
                    Ambiguous = false;

                    HoldContext = Base.ActiveContext;
                    CurrentMap = Base.ActiveContextMap;

                    Base.CurrentContext = HoldContext;
                }
                else
                {
                    if (DoLog) Log(string.Format("CurrentContext is {0}", HoldContext.WindowName));

                    CurrentMap = Base.GetContextMap(HoldContext);

                    foreach (var k in PressedKeys)
                    {
                        if (CurrentMap == null) break;

                        if (CurrentMap.ContainsKey(k))
                        {
                            var result = CurrentMap[k];
                            if (result.MyMap != null)
                            {
                                CurrentMap = result.MyMap;
                            }
                            else
                            {
                                CurrentMap = null;
                                break;                                
                            }
                        }
                        else
                        {
                            CurrentMap = null;
                            break;
                        }
                    }
                }

                if (CurrentMap != null && CurrentMap.ContainsKey(e.Key()))
                {
                    var result = CurrentMap[e.Key()];
                    if (result.MyAction == null)
                    {
                        CurrentMap = result.MyMap;
                        PressedKeys.Add(e.Key());

                        Ambiguous = PressedKeys.Count == 1;
                    }
                    else
                    {
                        Skip = true;
                        
                        if (DoLog) Log(string.Format("Action in {0}: {1}", HoldContext.WindowName, result.MyAction));
                        result.MyAction.Execute();
                        
                        Skip = false;

                        Ambiguous = false;
                    }

                    Suppress(e);
                }
                else
                {
                    //SendAmbiguousKeys();
                    //Reset();
                }
            }
            catch (Exception exc)
            {
                Log(exc);
            }
        }

        public void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (Skip)
            {
                if (DoLog) Log("Skip Up " + e.Key().ToString());
                return;
            }

            try
            {
                if (Ambiguous && PressedKeys.Contains(e.Key()))
                {
                    Skip = true;
                    e.Key().DoPress();
                    Skip = false;

                    Ambiguous = false;
                }

                PressedKeys.RemoveAll(match => match == e.Key());
                
                if (PressedKeys.Count == 0)
                {
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
            if (PressedKeys.Count > 0)
            {
                Skip = true;
                foreach (var k in PressedKeys)
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
            PressedKeys.Clear();
            HoldContext = null;
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