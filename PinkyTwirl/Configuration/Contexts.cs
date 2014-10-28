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
            Simple            = new Context(),
            Browser           = new Context(Default),
            Ide               = new Context(Default),
            Game              = new Context(Simple),

            Chrome            = new Context("Chrome",              null,         Browser),
            Firefox           = new Context("Firefox",             null,         Browser),
            VisualStudio      = new Context("Microsoft Visual",    null,         Ide),
            NotepadPlusPlus   = new Context("Notepad++",           null,         Ide),
            Notepad           = new Context("Notepad",             null,         Default),
            WingIde           = new Context("Wing IDE",            null,         Ide),
            CommandPrompt     = new Context("Command Prompt",      null,         Default),
            MinGW             = new Context("MINGW",               null,         CommandPrompt),
            Git               = MinGW,
            IPython           = new Context("IPython",             null,         CommandPrompt),
            LEd               = new Context("LEd",                 null,         Ide),
            Excel             = new Context("Excel",               null,         Default),
            Photoshop         = new Context("Photoshop",           null,         Default),

            CloudberryKingdom = new Context("Cloudberry Kingdom ", null,         Game),
            Dota              = new Context("DOTA",                null,         Game),
            Terracotta        = new Context("Terracotta",          "Terracotta", Game);
    }
}
