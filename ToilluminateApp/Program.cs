using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToilluminateApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 多重起動防止処理
            Mutex mutex = new Mutex(false, Application.ProductName);

            if (!mutex.WaitOne(0, false))
            {
                GC.KeepAlive(mutex);
                mutex.Close();
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // メイン画面起動
            MainForm mainFormInstance = new MainForm();
            Application.Run(mainFormInstance);

            mutex.ReleaseMutex();
        }
    }
}
