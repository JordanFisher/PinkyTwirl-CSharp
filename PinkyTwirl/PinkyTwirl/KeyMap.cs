using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HookManager;
using WindowsInput;

namespace PinkyTwirl
{
    public class KeyMapOrAction
    {
        public KeyMap MyMap;
        public SemanticAction MyAction;

        public static KeyMapOrAction MakeEmptyMap()
        {
            var NewMap = new KeyMapOrAction();
            NewMap.MyMap = new KeyMap();
            
            return NewMap;
        }

        public KeyMapOrAction()
        { 
        }

        public KeyMapOrAction(KeyMapOrAction source) : this()
        {
            if (source.MyMap != null)
            {
                MyMap = new KeyMap(source.MyMap);
            }

            MyAction = source.MyAction;
        }
    }

    public class KeyMap : Dictionary<Key, KeyMapOrAction>
    {
        public KeyMap() : base()
        {
        }

        public KeyMap(KeyMap source) : this()
        {
            Copy(source);
        }

        public void Copy(KeyMap source)
        {
            foreach (var pair in source)
            {
                this.Add(pair.Key, new KeyMapOrAction(pair.Value));
            }
        }
    }
}
