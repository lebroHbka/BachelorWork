﻿using System;
using System.Windows.Forms;

namespace TestGraph
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

            var f = new FormWithResults();

            Application.Run(f);
        }
    }
}
