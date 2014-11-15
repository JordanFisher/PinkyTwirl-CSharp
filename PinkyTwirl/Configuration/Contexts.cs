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
            Simple            = new Context(),
            Default           = new Context(),

            Browser           = new Context(Default),
            Ide               = new Context(Default),
            Game              = new Context(Simple),

            CloudberryKingdom = new Context("Cloudberry Kingdom ", null,         Game),
            Dota              = new Context("DOTA",                null,         Game),
            Terracotta        = new Context("Terracotta",          "Terracotta", Game),

            Chrome            = new Context(null,                  "Chrome",     Browser),
            Firefox           = new Context(null,                  "Firefox",    Browser),
            Notepad           = new Context("Notepad",             "",           Default),
            NotepadPlusPlus   = new Context(null,                  "Notepad++",  Ide),
            WingIde           = new Context("Wing IDE",            null,         Ide),
            CommandPrompt     = new Context("Command Prompt",      "",         Default),
            MinGW             = new Context("MINGW",               "",         CommandPrompt),
            Git               = MinGW,
            IPython           = new Context("IPython",             null,         CommandPrompt),
            LEd               = new Context("LEd",                 null,         Ide),
            Excel             = new Context("Excel",               null,         Default),
            Photoshop         = new Context("Photoshop",           null,         Default),
            VisualStudio      = new Context("Microsoft Visual",    null,         Ide);
    }
}
