using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing;   
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using HookManager;

using WindowsInput;

namespace PinkyTwirl
{
    public class DictionaryHelper
    {
        public static Dictionary<T1, Dictionary<T2, K>> DeepCopy<T1, T2, K>(Dictionary<T1, Dictionary<T2, K>> source)
        {
            var d = new Dictionary<T1, Dictionary<T2, K>>();
            foreach (var pair1 in source)
            {
                d.Add(pair1.Key, new Dictionary<T2, K>());
                foreach (var pair2 in pair1.Value)
                    d[pair1.Key].Add(pair2.Key, pair2.Value);
            }

            return d;
        }
    }
}