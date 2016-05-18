using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopySimple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.Copy(@"C:\Users\Win10Rsync\Desktop\elf2.jpg", @"\\WIN10-TPI-CLI-1\Users\Public\toto.jpg");
        }
    }
}
