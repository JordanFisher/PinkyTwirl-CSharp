using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HookManager;
using WindowsInput;

namespace PinkyTwirl
{
    public class Mappings : Semantics
    {
        public static void Inititalize()
        {
            UsingContext(Contexts.Default);
                Chord(Meta);
                    Key.I |= NavUp;
                    Key.J |= NavLeft;
                    Key.K |= NavDown;
                    Key.L |= NavRight;                                                                          

                    Key.U |= NavHome;
                    Key.O |= NavEnd;

                    Key.Y |= NavBigUp;
                    Key.H |= NavBigDown;

                    Key.M |= NavNextWord;
                    Key.N |= NavPreviousWord;

                    Key.P         |= NavTop;
                    Key.Semicolon |= NavBottom;

                    Key.Enter |= InsertLine;

                    Key.D9 |= NavPageUp;
                    Key.D0 |= NavPageDown;

                    Key.W |= Paste;
                    Key.E |= Undo;
                    Key.R |= Redo;

                    Key.G |= Focus;
                    
                    Key.A |= Search;
                    Key.Q |= Replace;
                    Key.Z |= SearchAll;

                    Key.T |= Fullscreen;

                    Key.Tab |= ProgramTab;
                    Key.F4  |= EndApplication;

                Chord(Key.D4);
                    Key.I |= SelectUp;
                    Key.J |= SelectLeft;
                    Key.K |= SelectDown;
                    Key.L |= SelectRight;

                    Key.U |= SelectHome;
                    Key.O |= SelectEnd;

                    Key.Y |= SelectBigUp;
                    Key.H |= SelectBigDown;

                    Key.M |= SelectNextWord;
                    Key.N |= SelectPreviousWord;

                    Key.P         |= SelectTop;
                    Key.Semicolon |= SelectBottom;

                    Key.D9 |= SelectPageUp;
                    Key.D0 |= SelectPageDown;
                                
                    Key.R |= Copy;
                    Key.T |= Paste;
                    Key.F |= Cut;
            
                Chord(Key.D3);
                    Key.I |= DeleteUp;
                    Key.J |= DeleteLeft;
                    Key.K |= DeleteDown;
                    Key.L |= DeleteRight;

                    Key.U |= DeleteHome;
                    Key.O |= DeleteEnd;

                    Key.Y |= DeleteBigUp;
                    Key.H |= DeleteBigDown;

                    Key.M |= DeleteNextWord;
                    Key.N |= DeletePreviousWord;

                    Key.P         |= DeleteTop;
                    Key.Semicolon |= DeleteBottom;

                    Key.D9 |= DeletePageUp;
                    Key.D0 |= DeletePageDown;

                Chord(Key.D2);
                    Key.J |= FileTabLeft;
                    Key.K |= FileTabRight;
                    Key.P |= CloseAllBut;
                    Key.O |= Close;
                    Key.I |= Save;

                Chord(Meta, Key.Space);
                    Key.G |= (Action)StartGit;

                Chord(Key.CapsLock);
                    Key.H            |= "{}" | Left;                                          // {}
                    Key.J            |= "()" | Left;                                          // ()
                    Key.K            |= "{}" | Left | Enter | Enter | Up | Tab;               // {\n\n}
                    Key.L            |= "[]" | Left;                                          // []
                    Key.Semicolon    |= "<>" | Left;                                          // <>
                    Key.Quotes       |= Quotes | Quotes | Left;                               // ""

                    Key.Y            |= Cut |  "{}" | Left                           | Paste; // {..}
                    Key.U            |= Cut |  "()" | Left                           | Paste; // (..)
                    Key.I            |= Cut |  "{}" | Left | Enter| Enter | Up | Tab | Paste; // {\n..\n}
                    Key.O            |= Cut |  "[]" | Left                           | Paste; // [..]
                    Key.P            |= Cut |  "<>" | Left                           | Paste; // <..>
                    Key.OpenBrackets |= Cut |  Quotes | Quotes | Left                | Paste; // ".."

            // Simple map, used for applications where you don"t want anything fancy.
            // Because we are overriding some default Alt behavior, we still need to implement that basic functionality (Alt-Tab and Alt-F4)
            UsingContext(Contexts.Simple, CopyParentContext:true);
                Chord(Meta);
                    Key.F4 |= EndApplication;

            // IDE map
            UsingContext(Contexts.Ide, CopyParentContext:true);
                Chord(Meta);
                    Key.Comma  |= Comment;
                    Key.Period |= Uncomment;

                    Key.B |= Debug;
                    Key.G |= CollapseScope;
                    Key.F |= GotoDefinition;
                    Key.D |= FindAllReferences;
                    Key.S |= Rename;

                Chord(Meta, Key.Space);
                    Key.H |= (Action)ClosePanel;

                    Key.J |= ViewProjectExplorer;
                    Key.K |= ViewErrorList;
                    Key.U |= ViewOutput;
                    Key.I |= ViewSymbols;
                    Key.O |= ViewFindResults;
                    Key.N |= ViewCallStack;
                    Key.M |= ViewInteractive;
                    Key.P |= ViewClasses;
                    Key.Y |= ViewConfigurationSelector;

            // Command prompt. This overrides the tedious Alt-Space e p method for pasting with the default PinkyTwirl paste command Alt + W
            UsingContext(Contexts.CommandPrompt, CopyParentContext:true);
                Chord(Meta);
                    // Context menu access requires pressing Alt + Space, which doesn't seem to work.

            // Browsers
            UsingContext(Contexts.Browser, CopyParentContext:true);
                Chord(Meta);
                    Key.F |= AddressBar;
                
                Chord(Key.D2);
                    Key.O |= CloseTab;
                    Key.I |= NewTab;

            // Git map
            UsingContext(Contexts.Git, CopyParentContext:true);
                Chord(Meta);
                    Key.F |= (Action)Commit;
                    Key.C |= DeleteLine + "git config --global user.email \"jordan@pwnee.com\"";
                    Key.V |= DeleteLine + "git config --global user.email \"jordan.efisher@gmail.com\"";

            // Photoshop mapu
            UsingContext(Contexts.Photoshop, CopyParentContext:true);
                Chord(Key.D1);
                    //Key.P |= SaveAsPNG;

            // Game map
            UsingContext(Contexts.Game, CopyParentContext:true);
                Chord(Meta);
                    Key.B |= Debug;
        }
    }
}
