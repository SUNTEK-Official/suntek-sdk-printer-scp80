using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POSdllDemo
{
    static class Program
    {
        /// <summary>

        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /**
              * 当前用户是管理员的时候，直接启动应用程序
              * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
              */
 // Windows
             System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
             System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

             if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
             {

                 //Application.Run(new Form1());
             }
             else
             {

                 System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                 startInfo.UseShellExecute = true;
                 startInfo.WorkingDirectory = Environment.CurrentDirectory;
                 startInfo.FileName = Application.ExecutablePath;

                 startInfo.Verb = "runas";
                 try
                 {
                     System.Diagnostics.Process.Start(startInfo);
                 }
                 catch
                 {
                     return;
                 }

                 Application.Exit();
             }
        }
    }
}
