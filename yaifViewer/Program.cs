using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace yaifViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static byte[] inputFile;
        [STAThread]
        static void Main(String[] args)
        {
            if (args.Length == 0)
            {

                System.Environment.Exit(0);
            }
            else
            {
                inputFile = File.ReadAllBytes(args[0]);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImageViewer());
       
        }
    }
}
