using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HookManager;
using WindowsInput;

namespace PinkyTwirl
{
    public class Base : SemanticAction
    {
        public static PinkyTwirlApp App = new PinkyTwirlApp();

        public static Key Meta = Key.F15;
        public static Context CurrentContext = null;
        public static Key CurrentChord1 = Key.None, CurrentChord2 = Key.None;

        public static List<Context> AllContexts = new List<Context>();
        public static Dictionary<Context, KeyMap> Map = new Dictionary<Context, KeyMap>();

        public static Context ActiveContext
        {
            get
            {
                var Info = WindowFunctions.GetActiveWindowTitle();
                var ActiveName = Info.Item1;
                var ActiveDescription = Info.Item2;

                // Try to match on description only first
                if (ActiveDescription != null)
                for (int i = AllContexts.Count - 1; i >= 0; i--)
                {
                    var context = AllContexts[i];

                    if (context.WindowDescription == null || context.WindowDescription.Length == 0) continue;

                    if (ActiveDescription.Contains(context.WindowDescription))
                    {
                        return context;
                    }
                }

                // Otherwise try to match on both name and description
                for (int i = AllContexts.Count - 1; i >= 0; i--)
                {
                    var context = AllContexts[i];

                    if (context.WindowName == null && context.WindowDescription == null) continue;

                    if ((context.WindowName        == null || context.WindowName.Length        == 0 || context.WindowName.Length        != 0 && ActiveName.Contains(context.WindowName)) &&
                        (context.WindowDescription == null || context.WindowDescription.Length == 0 || context.WindowDescription.Length != 0 && ActiveDescription.Contains(context.WindowDescription)))
                    {
                         return context;
                    }
                }

                return Contexts.Default;
            }
        }

        public static KeyMap ActiveContextMap
        {
            get
            {
                return GetContextMap(ActiveContext);
            }
        }

        public static KeyMap GetContextMap(Context context)
        {
            if (context == null) return null;

            if (Map.ContainsKey(context))
            {
                return Map[context];
            }
            else
            {
                return GetContextMap(context.ParentContext);
            }
        }

        public static void UsingContext(Context context, bool CopyParentContext = false)
        { 
            CurrentContext = context;

            if (CopyParentContext)
            {
                if (!Map.ContainsKey(context))
                {
                    Map.Add(context, new KeyMap());
                }

                if (context.ParentContext != null)
                {
                    Map[context].Copy(Map[context.ParentContext]);
                }
            }
        }

        public static void Chord(Key key)
        {
            CurrentChord1 = key;
            CurrentChord2 = Key.None;
        }

        public static void Chord(Key key1, Key key2)
        {
            CurrentChord1 = key1;
            CurrentChord2 = key2;
        }

        public static void SetKeyMapping(Key k, SemanticAction A)
        {
            if (CurrentContext == null) return;

            if (!Map.ContainsKey(CurrentContext))
            {
                Map.Add(CurrentContext, new KeyMap());
            }
            var map = Map[CurrentContext];

            if (!map.ContainsKey(CurrentChord1))
            {
                map.Add(CurrentChord1, KeyMapOrAction.MakeEmptyMap());
            }
            map = map[CurrentChord1].MyMap;

            if (CurrentChord2 != Key.None)
            {
                if (!map.ContainsKey(CurrentChord2))
                {
                    map.Add(CurrentChord2, KeyMapOrAction.MakeEmptyMap());
                }

                map = map[CurrentChord2].MyMap;
            }

            if (!map.ContainsKey(k))
            {
                map.Add(k, new KeyMapOrAction());
            }

            map[k].MyAction = A;
        }
    }
}
