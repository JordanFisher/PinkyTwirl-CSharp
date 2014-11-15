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
    public class Functions : Base
    {
        public static void StartAltShiftTab()
        {
            (LMenu + LShift + Tab).DoDown();
            StartCheckingToEndCtrlTab(Meta);
        }

        public static void StartAltTab()
        {
            (LShift).DoUp();
            (LMenu + Tab).DoDown();
            StartCheckingToEndCtrlTab(Meta);
        }

        public static void StartCtrlTab_Up()
        {
            Key.LControl.DoDown();
            (Tab).Do();
            StartCheckingToEndCtrlTab(Key.D2);
        }

        public static void StartCtrlTab_Down()
        {
            Key.LControl.DoDown();
            (Shift + Tab).Do();
            StartCheckingToEndCtrlTab(Key.D2);
        }

        public static void StartCheckingToEndCtrlTab(Key Key)
        {
            if (CheckingToEndCtrlTab) return;
            KeyToEndOn = Key;
            IoHooks.KeyUp += CheckToEndCtrlTab;
            CheckingToEndCtrlTab = true;
        }


        static bool CheckingToEndCtrlTab = false;
        static Key KeyToEndOn;
        static void CheckToEndCtrlTab(object sender, KeyEventArgs e)
        {
            if (App.Skip) return;

            if (e.KeyCode == KeyToEndOn)
            {
                EndCtrlTab();
            }
        }

        public static void EndCtrlTab()
        {
            IoHooks.KeyUp -= CheckToEndCtrlTab;
            CheckingToEndCtrlTab = false;

            App.Skip = true;
            //(LControl + LShift + LMenu + Tab).DoUp();
            ResetFunctionKeys();
            App.Skip = false;
        }

        public static void ResetFunctionKeys()
        {
            (LControl + LShift + LMenu + Tab).DoUp();

            (
                LControl | RControl |
                LShift | RShift |
                LMenu | RMenu |
                D1 | D2 | D3 | D4 |
                Tab | Space | Delete | Backspace | Escape |
                LButton | RButton |
                new SemanticAction(Meta)
            ).DoUp();

            PinkyTwirlApp.App.Reset();
        }
        
        public static void StartGit2()
        {
            git_dir = null;
            Thread thread = new Thread(() => {
                GetCurExplorerDir();

                if (git_dir != null)
                {
                    git_dir = Directory.GetParent(git_dir).FullName;

                    StartGitProcess(git_dir);
                    ResetFunctionKeys();
                }
            });
            thread.Start();
        }

        public static void StartGit()
        {
            git_dir = null;
            Thread thread = new Thread(GetCurExplorerDir);
            thread.Start();
            thread.Join();

            if (git_dir != null)
            {
                git_dir = Directory.GetParent(git_dir).FullName;

                StartGitProcess(git_dir);
            }
        }

        public static void StartGitProcess(string dir)
        {
            Process scriptProc = new Process();
            scriptProc.StartInfo.FileName = @"cscript";
            scriptProc.StartInfo.WorkingDirectory = dir;
            scriptProc.StartInfo.Arguments = "//B //Nologo \"C:/Program Files (x86)/Git/Git Bash.vbs\"";
            scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            scriptProc.Start();
        }

        static string git_dir = null;
        static void GetCurExplorerDir()
        {
            IntPtr MyHwnd = WindowFunctions.GetForegroundWindow();
            var t = Type.GetTypeFromProgID("Shell.Application");
            dynamic o = Activator.CreateInstance(t);
            try
            {
                var ws = o.Windows();
                for (int i = 0; i < ws.Count; i++)
                {
                    var ie = ws.Item(i);
                    if (ie == null || ie.hwnd != (long)MyHwnd) continue;
                    var path = System.IO.Path.GetFileName((string)ie.FullName);
                    if (path.ToLower() == "explorer.exe")
                    {
                        var explorepath = ie.document.focuseditem.path;
                        git_dir = explorepath;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Exception!" + e.ToString());
            }
            finally
            {
                Marshal.FinalReleaseComObject(o);
            }
        }

        public static void SaveAsPNG()
        {
            App.Skip = true;
            (Shift + Tab | Shift + Tab | Down | Space).Do();
            System.Threading.Thread.Sleep(150);
            (3 * Tab | 8 * P | Shift + Tab).Do();
            App.Skip = false;
        }
    }
}