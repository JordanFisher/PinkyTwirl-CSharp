using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HookManager;
using WindowsInput;

namespace PinkyTwirl
{
    public class Context
    {
        public string
            WindowName, WindowDescription;

        public Context
            ParentContext;

        public Context(Context ParentContext) : this("", "", ParentContext) { }

        public Context(string WindowName = "", string WindowDescription = "", Context ParentContext = null)
        {
            this.WindowName        = WindowName;
            this.WindowDescription = WindowDescription;
            this.ParentContext     = ParentContext;

            Base.AllContexts.Add(this);
        }

        public bool MapsKey(Key key)
        {
            return true;
        }
    }
}
