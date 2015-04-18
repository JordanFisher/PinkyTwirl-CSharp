using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WindowsInput;
using WindowsInput.Native;

using o = OtherWindowsInput;

namespace PinkyTwirl
{
    public static class KeyEventArgsExtension
    {
        public static Key Key(this KeyEventArgs e)
        {
            return (Key)(int)e.KeyCode;
        }
    }

    public struct Key
    {
        //public Key()
        //{
            
        //}

        public int Value;
        public Key(int Value)
        {
            this.Value = Value;
        }

        public static bool operator ==(Key k1, Key k2)
        {
            return k1.Value == k2.Value;
        }

        public static bool operator !=(Key k1, Key k2)
        {
            return k1.Value != k2.Value;
        }

        public static bool operator ==(Keys k1, Key k2)
        {
            return k1 == (Keys)(k2.Value);
        }

        public static bool operator !=(Keys k1, Key k2)
        {
            return k1 != (Keys)(k2.Value);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Key operator|(Key k, SemanticAction action)
        {
            Base.SetKeyMapping(k, action);
            return k;
        }

        public static Key operator |(Key k, Action action)
        {
            Base.SetKeyMapping(k, new SemanticAction(action));
            return k;
        }

        public static Key operator|(Key k, string s)
        {
            Base.SetKeyMapping(k, new SemanticAction(s));
            return k;
        }

        public static implicit operator Key(int val)
        {
            return new Key(val);
        }

        public override string ToString()
        {
            return string.Format("({0})", Value.ToString());
        }

        public static Key ApplyShift(Key k)
        {
            return k.Value | ShiftMod.Value;
        }

        public static implicit operator Key(char c)
        {
            if (c >= 'A' && c <= 'Z') return ApplyShift(A.Value + c - 'A');
            if (c >= 'a' && c <= 'z') return ApplyShift(A.Value + c - 'z');

            switch (c)
            {
                case ';': return Semicolon;
                case '+': return Plus;
                case ',': return Comma;
                case '-': return Minus;
                case '.': return Period;
                case '/': return Backslash;
                case '`': return ApplyShift(Tilde);
                case '\'': return Quotes;
                case '\\': return Backslash;
                case '[': return OpenBrackets;
                case ']': return CloseBrackets;

                case ':': return Semicolon;
                case '=': return ApplyShift(Plus);
                case '<': return Comma;
                case '_': return Minus;
                case '>': return Period;
                case '?': return Question;
                case '~': return Tilde;
                case '\"': return ApplyShift(Quotes);
                case '|': return Pipe;
                case '{': return ApplyShift(OpenBrackets);
                case '}': return ApplyShift(CloseBrackets);

                case '!': return ApplyShift(D1);
                case '@': return ApplyShift(D2);
                case '#': return ApplyShift(D3);
                case '$': return ApplyShift(D4);
                case '%': return ApplyShift(D5);
                case '^': return ApplyShift(D6);
                case '&': return ApplyShift(D7);
                case '*': return ApplyShift(D8);
                case '(': return ApplyShift(D9);
                case ')': return ApplyShift(D0);

                default: return Key.None;
            }
        }

        public VirtualKeyCode KeyCode
        {
            get
            {
                return (VirtualKeyCode)Value;
            }
        }

        public bool IsDown
        {
            get
            {
                //return false;
                return o.InputSimulator.IsKeyDown((o.VirtualKeyCode)KeyCode);
            }
        }

        public bool IsUp
        {
            get
            {
                return !IsDown;
            }
        }

        public static bool ShiftModifier = false;
        public void DoDown()
        {
            if (Value > 0)
            {
                //o.InputSimulator.SimulateKeyDown((o.VirtualKeyCode)KeyCode);
                InputSimulator.Keys.KeyDown(KeyCode);
            }
        }
            
        public void DoUp()
        {
            if (Value > 0)
            {
                o.InputSimulator.SimulateKeyUp((o.VirtualKeyCode)KeyCode); 
                InputSimulator.Keys.KeyUp(KeyCode);
            }
        }

        public void DoPress()
        {
            DoDown();
            DoUp();
        }

        // Summary:
        //     The bitmask to extract modifiers from a key value.
        public static Key Modifiers = -65536;
        //
        // Summary:
        //     No public static Key pressed.
        public static Key None = 0;
        //
        // Summary:
        //     The left mouse button.
        public static Key LButton = 1;
        //
        // Summary:
        //     The right mouse button.
        public static Key RButton = 2;
        //
        // Summary:
        //     The CANCEL key.
        public static Key Cancel = 3;
        //
        // Summary:
        //     The middle mouse button (three-button mouse).
        public static Key MButton = 4;
        //
        // Summary:
        //     The first x mouse button (five-button mouse).
        public static Key XButton1 = 5;
        //
        // Summary:
        //     The second x mouse button (five-button mouse).
        public static Key XButton2 = 6;
        //
        // Summary:
        //     The BACKSPACE key.
        public static Key Back = 8;
        //
        // Summary:
        //     The TAB key.
        public static Key Tab = 9;
        //
        // Summary:
        //     The LINEFEED key.
        public static Key LineFeed = 10;
        //
        // Summary:
        //     The CLEAR key.
        public static Key Clear = 12;
        //
        // Summary:
        //     The ENTER key.
        public static Key Enter = 13;
        //
        // Summary:
        //     The RETURN key.
        public static Key Return = 13;
        //
        // Summary:
        //     The SHIFT key.
        public static Key Shift = 16;
        //
        // Summary:
        //     The CTRL key.
        public static Key Ctrl = 17;
        //
        // Summary:
        //     The ALT key.
        public static Key Alt = 18;
        //
        // Summary:
        //     The PAUSE key.
        public static Key Pause = 19;
        //
        // Summary:
        //     The CAPS LOCK key.
        public static Key CapsLock = 20;
        //
        // Summary:
        //     The CAPS LOCK key.
        public static Key Capital = 20;
        //
        // Summary:
        //     The IME Kana mode key.
        public static Key KanaMode = 21;
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        public static Key HanguelMode = 21;
        //
        // Summary:
        //     The IME Hangul mode key.
        public static Key HangulMode = 21;
        //
        // Summary:
        //     The IME Junja mode key.
        public static Key JunjaMode = 23;
        //
        // Summary:
        //     The IME final mode key.
        public static Key FinalMode = 24;
        //
        // Summary:
        //     The IME Kanji mode key.
        public static Key KanjiMode = 25;
        //
        // Summary:
        //     The IME Hanja mode key.
        public static Key HanjaMode = 25;
        //
        // Summary:
        //     The ESC key.
        public static Key Escape = 27;
        //
        // Summary:
        //     The IME convert key.
        public static Key IMEConvert = 28;
        //
        // Summary:
        //     The IME nonconvert key.
        public static Key IMENonconvert = 29;
        //
        // Summary:
        //     The IME accept key. Obsolete; use System.Windows.Forms.Keys.IMEAccept instead.
        public static Key IMEAceept = 30;
        //
        // Summary:
        //     The IME accept key; replaces System.Windows.Forms.Keys.IMEAceept.
        public static Key IMEAccept = 30;
        //
        // Summary:
        //     The IME mode change key.
        public static Key IMEModeChange = 31;
        //
        // Summary:
        //     The SPACEBAR key.
        public static Key Space = 32;
        //
        // Summary:
        //     The PAGE UP key.
        public static Key Prior = 33;
        //
        // Summary:
        //     The PAGE UP key.
        public static Key PageUp = 33;
        //
        // Summary:
        //     The PAGE DOWN key.
        public static Key Next = 34;
        //
        // Summary:
        //     The PAGE DOWN key.
        public static Key PageDown = 34;
        //
        // Summary:
        //     The END key.
        public static Key End = 35;
        //
        // Summary:
        //     The HOME key.
        public static Key Home = 36;
        //
        // Summary:
        //     The LEFT ARROW key.
        public static Key Left = 37;
        //
        // Summary:
        //     The UP ARROW key.
        public static Key Up = 38;
        //
        // Summary:
        //     The RIGHT ARROW key.
        public static Key Right = 39;
        //
        // Summary:
        //     The DOWN ARROW key.
        public static Key Down = 40;
        //
        // Summary:
        //     The SELECT key.
        public static Key Select = 41;
        //
        // Summary:
        //     The PRINT key.
        public static Key Print = 42;
        //
        // Summary:
        //     The EXECUTE key.
        public static Key Execute = 43;
        //
        // Summary:
        //     The PRINT SCREEN key.
        public static Key PrintScreen = 44;
        //
        // Summary:
        //     The PRINT SCREEN key.
        public static Key Snapshot = 44;
        //
        // Summary:
        //     The INS key.
        public static Key Insert = 45;
        //
        // Summary:
        //     The DEL key.
        public static Key Delete = 46;
        //
        // Summary:
        //     The HELP key.
        public static Key Help = 47;
        //
        // Summary:
        //     The 0 key.
        public static Key D0 = 48;
        //
        // Summary:
        //     The 1 key.
        public static Key D1 = 49;
        //
        // Summary:
        //     The 2 key.
        public static Key D2 = 50;
        //
        // Summary:
        //     The 3 key.
        public static Key D3 = 51;
        //
        // Summary:
        //     The 4 key.
        public static Key D4 = 52;
        //
        // Summary:
        //     The 5 key.
        public static Key D5 = 53;
        //
        // Summary:
        //     The 6 key.
        public static Key D6 = 54;
        //
        // Summary:
        //     The 7 key.
        public static Key D7 = 55;
        //
        // Summary:
        //     The 8 key.
        public static Key D8 = 56;
        //
        // Summary:
        //     The 9 key.
        public static Key D9 = 57;
        //
        // Summary:
        //     The A key.
        public static Key A = 65;
        //
        // Summary:
        //     The B key.
        public static Key B = 66;
        //
        // Summary:
        //     The C key.
        public static Key C = 67;
        //
        // Summary:
        //     The D key.
        public static Key D = 68;
        //
        // Summary:
        //     The E key.
        public static Key E = 69;
        //
        // Summary:
        //     The F key.
        public static Key F = 70;
        //
        // Summary:
        //     The G key.
        public static Key G = 71;
        //
        // Summary:
        //     The H key.
        public static Key H = 72;
        //
        // Summary:
        //     The I key.
        public static Key I = 73;
        //
        // Summary:
        //     The J key.
        public static Key J = 74;
        //
        // Summary:
        //     The K key.
        public static Key K = 75;
        //
        // Summary:
        //     The L key.
        public static Key L = 76;
        //
        // Summary:
        //     The M key.
        public static Key M = 77;
        //
        // Summary:
        //     The N key.
        public static Key N = 78;
        //
        // Summary:
        //     The O key.
        public static Key O = 79;
        //
        // Summary:
        //     The P key.
        public static Key P = 80;
        //
        // Summary:
        //     The Q key.
        public static Key Q = 81;
        //
        // Summary:
        //     The R key.
        public static Key R = 82;
        //
        // Summary:
        //     The S key.
        public static Key S = 83;
        //
        // Summary:
        //     The T key.
        public static Key T = 84;
        //
        // Summary:
        //     The U key.
        public static Key U = 85;
        //
        // Summary:
        //     The V key.
        public static Key V = 86;
        //
        // Summary:
        //     The W key.
        public static Key W = 87;
        //
        // Summary:
        //     The X key.
        public static Key X = 88;
        //
        // Summary:
        //     The Y key.
        public static Key Y = 89;
        //
        // Summary:
        //     The Z key.
        public static Key Z = 90;
        //
        // Summary:
        //     The left Windows logo public static Key (Microsoft Natural Keyboard).
        public static Key LWin = 91;
        //
        // Summary:
        //     The right Windows logo public static Key (Microsoft Natural Keyboard).
        public static Key RWin = 92;
        //
        // Summary:
        //     The application public static Key (Microsoft Natural Keyboard).
        public static Key Apps = 93;
        //
        // Summary:
        //     The computer sleep key.
        public static Key Sleep = 95;
        //
        // Summary:
        //     The 0 public static Key on the numeric keypad.
        public static Key NumPad0 = 96;
        //
        // Summary:
        //     The 1 public static Key on the numeric keypad.
        public static Key NumPad1 = 97;
        //
        // Summary:
        //     The 2 public static Key on the numeric keypad.
        public static Key NumPad2 = 98;
        //
        // Summary:
        //     The 3 public static Key on the numeric keypad.
        public static Key NumPad3 = 99;
        //
        // Summary:
        //     The 4 public static Key on the numeric keypad.
        public static Key NumPad4 = 100;
        //
        // Summary:
        //     The 5 public static Key on the numeric keypad.
        public static Key NumPad5 = 101;
        //
        // Summary:
        //     The 6 public static Key on the numeric keypad.
        public static Key NumPad6 = 102;
        //
        // Summary:
        //     The 7 public static Key on the numeric keypad.
        public static Key NumPad7 = 103;
        //
        // Summary:
        //     The 8 public static Key on the numeric keypad.
        public static Key NumPad8 = 104;
        //
        // Summary:
        //     The 9 public static Key on the numeric keypad.
        public static Key NumPad9 = 105;
        //
        // Summary:
        //     The multiply key.
        public static Key Multiply = 106;
        //
        // Summary:
        //     The add key.
        public static Key Add = 107;
        //
        // Summary:
        //     The separator key.
        public static Key Separator = 108;
        //
        // Summary:
        //     The subtract key.
        public static Key Subtract = 109;
        //
        // Summary:
        //     The decimal key.
        public static Key Decimal = 110;
        //
        // Summary:
        //     The divide key.
        public static Key Divide = 111;
        //
        // Summary:
        //     The F1 key.
        public static Key F1 = 112;
        //
        // Summary:
        //     The F2 key.
        public static Key F2 = 113;
        //
        // Summary:
        //     The F3 key.
        public static Key F3 = 114;
        //
        // Summary:
        //     The F4 key.
        public static Key F4 = 115;
        //
        // Summary:
        //     The F5 key.
        public static Key F5 = 116;
        //
        // Summary:
        //     The F6 key.
        public static Key F6 = 117;
        //
        // Summary:
        //     The F7 key.
        public static Key F7 = 118;
        //
        // Summary:
        //     The F8 key.
        public static Key F8 = 119;
        //
        // Summary:
        //     The F9 key.
        public static Key F9 = 120;
        //
        // Summary:
        //     The F10 key.
        public static Key F10 = 121;
        //
        // Summary:
        //     The F11 key.
        public static Key F11 = 122;
        //
        // Summary:
        //     The F12 key.
        public static Key F12 = 123;
        //
        // Summary:
        //     The F13 key.
        public static Key F13 = 124;
        //
        // Summary:
        //     The F14 key.
        public static Key F14 = 125;
        //
        // Summary:
        //     The F15 key.
        public static Key F15 = 126;
        //
        // Summary:
        //     The F16 key.
        public static Key F16 = 127;
        //
        // Summary:
        //     The F17 key.
        public static Key F17 = 128;
        //
        // Summary:
        //     The F18 key.
        public static Key F18 = 129;
        //
        // Summary:
        //     The F19 key.
        public static Key F19 = 130;
        //
        // Summary:
        //     The F20 key.
        public static Key F20 = 131;
        //
        // Summary:
        //     The F21 key.
        public static Key F21 = 132;
        //
        // Summary:
        //     The F22 key.
        public static Key F22 = 133;
        //
        // Summary:
        //     The F23 key.
        public static Key F23 = 134;
        //
        // Summary:
        //     The F24 key.
        public static Key F24 = 135;
        //
        // Summary:
        //     The NUM LOCK key.
        public static Key NumLock = 144;
        //
        // Summary:
        //     The SCROLL LOCK key.
        public static Key Scroll = 145;
        //
        // Summary:
        //     The left SHIFT key.
        public static Key LShift = 160;
        //
        // Summary:
        //     The right SHIFT key.
        public static Key RShift = 161;
        //
        // Summary:
        //     The left CTRL key.
        public static Key LControl = 162;
        //
        // Summary:
        //     The right CTRL key.
        public static Key RControl = 163;
        //
        // Summary:
        //     The left ALT key.
        public static Key LMenu = 164;
        //
        // Summary:
        //     The right ALT key.
        public static Key RMenu = 165;
        //
        // Summary:
        //     The browser back public static Key (Windows 2000 or later).
        public static Key BrowserBack = 166;
        //
        // Summary:
        //     The browser forward public static Key (Windows 2000 or later).
        public static Key BrowserForward = 167;
        //
        // Summary:
        //     The browser refresh public static Key (Windows 2000 or later).
        public static Key BrowserRefresh = 168;
        //
        // Summary:
        //     The browser stop public static Key (Windows 2000 or later).
        public static Key BrowserStop = 169;
        //
        // Summary:
        //     The browser search public static Key (Windows 2000 or later).
        public static Key BrowserSearch = 170;
        //
        // Summary:
        //     The browser favorites public static Key (Windows 2000 or later).
        public static Key BrowserFavorites = 171;
        //
        // Summary:
        //     The browser home public static Key (Windows 2000 or later).
        public static Key BrowserHome = 172;
        //
        // Summary:
        //     The volume mute public static Key (Windows 2000 or later).
        public static Key VolumeMute = 173;
        //
        // Summary:
        //     The volume down public static Key (Windows 2000 or later).
        public static Key VolumeDown = 174;
        //
        // Summary:
        //     The volume up public static Key (Windows 2000 or later).
        public static Key VolumeUp = 175;
        //
        // Summary:
        //     The media next track public static Key (Windows 2000 or later).
        public static Key MediaNextTrack = 176;
        //
        // Summary:
        //     The media previous track public static Key (Windows 2000 or later).
        public static Key MediaPreviousTrack = 177;
        //
        // Summary:
        //     The media Stop public static Key (Windows 2000 or later).
        public static Key MediaStop = 178;
        //
        // Summary:
        //     The media play pause public static Key (Windows 2000 or later).
        public static Key MediaPlayPause = 179;
        //
        // Summary:
        //     The launch mail public static Key (Windows 2000 or later).
        public static Key LaunchMail = 180;
        //
        // Summary:
        //     The select media public static Key (Windows 2000 or later).
        public static Key SelectMedia = 181;
        //
        // Summary:
        //     The start application one public static Key (Windows 2000 or later).
        public static Key LaunchApplication1 = 182;
        //
        // Summary:
        //     The start application two public static Key (Windows 2000 or later).
        public static Key LaunchApplication2 = 183;
        //
        // Summary:
        //     The Semicolon public static Key on a US standard keyboard (Windows 2000 or later).
        public static Key Semicolon = 186;
        //
        // Summary:
        //     The plus public static Key on any country/region keyboard (Windows 2000 or later).
        public static Key Plus = 187;
        //
        // Summary:
        //     The comma public static Key on any country/region keyboard (Windows 2000 or later).
        public static Key Comma = 188;
        //
        // Summary:
        //     The minus public static Key on any country/region keyboard (Windows 2000 or later).
        public static Key Minus = 189;
        //
        // Summary:
        //     The period public static Key on any country/region keyboard (Windows 2000 or later).
        public static Key Period = 190;
        //
        // Summary:
        //     The question mark public static Key on a US standard keyboard (Windows 2000 or later).
        public static Key Question = 191;
        //
        // Summary:
        //     The tilde public static Key on a US standard keyboard (Windows 2000 or later).
        public static Key Tilde = 192;
        //
        // Summary:
        // Summary:
        //     The singled/double quote public static Key on a US standard keyboard (Windows 2000
        //     or later).
        public static Key Quotes = 222;
        //
        // Summary:
        //     The angle bracket or backslash public static Key on the RT 102 public static Key keyboard (Windows
        //     2000 or later).
        public static Key Backslash = 226;
        //
        // Summary:
        //     The PROCESS public static Key key.
        public static Key Processpublic = 229;
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key
        //     value is the low word of a 32-bit virtual-public static Key value used for non-keyboard
        //     input methods.
        public static Key Packet = 231;
        //
        // Summary:
        //     The ATTN key.
        public static Key Attn = 246;
        //
        // Summary:
        //     The CRSEL key.
        public static Key Crsel = 247;
        //
        // Summary:
        //     The EXSEL key.
        public static Key Exsel = 248;
        //
        // Summary:
        //     The ERASE EOF key.
        public static Key EraseEof = 249;
        //
        // Summary:
        //     The PLAY key.
        public static Key Play = 250;
        //
        // Summary:
        //     The ZOOM key.
        public static Key Zoom = 251;
        //
        // Summary:
        //     A constant reserved for future use.
        public static Key NoName = 252;
        //
        // Summary:
        //     The PA1 key.
        public static Key Pa1 = 253;
        //
        // Summary:
        //     The CLEAR key.
        public static Key ClearKey = 254;
        //
        // Summary:
        //     The SHIFT modifier key.
        public static Key ShiftMod = 65536;
        //
        // Summary:
        //     The CTRL modifier key.
        public static Key CtrlMod = 131072;
        //
        // Summary:
        //     The ALT modifier key.
        public static Key AltMod = 262144;
        //
        // Summary:
        //     The open bracket public static Key on a US standard keyboard (Windows 2000 or later).
        public static Key OpenBrackets = 219;
        //
        // Summary:
        //     The pipe public static Key on a US standard keyboard (Windows 2000 or later).
        public static Key Pipe = 220;
        //
        // Summary:
        //     The close bracket public static Key on a US standard keyboard (Windows 2000 or later).
        public static Key CloseBrackets = 221;
    }
}
