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
            // Nothing
            Nothing = new SemanticAction(),
            
            // Text navigation
            NavLeft         = Left,
            NavRight        = Right,
            NavUp           = Up,
            NavDown         = Down,
            NavHome         = Home,
            NavEnd          = End,
            NavPageUp       = PageUp,
            NavPageDown     = PageDown,
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
            DeleteLeft         = Delete,
            DeleteRight        = Backspace,
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
            Find           = Ctrl + F,
            FindAndReplace = Ctrl + H,

            // State
            Undo           = Ctrl + Z,
            Redo           = Ctrl + Y,

            // Navigate
            ProgramTab     = (Action)StartAltTab,
            FileTabLeft    = (Action)StartCtrlTab_Up,
            FileTabRight   = (Action)StartCtrlTab_Down,

            // File actions
            Save           = Ctrl + S,
            SaveAs         = Ctrl + Alt + S,
            SaveAll        = Ctrl + Shift + S,
            Close          = Ctrl + Semicolon,
            CloseAllBut    = Ctrl + Shift + Semicolon,

            // Brower
            AddressBar     = Ctrl + L,
            NewTab         = Ctrl + T,
            CloseTab       = Ctrl + W,

            // Application
            Fullscreen     = F11,
            EndApplication = Alt + F4,

            // Game

            // IDE
            Debug                     = Nothing,
            Comment                   = Nothing,
            Uncomment                 = Nothing,
            Rename                    = Nothing,
            FindAllReferences         = Nothing,
            GotoDefinition            = Nothing,
            CollapseScope             = Nothing,
            ViewProjectExplorer       = Nothing,
            ViewErrorList             = Nothing,
            ViewOutput                = Nothing,
            ViewSymbols               = Nothing,
            ViewFindResults           = Nothing,
            ViewCallStack             = Nothing,
            ViewInteractive           = Nothing,
            ViewClasses               = Nothing,
            ViewConfigurationSelector = Nothing;

        public static void Initialize()
        {
            UsingContext(Contexts.VisualStudio);
                Fullscreen                %= Shift + Alt + Enter;
                Close                     %= Ctrl + W;

                Debug                     %= Ctrl + Backslash | "'" | Ctrl + Tab;
                Comment                   %= Ctrl + '[';
                Uncomment                 %= Ctrl + ']';
                Rename                    %= Ctrl + D1;
                FindAllReferences         %= OpenVsPanelAction("^.");
                GotoDefinition            %= Ctrl + Comma;
                CollapseScope             %= Ctrl + M | M;
                ViewProjectExplorer       %= OpenVsPanelAction("^8");
                ViewErrorList             %= OpenVsPanelAction("^we");
                ViewOutput                %= OpenVsPanelAction("^wo");
                ViewSymbols               %= OpenVsPanelAction("^wq");
                ViewFindResults           %= OpenVsPanelAction("%f");
                ViewCallStack             %= OpenVsPanelAction("^dc");
                ViewInteractive           %= OpenVsPanelAction("^di");
                ViewClasses               %= OpenVsPanelAction("^99");
                ViewConfigurationSelector %= OpenVsPanelAction("^66");

            UsingContext(Contexts.Excel);
                DeleteLine %= Shift + Space | Ctrl + Minus;
                InsertLine %= Shift + Space | Ctrl + Plus;

            UsingContext(Contexts.Chrome);
                AddressBar %= Ctrl + L;
                NewTab     %= Ctrl + T;
                CloseTab   %= Ctrl + W;
                Fullscreen %= F11;

            UsingContext(Contexts.Firefox);
                AddressBar %= Ctrl + L;
                NewTab     %= Ctrl + T;
                CloseTab   %= Ctrl + W;
                Fullscreen %= F11;

            UsingContext(Contexts.Git);
                NavPreviousWord    %= Alt + B;
                NavNextWord        %= Alt + F;

                DeleteEnd          %= Ctrl + K;
                DeleteHome         %= Ctrl + U;
                DeleteLine         %= Home + DeleteEnd;
                DeletePreviousWord %= Ctrl + W;
        }
    }
}
