using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HookManager;
using WindowsInput;

namespace PinkyTwirl
{
    public static class Contexts
    {
        public static Context
            Default           = new Context(),
            Simple            = new Context(Default),

            Browser           = new Context(Default),
            Ide               = new Context(Default),
            Game              = new Context(Simple),

            CommandPrompt     = new Context("Command Prompt",      "",           Default),

            Notepad           = new Context("Notepad",               "",         Default),
            NotepadPlusPlus   = new Context(null,                  "Notepad++",  Ide),
            MinGW             = new Context("MINGW",               "",           CommandPrompt),
            Git               = MinGW,
            Ubuntu            = new Context("ubuntu",              null,         Git),
            IPython           = new Context("IPython",             null,         Git),
            LEd               = new Context("LEd",                 null,         Ide),
            Excel             = new Context("Excel",               null,         Default),
            Photoshop         = new Context("Photoshop",           null,         Default),

            VisualStudio      = new Context("Microsoft Visual", null, Ide),
            VSCode            = new Context(null, "Visual Studio Code",  Ide),
            VSCode            = new Context("Visual Studio Code", null,  Ide),
            VSCode            = new Context("Visual Studio Code", "Visual Studio Code", Ide),

            ExeShell          = new Context(".exe",                null,         CommandPrompt),
            Putty             = new Context("PuTTY",               "",           Git),
            Putty2            = new Context("@",                   "",           Git),

            Chrome            = new Context("Chrome",              null,         Browser),
            Firefox           = new Context(null,                  "Firefox",    Browser);
    }
}
