using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WindowsInput;

namespace PinkyTwirl
{
    public enum ExecuteType { Down, Up, Press };
    public class SemanticAction
    {
        public Dictionary<Context, Action<Context, ExecuteType>> ContextualEvents = new Dictionary<Context, Action<Context, ExecuteType>>();

        public static SemanticAction operator %(SemanticAction A, SemanticAction B)
        {
            if (!A.ContextualEvents.ContainsKey(Base.CurrentContext))
            {
                A.ContextualEvents.Add(Base.CurrentContext, null);
            }

            A.ContextualEvents[Base.CurrentContext] = (context, type) => B.Execute(type);
            A.Representation += "[" + B.ToString() + "]";

            return A;
        }

        public static SemanticAction operator +(SemanticAction A, SemanticAction B)
        {
            var Sum = new SemanticAction();
            Sum.Representation = A.ToString() + " + " + B.ToString();

            Action<Context, ExecuteType> action = (context, type) =>
            {
                if (type == ExecuteType.Press)
                {
                    A.Execute(ExecuteType.Down);
                    B.Execute(ExecuteType.Down);
                    A.Execute(ExecuteType.Up);
                    B.Execute(ExecuteType.Up);
                }

                if (type == ExecuteType.Down)
                {
                    A.Execute(ExecuteType.Down);   
                    B.Execute(ExecuteType.Down);
                }

                if (type == ExecuteType.Up)
                {
                    A.Execute(ExecuteType.Up);
                    B.Execute(ExecuteType.Up);
                }
            };

            Sum.ContextualEvents.Add(Contexts.Default, action);

            return Sum;
        }

        public static SemanticAction operator |(SemanticAction A, SemanticAction B)
        {
            var Sum = new SemanticAction();
            Sum.Representation = A.ToString() + " | " + B.ToString();

            Action<Context, ExecuteType> action = (context, type) =>
            {
                if (type == ExecuteType.Press || type == ExecuteType.Down)
                {
                    A.Execute(ExecuteType.Press);
                    B.Execute(ExecuteType.Press);
                }
            };

            Sum.ContextualEvents.Add(Contexts.Default, action);

            return Sum;
        }

        public static SemanticAction operator +(SemanticAction A, string s)
        {
            return A + new SemanticAction(s);
        }

        public static SemanticAction operator +(string s, SemanticAction B)
        {
            return new SemanticAction(s) + B;
        }

        public static SemanticAction operator*(int RepeatNumber, SemanticAction k)
        {
            return new SemanticAction(() =>
            {
                for (int i = 0; i < RepeatNumber; i++)
                {
                    k.Execute();
                }
            });
        }

        public static SemanticAction operator|(SemanticAction A, string s)
        {
            return A | new SemanticAction(s);
        }

        public static SemanticAction operator |(string s, SemanticAction B)
        {
            return new SemanticAction(s) | B;
        }

        public static implicit operator SemanticAction(Action action)
        {
            return new SemanticAction(action);
        }

        public static implicit operator SemanticAction(int val)
        {
            return new SemanticAction(val);
        }

        public string Representation = string.Empty;
        public override string ToString()
        {
            return Representation;
        }

        public SemanticAction()
        {

        }

        public SemanticAction(int Value) : this((Key)Value)
        {

        }

        public SemanticAction(char c) : this((Key)c)
        {

        }

        public SemanticAction(Key k) : this(k, k.ToString())
        {

        }

        public SemanticAction(Key k, string Representation)
        {
            this.Representation = Representation;

            Action<Context, ExecuteType> action = (context, type) =>
            {
                if (type == ExecuteType.Press || type == ExecuteType.Down)
                {
                    if (Base.App.DoLog)
                    {
                        Base.App.Log(string.Format("  Press {0}", this.Representation));
                    }

                    k.DoDown();
                }

                if (type == ExecuteType.Press || type == ExecuteType.Up)
                {
                    if (Base.App.DoLog)
                    {
                        Base.App.Log(string.Format("  Release {0}", this.Representation));
                    }

                    k.DoUp();
                }
            };

            ContextualEvents.Add(Contexts.Default, action);
        }
        
        public SemanticAction(Action ActionValue)
        {
            Representation = "Action";

            Action<Context, ExecuteType> action = (context, type) =>
            {
                if (type == ExecuteType.Press || type == ExecuteType.Down)
                {
                    ActionValue();
                }
            };

            ContextualEvents.Add(Contexts.Default, action);
        }

        public SemanticAction(string s)
        {
            Representation = s;

            Action<Context, ExecuteType> action = (context, type) =>
            {
                if (type == ExecuteType.Press || type == ExecuteType.Down)
                {
                    InputSimulator.Keys.TextEntry(s);
                }
            };

            ContextualEvents.Add(Contexts.Default, action);
        }

        public void DoDown()
        {
            Execute(ExecuteType.Down);
        }

        public void DoUp()
        {
            Execute(ExecuteType.Up);
        }

        public void Do()
        {
            Execute();
        }

        public static void Do(SemanticAction action)
        {
            action.Execute();
        }

        public static void DoDown(SemanticAction action)
        {
            action.Execute(ExecuteType.Down);
        }

        public static void DoUp(SemanticAction action)
        {
            action.Execute(ExecuteType.Up);
        }

        public void Execute(ExecuteType type = ExecuteType.Press)
        {
            Execute(Base.CurrentContext, type);
        }

        public void Execute(Context context, ExecuteType type = ExecuteType.Press)
        {
            if (context == null) return;

            if (ContextualEvents.ContainsKey(context))
            {
                ContextualEvents[context](context, type);
            }
            else
            {
                Execute(context.ParentContext, type);
            }
        }




        // Summary:
        //     The bitmask to extract modifiers from a key value.
        public static SemanticAction Modifiers = new SemanticAction(-65536, "Modifiers");
        //
        // Summary:
        //     No public static Key pressed.
        public static SemanticAction None = new SemanticAction(0, "None");
        //
        // Summary:
        //     The left mouse button.
        public static SemanticAction LButton = new SemanticAction(1, "LButton");
        //
        // Summary:
        //     The right mouse button.
        public static SemanticAction RButton = new SemanticAction(2, "RButton");
        //
        // Summary:
        //     The CANCEL key.
        public static SemanticAction Cancel = new SemanticAction(3, "Cancel");
        //
        // Summary:
        //     The middle mouse button (three-button mouse).
        public static SemanticAction MButton = new SemanticAction(4, "MButton");
        //
        // Summary:
        //     The first x mouse button (five-button mouse).
        public static SemanticAction XButton1 = new SemanticAction(5, "XButton1");
        //
        // Summary:
        //     The second x mouse button (five-button mouse).
        public static SemanticAction XButton2 = new SemanticAction(6, "XButton2");
        //
        // Summary:
        //     The BACKSPACE key.
        public static SemanticAction Backspace = new SemanticAction(8, "Backspace");
        //
        // Summary:
        //     The TAB key.
        public static SemanticAction Tab = new SemanticAction(9, "Tab");
        //
        // Summary:
        //     The LINEFEED key.
        public static SemanticAction LineFeed = new SemanticAction(10, "LineFeed");
        //
        // Summary:
        //     The CLEAR key.
        public static SemanticAction Clear = new SemanticAction(12, "Clear");
        //
        // Summary:
        //     The ENTER key.
        public static SemanticAction Enter = new SemanticAction(13, "Enter");
        //
        // Summary:
        //     The RETURN key.
        public static SemanticAction Return = new SemanticAction(13, "Return");
        //
        // Summary:
        //     The SHIFT key.
        public static SemanticAction Shift = new SemanticAction(16, "Shift");
        //
        // Summary:
        //     The CTRL key.
        public static SemanticAction Ctrl = new SemanticAction(17, "Ctrl");
        //
        // Summary:
        //     The ALT key.
        public static SemanticAction Alt = new SemanticAction(18, "Alt");
        //
        // Summary:
        //     The PAUSE key.
        public static SemanticAction Pause = new SemanticAction(19, "Pause");
        //
        // Summary:
        //     The CAPS LOCK key.
        public static SemanticAction CapsLock = new SemanticAction(20, "CapsLock");
        //
        // Summary:
        //     The CAPS LOCK key.
        public static SemanticAction Capital = new SemanticAction(20, "Capital");
        //
        // Summary:
        //     The IME Kana mode key.
        public static SemanticAction KanaMode = new SemanticAction(21, "KanaMode");
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        public static SemanticAction HanguelMode = new SemanticAction(21, "HanguelMode");
        //
        // Summary:
        //     The IME Hangul mode key.
        public static SemanticAction HangulMode = new SemanticAction(21, "HangulMode");
        //
        // Summary:
        //     The IME Junja mode key.
        public static SemanticAction JunjaMode = new SemanticAction(23, "JunjaMode");
        //
        // Summary:
        //     The IME final mode key.
        public static SemanticAction FinalMode = new SemanticAction(24, "FinalMode");
        //
        // Summary:
        //     The IME Kanji mode key.
        public static SemanticAction KanjiMode = new SemanticAction(25, "KanjiMode");
        //
        // Summary:
        //     The IME Hanja mode key.
        public static SemanticAction HanjaMode = new SemanticAction(25, "HanjaMode");
        //
        // Summary:
        //     The ESC key.
        public static SemanticAction Escape = new SemanticAction(27, "Escape");
        //
        // Summary:
        //     The IME convert key.
        public static SemanticAction IMEConvert = new SemanticAction(28, "IMEConvert");
        //
        // Summary:
        //     The IME nonconvert key.
        public static SemanticAction IMENonconvert = new SemanticAction(29, "IMENonconvert");
        //
        // Summary:
        //     The IME accept key. Obsolete; use System.Windows.Forms.Keys.IMEAccept instead.
        public static SemanticAction IMEAceept = new SemanticAction(30, "IMEAceept");
        //
        // Summary:
        //     The IME accept key; replaces System.Windows.Forms.Keys.IMEAceept.
        public static SemanticAction IMEAccept = new SemanticAction(30, "IMEAccept");
        //
        // Summary:
        //     The IME mode change key.
        public static SemanticAction IMEModeChange = new SemanticAction(31, "IMEModeChange");
        //
        // Summary:
        //     The SPACEBAR key.
        public static SemanticAction Space = new SemanticAction(32, "Space");
        //
        // Summary:
        //     The PAGE UP key.
        public static SemanticAction Prior = new SemanticAction(33, "Prior");
        //
        // Summary:
        //     The PAGE UP key.
        public static SemanticAction PageUp = new SemanticAction(33, "PageUp");
        //
        // Summary:
        //     The PAGE DOWN key.
        public static SemanticAction Next = new SemanticAction(34, "Next");
        //
        // Summary:
        //     The PAGE DOWN key.
        public static SemanticAction PageDown = new SemanticAction(34, "PageDown");
        //
        // Summary:
        //     The END key.
        public static SemanticAction End = new SemanticAction(35, "End");
        //
        // Summary:
        //     The HOME key.
        public static SemanticAction Home = new SemanticAction(36, "Home");
        //
        // Summary:
        //     The LEFT ARROW key.
        public static SemanticAction Left = new SemanticAction(37, "Left");
        //
        // Summary:
        //     The UP ARROW key.
        public static SemanticAction Up = new SemanticAction(38, "Up");
        //
        // Summary:
        //     The RIGHT ARROW key.
        public static SemanticAction Right = new SemanticAction(39, "Right");
        //
        // Summary:
        //     The DOWN ARROW key.
        public static SemanticAction Down = new SemanticAction(40, "Down");
        //
        // Summary:
        //     The SELECT key.
        public static SemanticAction Select = new SemanticAction(41, "Select");
        //
        // Summary:
        //     The PRINT key.
        public static SemanticAction Print = new SemanticAction(42, "Print");
        //
        // Summary:
        //     The EXECUTE key.
        public static SemanticAction ExecuteKey = new SemanticAction(43, "ExecuteKey");
        //
        // Summary:
        //     The PRINT SCREEN key.
        public static SemanticAction PrintScreen = new SemanticAction(44, "PrintScreen");
        //
        // Summary:
        //     The PRINT SCREEN key.
        public static SemanticAction Snapshot = new SemanticAction(44, "Snapshot");
        //
        // Summary:
        //     The INS key.
        public static SemanticAction Insert = new SemanticAction(45, "Insert");
        //
        // Summary:
        //     The DEL key.
        public static SemanticAction Delete = new SemanticAction(46, "Delete");
        //
        // Summary:
        //     The HELP key.
        public static SemanticAction Help = new SemanticAction(47, "Help");
        //
        // Summary:
        //     The 0 key.
        public static SemanticAction D0 = new SemanticAction(48, "D0");
        //
        // Summary:
        //     The 1 key.
        public static SemanticAction D1 = new SemanticAction(49, "D1");
        //
        // Summary:
        //     The 2 key.
        public static SemanticAction D2 = new SemanticAction(50, "D2");
        //
        // Summary:
        //     The 3 key.
        public static SemanticAction D3 = new SemanticAction(51, "D3");
        //
        // Summary:
        //     The 4 key.
        public static SemanticAction D4 = new SemanticAction(52, "D4");
        //
        // Summary:
        //     The 5 key.
        public static SemanticAction D5 = new SemanticAction(53, "D5");
        //
        // Summary:
        //     The 6 key.
        public static SemanticAction D6 = new SemanticAction(54, "D6");
        //
        // Summary:
        //     The 7 key.
        public static SemanticAction D7 = new SemanticAction(55, "D7");
        //
        // Summary:
        //     The 8 key.
        public static SemanticAction D8 = new SemanticAction(56, "D8");
        //
        // Summary:
        //     The 9 key.
        public static SemanticAction D9 = new SemanticAction(57, "D9");
        //
        // Summary:
        //     The A key.
        public static SemanticAction A = new SemanticAction(65, "A");
        //
        // Summary:
        //     The B key.
        public static SemanticAction B = new SemanticAction(66, "B");
        //
        // Summary:
        //     The C key.
        public static SemanticAction C = new SemanticAction(67, "C");
        //
        // Summary:
        //     The D key.
        public static SemanticAction D = new SemanticAction(68, "D");
        //
        // Summary:
        //     The E key.
        public static SemanticAction E = new SemanticAction(69, "E");
        //
        // Summary:
        //     The F key.
        public static SemanticAction F = new SemanticAction(70, "F");
        //
        // Summary:
        //     The G key.
        public static SemanticAction G = new SemanticAction(71, "G");
        //
        // Summary:
        //     The H key.
        public static SemanticAction H = new SemanticAction(72, "H");
        //
        // Summary:
        //     The I key.
        public static SemanticAction I = new SemanticAction(73, "I");
        //
        // Summary:
        //     The J key.
        public static SemanticAction J = new SemanticAction(74, "J");
        //
        // Summary:
        //     The K key.
        public static SemanticAction K = new SemanticAction(75, "K");
        //
        // Summary:
        //     The L key.
        public static SemanticAction L = new SemanticAction(76, "L");
        //
        // Summary:
        //     The M key.
        public static SemanticAction M = new SemanticAction(77, "M");
        //
        // Summary:
        //     The N key.
        public static SemanticAction N = new SemanticAction(78, "N");
        //
        // Summary:
        //     The O key.
        public static SemanticAction O = new SemanticAction(79, "O");
        //
        // Summary:
        //     The P key.
        public static SemanticAction P = new SemanticAction(80, "P");
        //
        // Summary:
        //     The Q key.
        public static SemanticAction Q = new SemanticAction(81, "Q");
        //
        // Summary:
        //     The R key.
        public static SemanticAction R = new SemanticAction(82, "R");
        //
        // Summary:
        //     The S key.
        public static SemanticAction S = new SemanticAction(83, "S");
        //
        // Summary:
        //     The T key.
        public static SemanticAction T = new SemanticAction(84, "T");
        //
        // Summary:
        //     The U key.
        public static SemanticAction U = new SemanticAction(85, "U");
        //
        // Summary:
        //     The V key.
        public static SemanticAction V = new SemanticAction(86, "V");
        //
        // Summary:
        //     The W key.
        public static SemanticAction W = new SemanticAction(87, "W");
        //
        // Summary:
        //     The X key.
        public static SemanticAction X = new SemanticAction(88, "X");
        //
        // Summary:
        //     The Y key.
        public static SemanticAction Y = new SemanticAction(89, "Y");
        //
        // Summary:
        //     The Z key.
        public static SemanticAction Z = new SemanticAction(90, "Z");
        //
        // Summary:
        //     The left Windows logo public static Key (Microsoft Natural Keyboard).
        public static SemanticAction LWin = new SemanticAction(91, "LWin");
        //
        // Summary:
        //     The right Windows logo public static Key (Microsoft Natural Keyboard).
        public static SemanticAction RWin = new SemanticAction(92, "RWin");
        //
        // Summary:
        //     The application public static Key (Microsoft Natural Keyboard).
        public static SemanticAction Apps = new SemanticAction(93, "Apps");
        //
        // Summary:
        //     The computer sleep key.
        public static SemanticAction Sleep = new SemanticAction(95, "Sleep");
        //
        // Summary:
        //     The 0 public static Key on the numeric keypad.
        public static SemanticAction NumPad0 = new SemanticAction(96, "NumPad0");
        //
        // Summary:
        //     The 1 public static Key on the numeric keypad.
        public static SemanticAction NumPad1 = new SemanticAction(97, "NumPad1");
        //
        // Summary:
        //     The 2 public static Key on the numeric keypad.
        public static SemanticAction NumPad2 = new SemanticAction(98, "NumPad2");
        //
        // Summary:
        //     The 3 public static Key on the numeric keypad.
        public static SemanticAction NumPad3 = new SemanticAction(99, "NumPad3");
        //
        // Summary:
        //     The 4 public static Key on the numeric keypad.
        public static SemanticAction NumPad4 = new SemanticAction(100, "NumPad4");
        //
        // Summary:
        //     The 5 public static Key on the numeric keypad.
        public static SemanticAction NumPad5 = new SemanticAction(101, "NumPad5");
        //
        // Summary:
        //     The 6 public static Key on the numeric keypad.
        public static SemanticAction NumPad6 = new SemanticAction(102, "NumPad6");
        //
        // Summary:
        //     The 7 public static Key on the numeric keypad.
        public static SemanticAction NumPad7 = new SemanticAction(103, "NumPad7");
        //
        // Summary:
        //     The 8 public static Key on the numeric keypad.
        public static SemanticAction NumPad8 = new SemanticAction(104, "NumPad8");
        //
        // Summary:
        //     The 9 public static Key on the numeric keypad.
        public static SemanticAction NumPad9 = new SemanticAction(105, "NumPad9");
        //
        // Summary:
        //     The multiply key.
        public static SemanticAction Multiply = new SemanticAction(106, "Multiply");
        //
        // Summary:
        //     The add key.
        public static SemanticAction Add = new SemanticAction(107, "Add");
        //
        // Summary:
        //     The separator key.
        public static SemanticAction Separator = new SemanticAction(108, "Separator");
        //
        // Summary:
        //     The subtract key.
        public static SemanticAction Subtract = new SemanticAction(109, "Subtract");
        //
        // Summary:
        //     The decimal key.
        public static SemanticAction Decimal = new SemanticAction(110, "Decimal");
        //
        // Summary:
        //     The divide key.
        public static SemanticAction Divide = new SemanticAction(111, "Divide");
        //
        // Summary:
        //     The F1 key.
        public static SemanticAction F1 = new SemanticAction(112, "F1");
        //
        // Summary:
        //     The F2 key.
        public static SemanticAction F2 = new SemanticAction(113, "F2");
        //
        // Summary:
        //     The F3 key.
        public static SemanticAction F3 = new SemanticAction(114, "F3");
        //
        // Summary:
        //     The F4 key.
        public static SemanticAction F4 = new SemanticAction(115, "F4");
        //
        // Summary:
        //     The F5 key.
        public static SemanticAction F5 = new SemanticAction(116, "F5");
        //
        // Summary:
        //     The F6 key.
        public static SemanticAction F6 = new SemanticAction(117, "F6");
        //
        // Summary:
        //     The F7 key.
        public static SemanticAction F7 = new SemanticAction(118, "F7");
        //
        // Summary:
        //     The F8 key.
        public static SemanticAction F8 = new SemanticAction(119, "F8");
        //
        // Summary:
        //     The F9 key.
        public static SemanticAction F9 = new SemanticAction(120, "F9");
        //
        // Summary:
        //     The F10 key.
        public static SemanticAction F10 = new SemanticAction(121, "F10");
        //
        // Summary:
        //     The F11 key.
        public static SemanticAction F11 = new SemanticAction(122, "F11");
        //
        // Summary:
        //     The F12 key.
        public static SemanticAction F12 = new SemanticAction(123, "F12");
        //
        // Summary:
        //     The F13 key.
        public static SemanticAction F13 = new SemanticAction(124, "F13");
        //
        // Summary:
        //     The F14 key.
        public static SemanticAction F14 = new SemanticAction(125, "F14");
        //
        // Summary:
        //     The F15 key.
        public static SemanticAction F15 = new SemanticAction(126, "F15");
        //
        // Summary:
        //     The F16 key.
        public static SemanticAction F16 = new SemanticAction(127, "F16");
        //
        // Summary:
        //     The F17 key.
        public static SemanticAction F17 = new SemanticAction(128, "F17");
        //
        // Summary:
        //     The F18 key.
        public static SemanticAction F18 = new SemanticAction(129, "F18");
        //
        // Summary:
        //     The F19 key.
        public static SemanticAction F19 = new SemanticAction(130, "F19");
        //
        // Summary:
        //     The F20 key.
        public static SemanticAction F20 = new SemanticAction(131, "F20");
        //
        // Summary:
        //     The F21 key.
        public static SemanticAction F21 = new SemanticAction(132, "F21");
        //
        // Summary:
        //     The F22 key.
        public static SemanticAction F22 = new SemanticAction(133, "F22");
        //
        // Summary:
        //     The F23 key.
        public static SemanticAction F23 = new SemanticAction(134, "F23");
        //
        // Summary:
        //     The F24 key.
        public static SemanticAction F24 = new SemanticAction(135, "F24");
        //
        // Summary:
        //     The NUM LOCK key.
        public static SemanticAction NumLock = new SemanticAction(144, "NumLock");
        //
        // Summary:
        //     The SCROLL LOCK key.
        public static SemanticAction Scroll = new SemanticAction(145, "Scroll");
        //
        // Summary:
        //     The left SHIFT key.
        public static SemanticAction LShift = new SemanticAction(160, "LShift");
        //
        // Summary:
        //     The right SHIFT key.
        public static SemanticAction RShift = new SemanticAction(161, "RShift");
        //
        // Summary:
        //     The left CTRL key.
        public static SemanticAction LControl = new SemanticAction(162, "LControl");
        //
        // Summary:
        //     The right CTRL key.
        public static SemanticAction RControl = new SemanticAction(163, "RControl");
        //
        // Summary:
        //     The left ALT key.
        public static SemanticAction LMenu = new SemanticAction(164, "LMenu");
        //
        // Summary:
        //     The right ALT key.
        public static SemanticAction RMenu = new SemanticAction(165, "RMenu");
        //
        // Summary:
        //     The browser back public static Key (Windows 2000 or later).
        public static SemanticAction BrowserBack = new SemanticAction(166, "BrowserBack");
        //
        // Summary:
        //     The browser forward public static Key (Windows 2000 or later).
        public static SemanticAction BrowserForward = new SemanticAction(167, "BrowserForward");
        //
        // Summary:
        //     The browser refresh public static Key (Windows 2000 or later).
        public static SemanticAction BrowserRefresh = new SemanticAction(168, "BrowserRefresh");
        //
        // Summary:
        //     The browser stop public static Key (Windows 2000 or later).
        public static SemanticAction BrowserStop = new SemanticAction(169, "BrowserStop");
        //
        // Summary:
        //     The browser search public static Key (Windows 2000 or later).
        public static SemanticAction BrowserSearch = new SemanticAction(170, "BrowserSearch");
        //
        // Summary:
        //     The browser favorites public static Key (Windows 2000 or later).
        public static SemanticAction BrowserFavorites = new SemanticAction(171, "BrowserFavorites");
        //
        // Summary:
        //     The browser home public static Key (Windows 2000 or later).
        public static SemanticAction BrowserHome = new SemanticAction(172, "BrowserHome");
        //
        // Summary:
        //     The volume mute public static Key (Windows 2000 or later).
        public static SemanticAction VolumeMute = new SemanticAction(173, "VolumeMute");
        //
        // Summary:
        //     The volume down public static Key (Windows 2000 or later).
        public static SemanticAction VolumeDown = new SemanticAction(174, "VolumeDown");
        //
        // Summary:
        //     The volume up public static Key (Windows 2000 or later).
        public static SemanticAction VolumeUp = new SemanticAction(175, "VolumeUp");
        //
        // Summary:
        //     The media next track public static Key (Windows 2000 or later).
        public static SemanticAction MediaNextTrack = new SemanticAction(176, "MediaNextTrack");
        //
        // Summary:
        //     The media previous track public static Key (Windows 2000 or later).
        public static SemanticAction MediaPreviousTrack = new SemanticAction(177, "MediaPreviousTrack");
        //
        // Summary:
        //     The media Stop public static Key (Windows 2000 or later).
        public static SemanticAction MediaStop = new SemanticAction(178, "MediaStop");
        //
        // Summary:
        //     The media play pause public static Key (Windows 2000 or later).
        public static SemanticAction MediaPlayPause = new SemanticAction(179, "MediaPlayPause");
        //
        // Summary:
        //     The launch mail public static Key (Windows 2000 or later).
        public static SemanticAction LaunchMail = new SemanticAction(180, "LaunchMail");
        //
        // Summary:
        //     The select media public static Key (Windows 2000 or later).
        public static SemanticAction SelectMedia = new SemanticAction(181, "SelectMedia");
        //
        // Summary:
        //     The start application one public static Key (Windows 2000 or later).
        public static SemanticAction LaunchApplication1 = new SemanticAction(182, "LaunchApplication1");
        //
        // Summary:
        //     The start application two public static Key (Windows 2000 or later).
        public static SemanticAction LaunchApplication2 = new SemanticAction(183, "LaunchApplication2");
        //
        // Summary:
        //     The Semicolon public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Semicolon = new SemanticAction(186, "Semicolon");
        //
        // Summary:
        //     The plus public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Plus = new SemanticAction(187, "Plus");
        //
        // Summary:
        //     The comma public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Comma = new SemanticAction(188, "Comma");
        //
        // Summary:
        //     The minus public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Minus = new SemanticAction(189, "Minus");
        //
        // Summary:
        //     The period public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Period = new SemanticAction(190, "Period");
        //
        // Summary:
        //     The question mark public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Question = new SemanticAction(191, "Question");
        //
        // Summary:
        //     The tilde public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Tilde = new SemanticAction(192, "tilde");
        //
        // Summary:
        // Summary:
        //     The singled/double quote public static Key on a US standard keyboard (Windows 2000
        //     or later).
        public static SemanticAction Quotes = new SemanticAction(222, "Quotes");
        //
        // Summary:
        //     The angle bracket or backslash public static Key on the RT 102 public static Key keyboard (Windows
        //     2000 or later).
        public static SemanticAction Backslash = new SemanticAction(226, "Backslash");
        //
        // Summary:
        //     The PROCESS public static Key key.
        public static SemanticAction Processpublic = new SemanticAction(229, "Processpublic");
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key
        //     value is the low word of a 32-bit virtual-public static Key value used for non-keyboard
        //     input methods.
        public static SemanticAction Packet = new SemanticAction(231, "Packet");
        //
        // Summary:
        //     The ATTN key.
        public static SemanticAction Attn = new SemanticAction(246, "Attn");
        //
        // Summary:
        //     The CRSEL key.
        public static SemanticAction Crsel = new SemanticAction(247, "Crsel");
        //
        // Summary:
        //     The EXSEL key.
        public static SemanticAction Exsel = new SemanticAction(248, "Exsel");
        //
        // Summary:
        //     The ERASE EOF key.
        public static SemanticAction EraseEof = new SemanticAction(249, "EraseEof");
        //
        // Summary:
        //     The PLAY key.
        public static SemanticAction Play = new SemanticAction(250, "Play");
        //
        // Summary:
        //     The ZOOM key.
        public static SemanticAction Zoom = new SemanticAction(251, "Zoom");
        //
        // Summary:
        //     A constant reserved for future use.
        public static SemanticAction NoName = new SemanticAction(252, "NoName");
        //
        // Summary:
        //     The PA1 key.
        public static SemanticAction Pa1 = new SemanticAction(253, "Pa1");
        //
        // Summary:
        //     The CLEAR key.
        public static SemanticAction ClearKey = new SemanticAction(254, "ClearKey");
        //
        // Summary:
        //     The bitmask to extract a public static Key code from a public static Key value.
        public static SemanticAction KeyCode = new SemanticAction(65535, "KeyCode");
        //
        // Summary:
        //     The SHIFT modifier key.
        public static SemanticAction ShiftMod = new SemanticAction(65536, "ShiftMod");
        //
        // Summary:
        //     The CTRL modifier key.
        public static SemanticAction CtrlMod = new SemanticAction(131072, "CtrlMod");
        //
        // Summary:
        //     The ALT modifier key.
        public static SemanticAction AltMod = new SemanticAction(262144, "AltMod");
        //
        // Summary:
        //     The open bracket public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction OpenBrackets = new SemanticAction(219, "OpenBrackets");
        //
        // Summary:
        //     The pipe public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Pipe = new SemanticAction(220, "Pipe");
        //
        // Summary:
        //     The close bracket public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction CloseBrackets = new SemanticAction(221, "CloseBrackets");
    }
}
