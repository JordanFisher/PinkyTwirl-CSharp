using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HookManager;
using WindowsInput;

namespace PinkyTwirl
{
    public class Semantics : Functions
    {
        public static SemanticAction
            // Text navigation
            NavLeft         = None + Left,
            NavRight        = None + Right,
            NavUp           = None + Up,
            NavDown         = None + Down,
            NavHome         = None + Home,
            NavEnd          = None + End,
            NavPageUp       = None + PageUp,
            NavPageDown     = None + PageDown,
            NavNextWord     = Ctrl + Right,
            NavPreviousWord = Ctrl + Left,
            NavBigUp        = 12 * Up,
            NavBigDown      = 12 * Down,
            NavTop          = Ctrl + Home,
            NavBottom       = Ctrl + End,

            // Text selection
            SelectLeft         = Shift + Left,
            SelectRight        = Shift + Right,
            SelectUp           = Shift + Up,
            SelectDown         = Shift + Down,
            SelectHome         = Shift + Home,
            SelectEnd          = Shift + End,
            SelectPageUp       = Shift + PageUp,
            SelectPageDown     = Shift + PageDown,
            SelectNextWord     = Shift + NavNextWord,
            SelectPreviousWord = Shift + NavPreviousWord,
            SelectBigUp        = Shift + NavBigUp,
            SelectBigDown      = Shift + NavBigDown,
            SelectTop          = Shift + NavTop,
            SelectBottom       = Shift + NavBottom,

            // Text deletion
            DeleteLine         = Home | Home | Shift + End | Delete,
            DeleteLeft         = None + Backspace,
            DeleteRight        = None + Delete,
            DeleteUp           = NavUp   | DeleteLine,
            DeleteDown         = NavDown | DeleteLine,
            DeleteHome         = SelectHome | Delete,
            DeleteEnd          = SelectEnd  | Delete,
            DeletePageUp       = SelectPageUp   | Delete,
            DeletePageDown     = SelectPageDown | Delete,
            DeleteNextWord     = SelectNextWord     | Delete,
            DeletePreviousWord = SelectPreviousWord | Delete,
            DeleteBigUp        = SelectBigUp   | Delete,
            DeleteBigDown      = SelectBigDown | Delete,
            DeleteTop          = SelectTop    | Delete,
            DeleteBottom       = SelectBottom | Delete,
            
            // Text action
            Cut            = Ctrl + X,
            Copy           = Ctrl + C,
            Paste          = Ctrl + V,
            InsertLine     = Up | End | Enter,
            Rewrap         = Alt + M,

            // Search/replace
            Search    = Ctrl + F,
            SearchAll = Ctrl + F,
            Replace   = Ctrl + H,

            // State
            Undo           = Ctrl + Z,
            Redo           = Ctrl + Y,

            // Navigate
            AddressBar     = Alt + D,
            Focus          = AddressBar | F6 | F6 | F6,
            ProgramTab     = (Action)StartAltTab,
            FileTabLeft    = (Action)StartCtrlTab_Up,
            FileTabRight   = (Action)StartCtrlTab_Down,
            Menu           = None + Alt,

            // File actions
            Save           = Ctrl + S,
            SaveAs         = Ctrl + Alt + S,
            SaveAll        = Ctrl + Shift + S,
            Close          = Ctrl + Semicolon,
            CloseAllBut    = Ctrl + Shift + Semicolon,
            FormatDoc      = Shift + Alt + F,

            // Brower
            NewTab         = Ctrl + T,
            CloseTab       = Ctrl + W,
            SearchTabs     = (SemanticAction)Mouse.ClickChromeTabSearch, //Ctrl + Shift + A,

            // Application
            Fullscreen     = None + F11,
            EndApplication = Alt + F4,

            // Game

            // Console
            ClearScreen = 0,

            // tmux
            PaneLeft = Ctrl + B | Left,
            PaneRight = Ctrl + B | Right,
            PaneUp = Ctrl + B | Up,
            PaneDown = Ctrl + B | Down,
            PaneSyncOn = Ctrl + B | ":" | "setw synchronize-panes on" | Enter,
            PaneSyncOff = Ctrl + B | ":" | "setw synchronize-panes off" | Enter,
            PaneNew = Ctrl + B | Shift + D5,
            PaneKill = Ctrl + B | X | Y,
            PaneArrange = Ctrl + B | Alt + D5,
            PaneBig = Ctrl + B | Z,

            // git
            GitStatus = DeleteLine | "clear ; git status -- \"******\"" | 4 * Left,
            GitDiff = DeleteLine | "clear ; git diff -- \"******\"" | 4 * Left,
            GitAdd = DeleteLine | "clear ; git add -- \"******\" ; git status" | 17 * Left,
            GitCommit = DeleteLine | "git commit -m\"\"" | Left,
            GitPush = DeleteLine | "git push origin ",
            GitReset = DeleteLine | "git reset ; clear ; git status",
            GitResetHard = DeleteLine | "git reset --hard HEAD ; clear ; git status",
            ReverseSearch = Ctrl + R,

            // IDE
            Debug                     = 0,
            Comment                   = 0,
            Uncomment                 = 0,
            Rename                    = 0,
            FindAllReferences         = 0,
            GotoDefinition            = 0,
            CollapseScope             = 0,
            ExpandScope               = 0,
            ViewProjectExplorer       = 0,
            ViewErrorList             = 0,
            ViewOutput                = 0,
            ViewSymbols               = 0,
            ViewFindResults           = 0,
            ViewCallStack             = 0,
            ViewInteractive           = 0,
            ViewClasses               = 0,
            ViewConfigurationSelector = 0,
            CloseAllTools             = 0,
            ViewOpenFiles             = 0,
            ViewDebugConsole          = 0,
            ViewTerminal              = 0,
            FindFile                  = 0;

        public static void Initialize()
        {
            UsingContext(Contexts.VSCode);
                SearchAll                 %= Ctrl + Shift + F;
                Close                     %= Ctrl + W;
                ViewProjectExplorer       %= Ctrl + Shift + E;
                CloseAllTools             %= Ctrl + B;
                FindFile                  %= Ctrl + E;
                ViewErrorList             %= Ctrl + Shift + M;
                Fullscreen                %= Ctrl + K | Z;
                GotoDefinition            %= F12;
                ViewOutput                %= Ctrl + Shift + U;
                ViewDebugConsole          %= Ctrl + Shift + Y;
                ViewTerminal              %= Ctrl + Tilde;
                Comment                   %= Ctrl + Question;// K | Ctrl + C;
                Uncomment                 %= Ctrl + K | Ctrl + U;

            UsingContext(Contexts.VisualStudio);
                Fullscreen                %= Shift + Alt + Enter;
                Close                     %= Ctrl + Semicolon;

                //Debug                     %= Ctrl + Quotes | Ctrl + Tab;
                Debug                     %= Ctrl + Tilde| Ctrl + Tab;
                Comment                   %= Ctrl + OpenBrackets;
                Uncomment                 %= Ctrl + CloseBrackets;
                Rename                    %= Ctrl + D1;
                SearchAll                 %= Shift + Ctrl + F | Alt + F;
                FindAllReferences         %= Ctrl + Period;
                GotoDefinition            %= Ctrl + Comma;
                CollapseScope             %= Ctrl + M | M;
                ExpandScope               %= CollapseScope;
                ViewProjectExplorer       %= Ctrl + D8;
                ViewErrorList             %= Ctrl + W | E;
                ViewOutput                %= Ctrl + W | O;
                ViewSymbols               %= Ctrl + W | Q;
                ViewFindResults           %= Ctrl + W | F;
                ViewCallStack             %= Ctrl + D | C;
                
                ViewInteractive           %= Ctrl + D | I;
                ViewClasses               %= Ctrl + D9 | D9;
                ViewConfigurationSelector %= Ctrl + D6 | D6;

                CloseAllTools             %= Escape | Ctrl + M | D1;

            UsingContext(Contexts.NotepadPlusPlus);
                Fullscreen                %= F11;
                Close                     %= Ctrl + W;

                Comment                   %= Ctrl + K;
                Uncomment                 %= Ctrl + Shift + K;
                SearchAll                 %= Ctrl + F;
                GotoDefinition            %= Ctrl + G;
                CollapseScope             %= Ctrl + Alt + F;
                ExpandScope               %= Shift + Ctrl + Alt + F;

            UsingContext(Contexts.Excel);
                DeleteLine %= Shift + Space | Ctrl + Minus;
                InsertLine %= Shift + Space | Ctrl + Plus;

            UsingContext(Contexts.Chrome);
                AddressBar %= Ctrl + L;
                Focus      %= AddressBar | F6;
                NewTab     %= Ctrl + T;
                CloseTab   %= Ctrl + W;
                Fullscreen %= F11;

            UsingContext(Contexts.Firefox);
                AddressBar %= Ctrl + L;
                NewTab     %= Ctrl + T;
                CloseTab   %= Ctrl + W;
                Fullscreen %= F11;

            UsingContext(Contexts.CommandPrompt);
                Menu               %= (SemanticAction)Mouse.ClickConsoleMenu;
                EndApplication     %= Menu | C;

                ClearScreen        %= DeleteLine | "cls" | Enter;

                Paste              %= Menu | E | P;

                NavPreviousWord    %= Ctrl + Left;
                NavNextWord        %= Ctrl + Right;

                DeleteEnd          %= Ctrl + End;
                DeleteHome         %= Ctrl + Home;
                DeleteLine         %= Escape;
                DeletePreviousWord %= Backspace;
                DeleteNextWord     %= Delete;

            UsingContext(Contexts.WindowsSsh);
                Menu               %= (SemanticAction)Mouse.ClickConsoleMenu;
                EndApplication     %= Menu | C;

                ClearScreen        %= Ctrl + L;

                Paste              %= Menu | E | P;

                NavPreviousWord    %= Alt + B;
                NavNextWord        %= Alt + F;

                DeleteEnd          %= Ctrl + K;
                DeleteHome         %= Ctrl + U;
                DeleteLine         %= Home + DeleteEnd;
                DeletePreviousWord %= Ctrl + W;
                DeleteNextWord     %= NavNextWord | DeletePreviousWord;

            UsingContext(Contexts.Git);
                Menu               %= (SemanticAction)Mouse.ClickConsoleMenu;
                EndApplication     %= Menu | C;

                ClearScreen        %= Ctrl + L;

                Paste              %= Shift + Insert;

                // These no longer appear correct, instead just use the default ctrl left and right.
                //NavPreviousWord    %= Alt + B;
                //NavNextWord        %= Alt + F;

                DeleteEnd          %= Ctrl + K;
                DeleteHome         %= Ctrl + U;
                DeleteLine         %= Home + DeleteEnd;
                DeletePreviousWord %= Ctrl + W;
                DeleteNextWord     %= NavNextWord | DeletePreviousWord;

            UsingContext(Contexts.Game);
                Debug              %= Alt + Tab | Ctrl + Quotes | Ctrl + Tab;

            UsingContext(Contexts.Putty);
                Paste              %= Shift + Insert;
        }
    }
}
