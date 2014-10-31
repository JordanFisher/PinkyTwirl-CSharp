﻿using System;
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

        public Context(Context ParentContext) : this(null, null, ParentContext) { }

        public Context(string WindowName = null, string WindowDescription = null, Context ParentContext = null)
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

    //public class Context : Tuple<string, string>
    //{
    //    public string WindowName { get { return Item1; } }
    //    public string WindowDescription { get { return Item1; } }

    //    public Context
    //        ParentContext;

    //    public Context(Context ParentContext)
    //        : this("", "", ParentContext) { }

    //    public Context(string WindowName = "", string WindowDescription = "", Context ParentContext = null)
    //        : base(WindowName, WindowDescription)
    //    {
    //        this.ParentContext = ParentContext;

    //        Base.AllContexts.Add(this);
    //    }

    //    public bool MapsKey(Key key)
    //    {
    //        return true;
    //    }
    //}
}
