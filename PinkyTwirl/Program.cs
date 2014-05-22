using System;
using System.Windows.Forms;

namespace PinkyTwirl
{
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new PinkyTwirlForm());
            }
            catch (Exception e)
            {
            }
        }
    }
}