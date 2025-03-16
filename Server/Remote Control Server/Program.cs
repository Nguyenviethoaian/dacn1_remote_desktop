using System;
using System.Windows.Forms;
using Remote_Control_Server;

namespace Remote_Control_Server
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            // Cấu hình cho ứng dụng Windows Forms
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Tạo và chạy ServerForm (giao diện của Server)
            Application.Run(new ServerForm());
        }
    }
}