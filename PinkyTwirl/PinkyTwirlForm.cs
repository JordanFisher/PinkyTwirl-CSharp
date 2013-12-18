using System;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using WindowsInput;

namespace PinkyTwirl
{
    public partial class PinkyTwirlForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick_Down()
        {
            // Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
        }

        public void DoMouseClick_Up()
        {
            // Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
                return Buff.ToString();
            else
                return "";
        }

        static PinkyTwirlForm TheForm;
        public PinkyTwirlForm()
        {
            TheForm = this;

            InitializeComponent();
            InitDicts();

            this.ActiveCheckbox.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        void Activate()
        {
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;
        }

        void Deactivate()
        {
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.KeyUp -= HookManager_KeyUp;
        }


        bool[] _IsKeyDown = new bool[10000000];
        //Dictionary<Keys, bool> IsKeyDown = new Dictionary<Keys, bool>(500);

        Dictionary<string, Dictionary<Keys, Dictionary<Keys, ShortcutAction>>> WindowMap;
        List<Keys> AmbiguousKeys = new List<Keys>(new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.F });

        //bool SkipUp = false;
        //bool SkipDown = false;
        ShortcutAction Down(VirtualKeyCode key)
        {
            return (Action)(() => _Down(key));
        }
        void _Down(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyDown(key);
            //SkipUp = true;
            //SendKeys.Send(keys);
            //SkipUp = false;
        }
        ShortcutAction Up(VirtualKeyCode key)
        {
            return (Action)(() => _Up(key));
        }
        void _Up(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyUp(key);
            //SkipUp = true;
            //Sendkey.Send(key);
            //SkipUp = false;
        }
        void _Press(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyDown(key);
            InputSimulator.SimulateKeyUp(key);
        }

        void Down()
        {
            for (int i = 0; i < 12; i++)
                InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
        }

        void ShiftDown()
        {
            for (int i = 0; i < 12; i++)
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.DOWN);
        }

        void Up()
        {
            for (int i = 0; i < 12; i++)
                InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
        }            

        void ShiftUp()
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
            for (int i = 0; i < 12; i++)
                InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
        }

        class ShortcutAction
        {
            string StringAction = null;
            Action ActionAction = null;

            public ShortcutAction(string StringAction) { this.StringAction = StringAction; }
            public ShortcutAction(Action ActionAction) { this.ActionAction = ActionAction; }

            public static implicit operator ShortcutAction(string StringAction)
            {
                return new ShortcutAction(StringAction);
            }

            public static implicit operator ShortcutAction(Action ActionAction)
            {
                return new ShortcutAction(ActionAction);
            }

            public void Go()
            {
                if (StringAction != null)
                {
                    if (DoLog) Log(string.Format("    {0} sent.", StringAction));
                    SendKeys.Send(StringAction);
                    SendKeys.Flush();
                    //SendKeys.SendWait(StringAction);
                }
                if (ActionAction != null)
                {
                    if (DoLog) Log("    Action taken");
                    ActionAction();
                }
            }
        }

      /*  void Template()
        {
            string name = Clipboard.GetText();

//            string template =
//@"class {0} : Lambda
//{{
//    public {0}()
//    {{
//    }}
//
//    public void Apply()
//    {{
//    }}
//}}";

            //string template = "std::shared_ptr<{0}>";

            string newtext = string.Format(template, name);

            Clipboard.SetText(newtext);
            SendKeys.Send("^v");
        }*/

        void ToDesktop()
        {
            _Down(VirtualKeyCode.LWIN);
            SendKeys.Send("D");
            _Up(VirtualKeyCode.LWIN);
        }
        void Space()
        {
            FuncKey = Keys.F13;
            var Map = GetMap();
            FuncMap = Map[FuncKey];
        }
        void AltF4()
        {
            //_Down("{ALT}");
            SendKeys.Send("%{F4}");
            //_Up("{ALT}");
        }
        void StartAltTab()
        {
            _Down(VirtualKeyCode.LMENU);
            _Down(VirtualKeyCode.TAB);
            //SendKeys.Send("{TAB}");
            StartCheckingToEndCtrlTab(MainFuncKey);
        }
        void StartCtrlTab_Up()
        {
            _Down(VirtualKeyCode.LCONTROL);
            SendKeys.Send("{TAB}");
            StartCheckingToEndCtrlTab(Keys.D2);
        }
        void StartCtrlTab_Down()
        {
            _Down(VirtualKeyCode.LCONTROL);
            SendKeys.Send("+{TAB}");
            StartCheckingToEndCtrlTab(Keys.D2);
        }

        void StartCheckingToEndCtrlTab(Keys Key)
        {
            if (CheckingToEndCtrlTab) return;
            KeyToEndOn = Key;
            HookManager.KeyUp += CheckToEndCtrlTab;
            CheckingToEndCtrlTab = true;
        }
        void DebugCloudberry()
        {
            Skip = true;
            _Down(VirtualKeyCode.LWIN);
            _Down(VirtualKeyCode.VK_1);
            _Up(VirtualKeyCode.VK_1);
            _Up(VirtualKeyCode.LWIN);

            //System.Threading.Thread.Sleep(500);
            //SendKeys.Flush();
            //SendKeys.SendWait("^\'");
            //SendKeys.Flush();
            //SendKeys.SendWait("^{TAB}");
            //SendKeys.SendWait("^\'^{TAB}");

            //_Down(VirtualKeyCode.LMENU);
            //_Press(VirtualKeyCode.VK_1);
            //_Press(VirtualKeyCode.TAB);
            //_Up(VirtualKeyCode.LMENU);

            //SendKeys.Send("^\'^{TAB}");
            Skip = false;
        }
        bool CheckingToEndCtrlTab = false;
        Keys KeyToEndOn;
        void CheckToEndCtrlTab(object sender, KeyEventArgs e)
        {
            if (Skip) return;

            //if (Map.ContainsKey(e.KeyCode))
            if (e.KeyCode == KeyToEndOn)
            {
                HookManager.KeyUp -= CheckToEndCtrlTab;
                CheckingToEndCtrlTab = false;

                Skip = true;
                _Up(VirtualKeyCode.LCONTROL);
                _Up(VirtualKeyCode.LMENU);
                _Up(VirtualKeyCode.TAB);
                Skip = false;

                if (DoLog) Log(string.Format("    Ending alt tab via function key {0}", e.KeyCode));
            }
        }

        string KillLine = "{HOME}{HOME}+{END}{DEL}";
        void Kill() { }

        Dictionary<T1, Dictionary<T2, K>> DeepCopy<T1, T2, K>(Dictionary<T1, Dictionary<T2, K>> source)
        {
            var d = new Dictionary<T1, Dictionary<T2, K>>();
            foreach (var pair1 in source)
            {
                d.Add(pair1.Key, new Dictionary<T2, K>());
                foreach (var pair2 in pair1.Value)
                    d[pair1.Key].Add(pair2.Key, pair2.Value);
            }

            return d;
        }

        const Keys MainFuncKey = Keys.F12;
        //const Keys MainFuncKey = Keys.RMenu;
        //const Keys MainFuncKey = Keys.Oemtilde;

        void InitDicts()
        {
            var DefaultMap = new Dictionary<Keys, Dictionary<Keys, ShortcutAction>>()
            {
                // Alt mapping
                { 
                    MainFuncKey, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        //{ Keys.Z, (Action)Template },
                        { Keys.Z, "std::shared_ptr<" },

                        { Keys.I , "{UP}" }, { Keys.J , "{LEFT}"}, { Keys.K , "{DOWN}"}, { Keys.L , "{RIGHT}"},
                        { Keys.M , "^{RIGHT}"}, { Keys.N , "^{LEFT}"},
                        { Keys.Oemcomma, "^["}, { Keys.OemPeriod , "^]"},
                        { Keys.D9 , "{PGUP}"}, { Keys.D0 , "{PGDN}"},
                        { Keys.U , "{HOME}"}, { Keys.O , "{END}"}, { Keys.Y , (Action)Up}, { Keys.H , (Action)Down}, 
                        { Keys.P , "^{HOME}"}, { Keys.OemSemicolon , "^{END}"},
                        { Keys.Tab , (Action)StartAltTab},
                        { Keys.F4 , (Action)AltF4},

                        // Kill                
                        { Keys.Escape , (Action)Kill},      
                        
                        // Add line above current line
                        { Keys.Enter , "{UP}{END}{ENTER}"},

                        // Go to address bar
                        { Keys.F , "{F6}%d"}, { Keys.G , "%d{F6}{F6}"}, { Keys.D , (Action)ToDesktop},

                        // Find      // Find and replace
                        { Keys.A , "^f"}, { Keys.Q , "^h"},
                        // undo      // Redo      // Paste
                        { Keys.E , "^z"}, { Keys.R , "^y"}, { Keys.W , "^v"},

                        // Special Alt + Space + AdditionalKey command. Once the function Space is ran}, { PinkyTwirl waits to hear for an additional third key.
                        // This is a hack. Eventually robust support for 3-key combos will be added.
                        { Keys.Space, (Action)Space }
                    }
                },

                // 4-mapping: Select/Highlight
                { Keys.D4, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        { Keys.I , "+{UP}"}, { Keys.J , "+{LEFT}"}, { Keys.K , "+{DOWN}"}, { Keys.L , "+{RIGHT}"},
                        { Keys.M , "+^{RIGHT}"}, { Keys.N , "+^{LEFT}"},
                        { Keys.U , "+{HOME}"}, { Keys.O , "+{END}"}, { Keys.Y , "+{UP 6}"}, { Keys.H , "+{DOWN 6}"},
                        { Keys.P , "+^{HOME}"}, { Keys.OemSemicolon, "+^{END}"},

                        // Copy              Paste             Cut
                        {  Keys.R , "^c"}, { Keys.T , "^v"}, { Keys.F , "^x" }
                    }
                },

                // 3-mapping, Delete
                { Keys.D3, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        { Keys.I , "{UP}" + KillLine + KillLine},
                        { Keys.J , "{BS}"}, { Keys.K , KillLine}, { Keys.L , "{DEL}"},
                        { Keys.M , "+^{RIGHT}{DEL}"}, { Keys.N , "+^{LEFT}{DEL}"},
                        { Keys.U , "+{HOME}{DEL}"}, { Keys.O , "+{END}{DEL}"}, { Keys.Y , "+{PGUP}{DEL}"}, { Keys.H , "+{PGDN}{DEL}" }
                    }
                },

                // 2-mapping, File tabs
                { Keys.D2, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        // Switch         Switch back     Close all but this   Close     Save
                        { Keys.J , (Action)StartCtrlTab_Up }, { Keys.K , (Action)StartCtrlTab_Down }, { Keys.P , "+^;" }, {          Keys.O , "^;" }, { Keys.I , "^s" }
                    }
                 },

                // 1-mapping, easy typing
                { Keys.D1, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        { Keys.J, "std::shared_ptr<" },
                        { Keys.K, "std::static_pointer_cast<" }
                    }
                 },
				 /*
                // F-mapping, easy chars
                { Keys.F, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        { Keys.J, "{(}" },
                        { Keys.K, "{)}" }
                    }
                 },*/

                 // Alt-Space mapping
                 {
                     Keys.F13,
                        new Dictionary<Keys, ShortcutAction>
                        {
                            { Keys.H, "^5^5{ESC}{ESC}"}, { Keys.J, "^8"}, { Keys.K, "^9^9"},
                            { Keys.U, "^we"}, { Keys.I, "^wo"},
                            { Keys.O, "^wq"}, { Keys.P, "^di"},
                            { Keys.Y, "^66"}, { Keys.N, "^dc"},
                            { Keys.F, "^/>open "},
        
							// WiiU network shenanigans
							{ Keys.W, "{TAB}19216810{RIGHT}3{TAB}2552552550{TAB}19216810{RIGHT}1"},

							

                            //{ "D", BringVisualStudio},
                            //{ "F", BringChrome},
                            //{ "E", BringToDo},
                            //{ "R", BringCkToDo},
                            //{ "V", BringGit },
                        }
                  }
            };


            // Simple map, used for applications where you don"t want anything fancy.
            // Because we are overriding some default Alt behavior, we still need to implement that basic functionality (Alt-Tab and Alt-F4)
            var SimpleMap = new Dictionary<Keys, Dictionary<Keys, ShortcutAction>>()
            {
                // Alt mapping
                { MainFuncKey, 
                    new Dictionary<Keys, ShortcutAction>
                    {
                        //{ Keys.Tab, (Action)StartAltTab },
                        { Keys.F4, (Action)AltF4 }
                    }
                 }
            };


            // Visual Studios map
            var VisualStudioMap = DeepCopy(DefaultMap);
            VisualStudioMap[MainFuncKey][Keys.B] = "^\'^{TAB}";
            VisualStudioMap[MainFuncKey][Keys.G] = "^mm"; // Collapse scope
            VisualStudioMap[MainFuncKey][Keys.F] = "^,"; // Go to definition
            VisualStudioMap[MainFuncKey][Keys.D] = "^."; // Find all references
            VisualStudioMap[MainFuncKey][Keys.S] = "^1"; // Rename
            //VisualStuioMap[MainFuncKey][Keys.G] = "{F3}" // Search again
            //VisualStuioMap[MainFuncKey][Keys.F] = "^22" // Incremental search
            //VisualStuioMap[MainFuncKey][Keys.D] = "^23" // Backward incremental search
            //VisualStuioMap[MainFuncKey][Keys.S] = "{ESC}" // Clear search
            VisualStudioMap[MainFuncKey][Keys.T] = "+%{ENTER}"; // Fullscreen
            VisualStudioMap[MainFuncKey][Keys.Space] = (Action)Space;

            // Notepad map example. I use Notepad as a food diary, where I track my food, symptoms, and workout schedule. To facillitate data entry I use these shortcuts:
            var NotepadMap = DeepCopy(DefaultMap);
            //NotepadMap[MainFuncKey][Keys.F] = KillLine + "//Ate{SPACE}"
            //NotepadMap[MainFuncKey][Keys.D] = KillLine + "//Data{SPACE}"
            //NotepadMap[MainFuncKey][Keys.S] = KillLine + "//Action{SPACE}"
            //def Day():
            //    now = datetime.now()
            //    return "//Day{SPACE}%d-%d-%d{ENTER}{ENTER}" % (now.month, now.day, now.year)
            //NotepadMap[MainFuncKey][Keys.R] = Day

            // Command prompt. This overrides the tedious Alt-Space e p method for pasting with the default PinkyTwirl paste command Alt + W
            var CommandPromptMap = DeepCopy(DefaultMap);
            CommandPromptMap[MainFuncKey][Keys.W] = "%{SPACE}ep";
            CommandPromptMap[Keys.D4][Keys.R] = "%{SPACE}es{ENTER}";
            CommandPromptMap[MainFuncKey][Keys.A] = "%{SPACE}ef{ENTER}";
            CommandPromptMap[MainFuncKey][Keys.F4] = "%{SPACE}c";

            // Notepad++
            var NotepadPlusPlusMap = DeepCopy(DefaultMap);
            NotepadPlusPlusMap[Keys.D2][Keys.O] = "^w";
            //NotepadPlusPlusMap[Keys.Capital][Keys.J] = NotepadPlusPlusMap[MainFuncKey][Keys.Oem_Comma] = "^k"
            //NotepadPlusPlusMap[Keys.Capital][Keys.K] = NotepadPlusPlusMap[MainFuncKey][Keys.Oem_Period] = "^+k"

            var WingIdeMap = DeepCopy(NotepadPlusPlusMap);

            var LEdMap = DeepCopy(NotepadPlusPlusMap);

            // WinSCP is a barebones text editor I occasionally use to remotely edit Python files. It doesn"t support spaced tabs, so instead PinkyTwirl replaces Tab with spaces.
            //var WinSCP = DeepCopy(DefaultMap);
            //WinSCP[None] = { };
            //WinSCP[None][Keys.Tab] = "{SPACE 4}";

            // Chrome remapping. Quickly open and close tabs, as well as get to the Omnibox.
            var ChromeMap = DeepCopy(DefaultMap);
            ChromeMap[MainFuncKey][Keys.F] = "^l";
            ChromeMap[Keys.D2][Keys.O] = "^w";
            ChromeMap[Keys.D2][Keys.I] = "^t";

            // Git map
            var GitMap = DeepCopy(CommandPromptMap);
            GitMap[MainFuncKey][Keys.F] = (Action)Commit;
            GitMap[MainFuncKey][Keys.D] = (Action)GitToCloudberry;
            GitMap[Keys.D3][Keys.O] = "^k";
            GitMap[Keys.D3][Keys.U] = "^u";
            GitMap[Keys.D3][Keys.K] = "{HOME}^k";
            GitMap[Keys.D3][Keys.N] = "^w";
            //GitMap[Keys.D3][Keys.M] = "^l";
            GitMap[MainFuncKey][Keys.N] = "%b";
            GitMap[MainFuncKey][Keys.M] = "%f";

            /*
            // Photoshop mapu
            var PhotoshopMap = DeepCopy(DefaultMap);
            PhotoshopMap.Add(Keys.D1,
                    new Dictionary<Keys, ShortcutAction>
                    {
                        //{ Keys.P , "+^s{WAIT}{TAB}{DOWN 18}{ENTER}+{TAB}" }
                        //{ Keys.P , "{TAB}{DOWN 18}{ENTER}+{TAB}" }
                        //{ Keys.P, (Action)SaveAsPNG }
                    });

            // SaveAs map
            var SaveAsMap = DeepCopy(DefaultMap);
            SaveAsMap.Add(Keys.D1,
                    new Dictionary<Keys, ShortcutAction>
                    {
                        { Keys.P , (Action)SaveAsPNG }
                    });*/

            // Cloudberry game map
            var CloudberryMap = DeepCopy(SimpleMap);
            CloudberryMap[MainFuncKey][Keys.B] = (Action)DebugCloudberry;

			// DOTA map
			var DotaMap = DeepCopy(DefaultMap);
			DotaMap[MainFuncKey][Keys.A] = "G";
			DotaMap[MainFuncKey][Keys.S] = "H";
			DotaMap[MainFuncKey][Keys.D] = "J";
			DotaMap[MainFuncKey][Keys.F] = "K";
			
			DotaMap[MainFuncKey][Keys.D1] = "%G";
			DotaMap[MainFuncKey][Keys.D2] = "%H";
			DotaMap[MainFuncKey][Keys.D3] = "%J";
			DotaMap[MainFuncKey][Keys.Q] = "%K";
			DotaMap[MainFuncKey][Keys.W] = "%L";
			DotaMap[MainFuncKey][Keys.E] = "%M";

			// Excel
			var ExcelMap = DeepCopy(DefaultMap);
			ExcelMap[MainFuncKey][Keys.Enter] = "+ ^{+}";
			ExcelMap[Keys.D3][Keys.K] = "+ ^-";


            // Application mapping. Determines which application gets which dictionary of commands.
            // We take the name of the current window and look to see if it contains any of the following strings.
            // If a match is found we use the associated dictionary. Matching works sequentially.
            WindowMap = new Dictionary<string, Dictionary<Keys, Dictionary<Keys, ShortcutAction>>>
            {
                {"Chrome", ChromeMap},
                {"Microsoft Visual", VisualStudioMap},
                {"POS Editor", SimpleMap},
                {"Notepad++", NotepadPlusPlusMap},
                {"Notepad", NotepadMap},
                {"Wing IDE", WingIdeMap},
                {"Command Prompt", CommandPromptMap},
                {"MINGW", GitMap},
                {"shell", CommandPromptMap},
                {"IPython", CommandPromptMap},
                {"LEd", LEdMap},
                //{"@", PhotoshopMap},
                //{"Save", SaveAsMap},
                {"Cloudberry Kingdom ", CloudberryMap},
				{"DOTA", DotaMap},
				{"Excel", ExcelMap},
                {"__DEFAULT__", DefaultMap }
                //{"/", WinSCP}
            };
        }

        void Commit()
        {
            SendKeys.Send("{HOME}^k");
            SendKeys.Send("git add . ; git commit -a");
        }
        void GitToCloudberry()
        {
            SendKeys.Send("{HOME}^k");
            SendKeys.Send("cd /c/Users/Ezra/Desktop/Dir/Pwnee/CK/Cloudberry\\ Kingdom");
        }

        void SaveAsPNG()
        {
            Skip = true;
            //SendKeys.Send("+^s");
            SendKeys.Send("+{TAB}+{TAB}{DOWN} ");
            System.Threading.Thread.Sleep(150);
            SendKeys.Send("{TAB 3}pppppppp+{TAB}");
            //SendKeys.Send("{TAB}{DOWN 18}{ENTER}+{TAB}");
            Skip = false;
        }
        
        /*
'1' : {
    'J' : BringVisualStudio}, {
    'K' : BringChrome}, {
    'I' : BringToDo}, {
    'O' : BringCkToDo}, {
    'U' : BringGit}, {
    }}, {
'Capital' : {
    'J' : BringVisualStudio}, {
    'K' : BringChrome}, {
    'I' : BringToDo}, {
    'O' : BringCkToDo}, {
    'U' : BringGit}, {
    }}, {
*/

        static void Log(string str)
        {
            if (!DoLog) return;

            TheForm.textBoxLog.AppendText(str);
            TheForm.textBoxLog.AppendText("\n");
            TheForm.textBoxLog.ScrollToCaret();
        }

        Dictionary<Keys, Dictionary<Keys, ShortcutAction>> GetMap()
        {
            var name = GetActiveWindowTitle();

            int Count = 0;
            foreach (var kv in WindowMap)
            {
                if (name.Contains(kv.Key))
                {
                    if (DoLog) Log(Count.ToString());
                    return kv.Value;
                }
                Count++;
            }

            return WindowMap["__DEFAULT__"];
        }

        char HoldAmbiguous;
        bool Ambiguous = false;
        bool Skip = false;
        Keys FuncKey = Keys.None;
        Dictionary<Keys, ShortcutAction> FuncMap;
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (Skip) return;

            //if (e.KeyCode == Keys.H) Console.Write("");

            try
            {
                // Shift-Shift = CapsLock
                if (e.KeyCode == Keys.LShiftKey && _IsKeyDown[(int)Keys.RShiftKey] && !_IsKeyDown[(int)Keys.LShiftKey] ||
                    e.KeyCode == Keys.RShiftKey && _IsKeyDown[(int)Keys.LShiftKey] && !_IsKeyDown[(int)Keys.RShiftKey])
                {
                    Skip = true;
                    InputSimulator.SimulateKeyPress(VirtualKeyCode.CAPITAL);
                    //SendKeys.Send("{CAPSLOCK}");
                    //Log("Caps");
                    Skip = false;
                    return;
                }

                // Convert tilde into Left Mouse Button.
				//if (e.KeyCode == Keys.Oemtilde)
				//{
				//    if (!_IsKeyDown[(int)e.KeyCode])
				//    {
				//        Skip = true;
				//        for (int i = 0; i < 50000; i++)
				//        {
				//            DoMouseClick_Down();
				//            System.Threading.Thread.Sleep(5);
				//            DoMouseClick_Up();
				//        }
				//        Skip = false;
						
				//        e.Handled = true; e.SuppressKeyPress = true;
				//        _IsKeyDown[(int)e.KeyCode] = true;
				//    }

				//    return;
				//}

                // Mark key as down.
                _IsKeyDown[(int)e.KeyCode] = true;

                bool ctrl = InputSimulator.IsKeyDown(VirtualKeyCode.CONTROL);
                bool shift = InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT);
                bool alt = InputSimulator.IsKeyDown(VirtualKeyCode.MENU);
                if (DoLog) Log(string.Format("Window name is {0}", GetActiveWindowTitle()));
                if (DoLog) Log(string.Format("KeyDown {0} ({4}) {1}{2}{3}", e.KeyCode, shift ? " Shift" : "", alt ? " Alt" : "", ctrl ? " Cntrl" : "", (char)e.KeyValue));

                var Map = GetMap();

                //if (IsKeyDown(Keys.Y) && IsKeyDown(Keys.H))
                //  if (DoLog) Log("Both down!");

                if (Map.ContainsKey(e.KeyCode))
                {
                    if (DoLog) Log(string.Format("Function key {0} pressed", e.KeyCode));

                    FuncKey = e.KeyCode;
                    FuncMap = Map[FuncKey];

                    e.Handled = true; e.SuppressKeyPress = true;

                    if (AmbiguousKeys.Contains(FuncKey))
                    {
                        Ambiguous = true;
                        HoldAmbiguous = (char)e.KeyValue;
                    }

                    return;
                }

                if (FuncKey != Keys.None && FuncMap.ContainsKey(e.KeyCode))
                {
                    if (DoLog) Log(string.Format("Func {0} + KeyDown - {1}", FuncKey, e.KeyCode));

                    Ambiguous = false;

                    e.Handled = true; e.SuppressKeyPress = true;

                    //SendKeys.Flush();
                    Skip = true;
                    FuncMap[e.KeyCode].Go();
                    //SendKeys.Flush();
                    Skip = false;
                }
            }
            catch
            {
                Log("");
                Log("Error uncaught!");
                Log("");
            }
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (Skip) return;

            try
            {
                var Map = GetMap();

                if (Map.ContainsKey(e.KeyCode))
                {
                    if (DoLog) Log(string.Format("Function key {0} released.", e.KeyCode));
                    FuncKey = Keys.None;

                    if (Ambiguous)
                    {
                        Ambiguous = false;

                        Skip = true;
                        SendKeys.Send(HoldAmbiguous.ToString());
                        //SendKeys.Flush();
                        Skip = false;
                    }
                }

                _IsKeyDown[(int)e.KeyCode] = false;

                // Convert tilde into Left Mouse Button.
				//if (e.KeyCode == Keys.Oemtilde)
				//{
				//    Skip = true;
				//    DoMouseClick_Up();
				//    Skip = false;
                    
				//    e.Handled = true; e.SuppressKeyPress = true;
				//    return;
				//}

                if (DoLog) Log(string.Format("KeyUp - {0}", e.KeyCode));
            }
            catch
            {
                Log("");
                Log("Error uncaught!");
                Log("");
            }
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

        static bool DoLog = false;
        private void LogCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            DoLog = this.LogCheckbox.Checked;
        }
    }
}