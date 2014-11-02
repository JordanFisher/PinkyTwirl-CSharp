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
                for (int i = 0; i <= RepeatNumber; i++)
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
        
        public SemanticAction(Key k)
        {
            Representation = k.ToString();

            Action<Context, ExecuteType> action = (context, type) =>
            {
                if (type == ExecuteType.Press || type == ExecuteType.Down)
                {
                    k.DoDown();
                }

                if (type == ExecuteType.Press || type == ExecuteType.Up)
                {
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
        public static SemanticAction Modifiers = -65536;
        //
        // Summary:
        //     No public static Key pressed.
        public static SemanticAction None = 0;
        //
        // Summary:
        //     The left mouse button.
        public static SemanticAction LButton = 1;
        //
        // Summary:
        //     The right mouse button.
        public static SemanticAction RButton = 2;
        //
        // Summary:
        //     The CANCEL key.
        public static SemanticAction Cancel = 3;
        //
        // Summary:
        //     The middle mouse button (three-button mouse).
        public static SemanticAction MButton = 4;
        //
        // Summary:
        //     The first x mouse button (five-button mouse).
        public static SemanticAction XButton1 = 5;
        //
        // Summary:
        //     The second x mouse button (five-button mouse).
        public static SemanticAction XButton2 = 6;
        //
        // Summary:
        //     The BACKSPACE key.
        public static SemanticAction Backspace = 8;
        //
        // Summary:
        //     The TAB key.
        public static SemanticAction Tab = 9;
        //
        // Summary:
        //     The LINEFEED key.
        public static SemanticAction LineFeed = 10;
        //
        // Summary:
        //     The CLEAR key.
        public static SemanticAction Clear = 12;
        //
        // Summary:
        //     The ENTER key.
        public static SemanticAction Enter = 13;
        //
        // Summary:
        //     The RETURN key.
        public static SemanticAction Return = 13;
        //
        // Summary:
        //     The SHIFT key.
        public static SemanticAction Shift = 16;
        //
        // Summary:
        //     The CTRL key.
        public static SemanticAction Ctrl = 17;
        //
        // Summary:
        //     The ALT key.
        public static SemanticAction Alt = 18;
        //
        // Summary:
        //     The PAUSE key.
        public static SemanticAction Pause = 19;
        //
        // Summary:
        //     The CAPS LOCK key.
        public static SemanticAction CapsLock = 20;
        //
        // Summary:
        //     The CAPS LOCK key.
        public static SemanticAction Capital = 20;
        //
        // Summary:
        //     The IME Kana mode key.
        public static SemanticAction KanaMode = 21;
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        public static SemanticAction HanguelMode = 21;
        //
        // Summary:
        //     The IME Hangul mode key.
        public static SemanticAction HangulMode = 21;
        //
        // Summary:
        //     The IME Junja mode key.
        public static SemanticAction JunjaMode = 23;
        //
        // Summary:
        //     The IME final mode key.
        public static SemanticAction FinalMode = 24;
        //
        // Summary:
        //     The IME Kanji mode key.
        public static SemanticAction KanjiMode = 25;
        //
        // Summary:
        //     The IME Hanja mode key.
        public static SemanticAction HanjaMode = 25;
        //
        // Summary:
        //     The ESC key.
        public static SemanticAction Escape = 27;
        //
        // Summary:
        //     The IME convert key.
        public static SemanticAction IMEConvert = 28;
        //
        // Summary:
        //     The IME nonconvert key.
        public static SemanticAction IMENonconvert = 29;
        //
        // Summary:
        //     The IME accept key. Obsolete; use System.Windows.Forms.Keys.IMEAccept instead.
        public static SemanticAction IMEAceept = 30;
        //
        // Summary:
        //     The IME accept key; replaces System.Windows.Forms.Keys.IMEAceept.
        public static SemanticAction IMEAccept = 30;
        //
        // Summary:
        //     The IME mode change key.
        public static SemanticAction IMEModeChange = 31;
        //
        // Summary:
        //     The SPACEBAR key.
        public static SemanticAction Space = 32;
        //
        // Summary:
        //     The PAGE UP key.
        public static SemanticAction Prior = 33;
        //
        // Summary:
        //     The PAGE UP key.
        public static SemanticAction PageUp = 33;
        //
        // Summary:
        //     The PAGE DOWN key.
        public static SemanticAction Next = 34;
        //
        // Summary:
        //     The PAGE DOWN key.
        public static SemanticAction PageDown = 34;
        //
        // Summary:
        //     The END key.
        public static SemanticAction End = 35;
        //
        // Summary:
        //     The HOME key.
        public static SemanticAction Home = 36;
        //
        // Summary:
        //     The LEFT ARROW key.
        public static SemanticAction Left = 37;
        //
        // Summary:
        //     The UP ARROW key.
        public static SemanticAction Up = 38;
        //
        // Summary:
        //     The RIGHT ARROW key.
        public static SemanticAction Right = 39;
        //
        // Summary:
        //     The DOWN ARROW key.
        public static SemanticAction Down = 40;
        //
        // Summary:
        //     The SELECT key.
        public static SemanticAction Select = 41;
        //
        // Summary:
        //     The PRINT key.
        public static SemanticAction Print = 42;
        //
        // Summary:
        //     The EXECUTE key.
        public static SemanticAction ExecuteKey = 43;
        //
        // Summary:
        //     The PRINT SCREEN key.
        public static SemanticAction PrintScreen = 44;
        //
        // Summary:
        //     The PRINT SCREEN key.
        public static SemanticAction Snapshot = 44;
        //
        // Summary:
        //     The INS key.
        public static SemanticAction Insert = 45;
        //
        // Summary:
        //     The DEL key.
        public static SemanticAction Delete = 46;
        //
        // Summary:
        //     The HELP key.
        public static SemanticAction Help = 47;
        //
        // Summary:
        //     The 0 key.
        public static SemanticAction D0 = 48;
        //
        // Summary:
        //     The 1 key.
        public static SemanticAction D1 = 49;
        //
        // Summary:
        //     The 2 key.
        public static SemanticAction D2 = 50;
        //
        // Summary:
        //     The 3 key.
        public static SemanticAction D3 = 51;
        //
        // Summary:
        //     The 4 key.
        public static SemanticAction D4 = 52;
        //
        // Summary:
        //     The 5 key.
        public static SemanticAction D5 = 53;
        //
        // Summary:
        //     The 6 key.
        public static SemanticAction D6 = 54;
        //
        // Summary:
        //     The 7 key.
        public static SemanticAction D7 = 55;
        //
        // Summary:
        //     The 8 key.
        public static SemanticAction D8 = 56;
        //
        // Summary:
        //     The 9 key.
        public static SemanticAction D9 = 57;
        //
        // Summary:
        //     The A key.
        public static SemanticAction A = 65;
        //
        // Summary:
        //     The B key.
        public static SemanticAction B = 66;
        //
        // Summary:
        //     The C key.
        public static SemanticAction C = 67;
        //
        // Summary:
        //     The D key.
        public static SemanticAction D = 68;
        //
        // Summary:
        //     The E key.
        public static SemanticAction E = 69;
        //
        // Summary:
        //     The F key.
        public static SemanticAction F = 70;
        //
        // Summary:
        //     The G key.
        public static SemanticAction G = 71;
        //
        // Summary:
        //     The H key.
        public static SemanticAction H = 72;
        //
        // Summary:
        //     The I key.
        public static SemanticAction I = 73;
        //
        // Summary:
        //     The J key.
        public static SemanticAction J = 74;
        //
        // Summary:
        //     The K key.
        public static SemanticAction K = 75;
        //
        // Summary:
        //     The L key.
        public static SemanticAction L = 76;
        //
        // Summary:
        //     The M key.
        public static SemanticAction M = 77;
        //
        // Summary:
        //     The N key.
        public static SemanticAction N = 78;
        //
        // Summary:
        //     The O key.
        public static SemanticAction O = 79;
        //
        // Summary:
        //     The P key.
        public static SemanticAction P = 80;
        //
        // Summary:
        //     The Q key.
        public static SemanticAction Q = 81;
        //
        // Summary:
        //     The R key.
        public static SemanticAction R = 82;
        //
        // Summary:
        //     The S key.
        public static SemanticAction S = 83;
        //
        // Summary:
        //     The T key.
        public static SemanticAction T = 84;
        //
        // Summary:
        //     The U key.
        public static SemanticAction U = 85;
        //
        // Summary:
        //     The V key.
        public static SemanticAction V = 86;
        //
        // Summary:
        //     The W key.
        public static SemanticAction W = 87;
        //
        // Summary:
        //     The X key.
        public static SemanticAction X = 88;
        //
        // Summary:
        //     The Y key.
        public static SemanticAction Y = 89;
        //
        // Summary:
        //     The Z key.
        public static SemanticAction Z = 90;
        //
        // Summary:
        //     The left Windows logo public static Key (Microsoft Natural Keyboard).
        public static SemanticAction LWin = 91;
        //
        // Summary:
        //     The right Windows logo public static Key (Microsoft Natural Keyboard).
        public static SemanticAction RWin = 92;
        //
        // Summary:
        //     The application public static Key (Microsoft Natural Keyboard).
        public static SemanticAction Apps = 93;
        //
        // Summary:
        //     The computer sleep key.
        public static SemanticAction Sleep = 95;
        //
        // Summary:
        //     The 0 public static Key on the numeric keypad.
        public static SemanticAction NumPad0 = 96;
        //
        // Summary:
        //     The 1 public static Key on the numeric keypad.
        public static SemanticAction NumPad1 = 97;
        //
        // Summary:
        //     The 2 public static Key on the numeric keypad.
        public static SemanticAction NumPad2 = 98;
        //
        // Summary:
        //     The 3 public static Key on the numeric keypad.
        public static SemanticAction NumPad3 = 99;
        //
        // Summary:
        //     The 4 public static Key on the numeric keypad.
        public static SemanticAction NumPad4 = 100;
        //
        // Summary:
        //     The 5 public static Key on the numeric keypad.
        public static SemanticAction NumPad5 = 101;
        //
        // Summary:
        //     The 6 public static Key on the numeric keypad.
        public static SemanticAction NumPad6 = 102;
        //
        // Summary:
        //     The 7 public static Key on the numeric keypad.
        public static SemanticAction NumPad7 = 103;
        //
        // Summary:
        //     The 8 public static Key on the numeric keypad.
        public static SemanticAction NumPad8 = 104;
        //
        // Summary:
        //     The 9 public static Key on the numeric keypad.
        public static SemanticAction NumPad9 = 105;
        //
        // Summary:
        //     The multiply key.
        public static SemanticAction Multiply = 106;
        //
        // Summary:
        //     The add key.
        public static SemanticAction Add = 107;
        //
        // Summary:
        //     The separator key.
        public static SemanticAction Separator = 108;
        //
        // Summary:
        //     The subtract key.
        public static SemanticAction Subtract = 109;
        //
        // Summary:
        //     The decimal key.
        public static SemanticAction Decimal = 110;
        //
        // Summary:
        //     The divide key.
        public static SemanticAction Divide = 111;
        //
        // Summary:
        //     The F1 key.
        public static SemanticAction F1 = 112;
        //
        // Summary:
        //     The F2 key.
        public static SemanticAction F2 = 113;
        //
        // Summary:
        //     The F3 key.
        public static SemanticAction F3 = 114;
        //
        // Summary:
        //     The F4 key.
        public static SemanticAction F4 = 115;
        //
        // Summary:
        //     The F5 key.
        public static SemanticAction F5 = 116;
        //
        // Summary:
        //     The F6 key.
        public static SemanticAction F6 = 117;
        //
        // Summary:
        //     The F7 key.
        public static SemanticAction F7 = 118;
        //
        // Summary:
        //     The F8 key.
        public static SemanticAction F8 = 119;
        //
        // Summary:
        //     The F9 key.
        public static SemanticAction F9 = 120;
        //
        // Summary:
        //     The F10 key.
        public static SemanticAction F10 = 121;
        //
        // Summary:
        //     The F11 key.
        public static SemanticAction F11 = 122;
        //
        // Summary:
        //     The F12 key.
        public static SemanticAction F12 = 123;
        //
        // Summary:
        //     The F13 key.
        public static SemanticAction F13 = 124;
        //
        // Summary:
        //     The F14 key.
        public static SemanticAction F14 = 125;
        //
        // Summary:
        //     The F15 key.
        public static SemanticAction F15 = 126;
        //
        // Summary:
        //     The F16 key.
        public static SemanticAction F16 = 127;
        //
        // Summary:
        //     The F17 key.
        public static SemanticAction F17 = 128;
        //
        // Summary:
        //     The F18 key.
        public static SemanticAction F18 = 129;
        //
        // Summary:
        //     The F19 key.
        public static SemanticAction F19 = 130;
        //
        // Summary:
        //     The F20 key.
        public static SemanticAction F20 = 131;
        //
        // Summary:
        //     The F21 key.
        public static SemanticAction F21 = 132;
        //
        // Summary:
        //     The F22 key.
        public static SemanticAction F22 = 133;
        //
        // Summary:
        //     The F23 key.
        public static SemanticAction F23 = 134;
        //
        // Summary:
        //     The F24 key.
        public static SemanticAction F24 = 135;
        //
        // Summary:
        //     The NUM LOCK key.
        public static SemanticAction NumLock = 144;
        //
        // Summary:
        //     The SCROLL LOCK key.
        public static SemanticAction Scroll = 145;
        //
        // Summary:
        //     The left SHIFT key.
        public static SemanticAction LShift = 160;
        //
        // Summary:
        //     The right SHIFT key.
        public static SemanticAction RShift = 161;
        //
        // Summary:
        //     The left CTRL key.
        public static SemanticAction LControl = 162;
        //
        // Summary:
        //     The right CTRL key.
        public static SemanticAction RControl = 163;
        //
        // Summary:
        //     The left ALT key.
        public static SemanticAction LMenu = 164;
        //
        // Summary:
        //     The right ALT key.
        public static SemanticAction RMenu = 165;
        //
        // Summary:
        //     The browser back public static Key (Windows 2000 or later).
        public static SemanticAction BrowserBack = 166;
        //
        // Summary:
        //     The browser forward public static Key (Windows 2000 or later).
        public static SemanticAction BrowserForward = 167;
        //
        // Summary:
        //     The browser refresh public static Key (Windows 2000 or later).
        public static SemanticAction BrowserRefresh = 168;
        //
        // Summary:
        //     The browser stop public static Key (Windows 2000 or later).
        public static SemanticAction BrowserStop = 169;
        //
        // Summary:
        //     The browser search public static Key (Windows 2000 or later).
        public static SemanticAction BrowserSearch = 170;
        //
        // Summary:
        //     The browser favorites public static Key (Windows 2000 or later).
        public static SemanticAction BrowserFavorites = 171;
        //
        // Summary:
        //     The browser home public static Key (Windows 2000 or later).
        public static SemanticAction BrowserHome = 172;
        //
        // Summary:
        //     The volume mute public static Key (Windows 2000 or later).
        public static SemanticAction VolumeMute = 173;
        //
        // Summary:
        //     The volume down public static Key (Windows 2000 or later).
        public static SemanticAction VolumeDown = 174;
        //
        // Summary:
        //     The volume up public static Key (Windows 2000 or later).
        public static SemanticAction VolumeUp = 175;
        //
        // Summary:
        //     The media next track public static Key (Windows 2000 or later).
        public static SemanticAction MediaNextTrack = 176;
        //
        // Summary:
        //     The media previous track public static Key (Windows 2000 or later).
        public static SemanticAction MediaPreviousTrack = 177;
        //
        // Summary:
        //     The media Stop public static Key (Windows 2000 or later).
        public static SemanticAction MediaStop = 178;
        //
        // Summary:
        //     The media play pause public static Key (Windows 2000 or later).
        public static SemanticAction MediaPlayPause = 179;
        //
        // Summary:
        //     The launch mail public static Key (Windows 2000 or later).
        public static SemanticAction LaunchMail = 180;
        //
        // Summary:
        //     The select media public static Key (Windows 2000 or later).
        public static SemanticAction SelectMedia = 181;
        //
        // Summary:
        //     The start application one public static Key (Windows 2000 or later).
        public static SemanticAction LaunchApplication1 = 182;
        //
        // Summary:
        //     The start application two public static Key (Windows 2000 or later).
        public static SemanticAction LaunchApplication2 = 183;
        //
        // Summary:
        //     The Semicolon public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Semicolon = 186;
        //
        // Summary:
        //     The plus public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Plus = 187;
        //
        // Summary:
        //     The comma public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Comma = 188;
        //
        // Summary:
        //     The minus public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Minus = 189;
        //
        // Summary:
        //     The period public static Key on any country/region keyboard (Windows 2000 or later).
        public static SemanticAction Period = 190;
        //
        // Summary:
        //     The question mark public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Question = 191;
        //
        // Summary:
        //     The tilde public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction tilde = 192;
        //
        // Summary:
        // Summary:
        //     The singled/double quote public static Key on a US standard keyboard (Windows 2000
        //     or later).
        public static SemanticAction Quotes = 222;
        //
        // Summary:
        //     The angle bracket or backslash public static Key on the RT 102 public static Key keyboard (Windows
        //     2000 or later).
        public static SemanticAction Backslash = 226;
        //
        // Summary:
        //     The PROCESS public static Key key.
        public static SemanticAction Processpublic = 229;
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key
        //     value is the low word of a 32-bit virtual-public static Key value used for non-keyboard
        //     input methods.
        public static SemanticAction Packet = 231;
        //
        // Summary:
        //     The ATTN key.
        public static SemanticAction Attn = 246;
        //
        // Summary:
        //     The CRSEL key.
        public static SemanticAction Crsel = 247;
        //
        // Summary:
        //     The EXSEL key.
        public static SemanticAction Exsel = 248;
        //
        // Summary:
        //     The ERASE EOF key.
        public static SemanticAction EraseEof = 249;
        //
        // Summary:
        //     The PLAY key.
        public static SemanticAction Play = 250;
        //
        // Summary:
        //     The ZOOM key.
        public static SemanticAction Zoom = 251;
        //
        // Summary:
        //     A constant reserved for future use.
        public static SemanticAction NoName = 252;
        //
        // Summary:
        //     The PA1 key.
        public static SemanticAction Pa1 = 253;
        //
        // Summary:
        //     The CLEAR key.
        public static SemanticAction ClearKey = 254;
        //
        // Summary:
        //     The bitmask to extract a public static Key code from a public static Key value.
        public static SemanticAction KeyCode = 65535;
        //
        // Summary:
        //     The SHIFT modifier key.
        public static SemanticAction ShiftMod = 65536;
        //
        // Summary:
        //     The CTRL modifier key.
        public static SemanticAction CtrlMod = 131072;
        //
        // Summary:
        //     The ALT modifier key.
        public static SemanticAction AltMod = 262144;
        //
        // Summary:
        //     The open bracket public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction OpenBrackets = 219;
        //
        // Summary:
        //     The pipe public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction Pipe = 220;
        //
        // Summary:
        //     The close bracket public static Key on a US standard keyboard (Windows 2000 or later).
        public static SemanticAction CloseBrackets = 221;
    }
}
