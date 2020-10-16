using System;
using System.Windows.Forms;

namespace PunchClock
{
    static class Program
    {
        private static PunchClockApplicationContext appContext;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                appContext = new PunchClockApplicationContext();

                Application.Run(appContext);
            }
            catch (Exception ex)
            {
                string errorMessage = "";
                Exception current = ex;
                while (current != null)
                {
                    errorMessage += current.Message + "\r\n";
                    current = current.InnerException;
                }
                System.Windows.Forms.MessageBox.Show($"{errorMessage}", "Error occured while executing punch clock", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }
        public static void Quit()
        {
            appContext.ExitThread();
        }
    }
}
