using FormFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //IForm main = new MainFormFactory();
            //Application.Run(main.createForm("DESKTOP-TPBIN8U", "StudentList", "", ""));

            MyFactory factory = new MyFactory();
            Application.Run(factory.ReturnForm(null, null, null, "login")); //factory method 
        }
    }
}
