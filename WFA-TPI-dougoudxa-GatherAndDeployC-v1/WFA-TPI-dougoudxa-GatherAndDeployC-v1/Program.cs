/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Main thread of the application


using System;
using System.Windows.Forms;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    static class Program
    {
        public static gAndDForm Form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form = new gAndDForm();
            Application.Run(Form);
        }
    }
}
