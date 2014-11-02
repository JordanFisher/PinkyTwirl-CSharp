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

            // Brower
            NewTab         = Ctrl + T,
            CloseTab       = Ctrl + W,

            // Application
            Fullscreen     = None + F11,
            EndApplication = Alt + F4,

            // Game

            // git
            Commit = DeleteLine | "git add . ; git commit -a",

            // IDE
            Debug                     = 0,
            Comment                   = 0,
            Uncomment                 = 0,
            Rename                    = 0,
            FindAllReferences         = 0,
            GotoDefinition            = 0,
            CollapseScope             = 0,
            ViewProjectExplorer       = 0,
            ViewErrorList             = 0,
            ViewOutput                = 0,
            ViewSymbols               = 0,
            ViewFindResults           = 0,
            ViewCallStack             = 0,
            ViewInteractive           = 0,
            ViewClasses               = 0,
            ViewConfigurationSelector = 0;

        public static void Initialize()
        {
            UsingContext(Contexts.VisualStudio);
                Fullscreen                %= Shift + Alt + Enter;
                Close                     %= Ctrl + Semicolon;

                Debug                     %= Ctrl + Backslash | "'" | Ctrl + Tab;
                Comment                   %= Ctrl + '[';
                Uncomment                 %= Ctrl + ']';
                Rename                    %= Ctrl + D1;
                SearchAll                 %= OpenVsPanelAction(Shift + Ctrl + F | Alt + F);
                FindAllReferences         %= OpenVsPanelAction(Ctrl + Period);
                GotoDefinition            %= Ctrl + Comma;
                CollapseScope             %= Ctrl + M | M;
                ViewProjectExplorer       %= OpenVsPanelAction(Ctrl + D8);
                ViewErrorList             %= OpenVsPanelAction(Ctrl + W | E);
                ViewOutput                %= OpenVsPanelAction(Ctrl + W | O);
                ViewSymbols               %= OpenVsPanelAction(Ctrl + W | Q);
                ViewFindResults           %= OpenVsPanelAction(Alt + F);
                ViewCallStack             %= OpenVsPanelAction(Ctrl + D | C);
                ViewInteractive           %= OpenVsPanelAction(Ctrl + D | I);
                ViewClasses               %= OpenVsPanelAction(Ctrl + D9 | D9);
                ViewConfigurationSelector %= OpenVsPanelAction(Ctrl + D6 | D6);

            UsingContext(Contexts.Excel);
                DeleteLine %= Shift + Space | Ctrl + Minus;
                InsertLine %= Shift + Space | Ctrl + Plus;

            UsingContext(Contexts.Chrome);
                AddressBar %= Ctrl + L;
                Focus      %= AddressBar | F6 | F6;
                NewTab     %= Ctrl + T;
                CloseTab   %= Ctrl + W;
                Fullscreen %= F11;

            UsingContext(Contexts.Firefox);
                AddressBar %= Ctrl + L;
                NewTab     %= Ctrl + T;
                CloseTab   %= Ctrl + W;
                Fullscreen %= F11;

            UsingContext(Contexts.Git);
                Menu               %= (SemanticAction)Mouse.ClickConsoleMenu;
                EndApplication     %= Menu | C;

                Paste              %= Menu | E | P;

                NavPreviousWord    %= Alt + B;
                NavNextWord        %= Alt + F;

                DeleteEnd          %= Ctrl + K;
                DeleteHome         %= Ctrl + U;
                DeleteLine         %= Home + DeleteEnd;
                DeletePreviousWord %= Ctrl + W;
                DeleteNextWord     %= NavNextWord | DeletePreviousWord;
        }
    }
}
