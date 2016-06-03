using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace InvokedelegateExample
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Thread destined to change the label's text
        /// </summary>
        private Thread changeTextThread;

        /// <summary>
        /// Delegate responsible for allowing a crossThread modification.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        private delegate void crossthreadTextChange(Control control, String text);

        /// <summary>
        /// constructor of the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method invoked to change the label text
        /// </summary>
        /// <param name="control">The label we want to modify</param>
        /// <param name="newText">The new text</param>
        private static void updateText(Control control, String newText)
        {
            //Must we invoke a delegate or not?
            if (!control.InvokeRequired)
            {
                //The action we would normally like to perform
                control.Text = newText;
            }
            else
            {
                //Since we cannot perform our action we must invoke a delegate to perform it instead
                //The delegate calls this same method.
                control.Invoke(new crossthreadTextChange(updateText), new object[] { control, newText });
            }
        }

        /// <summary>
        /// Start method used when thread is defined
        /// </summary>
        private void threadStart()
        {
            if (label1.Text != "New text")
            {
                updateText(label1, "New text");
            }
            else
            {
                updateText(label1, "Other text");
            }
        }

        /// <summary>
        /// Event handler setting all in motion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            changeTextThread = new Thread(threadStart);

            changeTextThread.Start();
        }
    }
}
