﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormFactory;

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

            IForm login = new LoginFormFactory();
            Application.Run(login.createForm("","","","")); //factory method
        }
    }
}
