// Copyright © 2016 ASM-SW
//asm-sw@outlook.com  https://github.com/asm-sw
using System;
using System.Windows.Forms;

namespace DonorStatement
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
            Application.Run(new FormMain());
        }
    }
}
