using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    public partial class gAndDForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private List<TargetHost> targetHostList = new List<TargetHost>();

        //Source is needed for file/directory transfers. Instanciated.
        private SourceHost currentSource = new SourceHost();

        /// <summary>
        /// 
        /// </summary>
        private bool stopSynchronization = false;

        

        public gAndDForm()
        {
            InitializeComponent();

            //hardcoding - Transfer test
            TargetHost client1 = new TargetHost("\\\\WIN10-TPI-CLI-1", "ONLINE", 0);
            TargetHost client2 = new TargetHost("\\\\WIN10-TPI-CLI-2", "ONLINE", 1);

            targetHostList.Add(client1);
            targetHostList.Add(client2);

            hostPanelContainer.Controls.Add(targetHostList[0].getTargetHostPanel());
            hostPanelContainer.Controls.Add(targetHostList[1].getTargetHostPanel());



            //Testing purposes.
            int dummyEntries = 17;
            TargetHost tempHost;

            for (int index = 0; index < dummyEntries; ++index)
            {
                tempHost = new TargetHost("\\\\INF-N511-" + (index + 1).ToString("00"), "dummy test", index + 2);
                
                hostPanelContainer.Controls.Add(tempHost.getTargetHostPanel());
            }
            //End of test
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void synchroniseButtonClick(object sender, EventArgs e)
        {
            if (synchroniseButton.Text == "Synchronise")
            {
                int index = 0;
                
                synchroniseButton.Text = "Interrupt";
                stopSynchronization = false;

                //Checks if the source exists
                bool[] existenceResults = checkExistence(sourcePathTextBox.Text);

                //If the source doesn't exist.
                if (!existenceResults[0] && !existenceResults[1])
                {
                    System.Windows.Forms.MessageBox.Show("No such file or directory.", "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    //Transfers file/directory to every host chosen.
                    foreach (TargetHost target in targetHostList)
                    {
                        synchronise(currentSource, target, existenceResults, index);
                        ++index;
                    }
                }

                stopSynchronization = true;
                synchroniseButton.Text = "Synchronise";
            }
            else
            {
                stopSynchronization = true;
                synchroniseButton.Text = "Synchronise";
            }
        }
        /*---------------------------------------------------------------*/

            /// <summary>
            /// 
            /// </summary>
            /// <param name="currentSourcePath"></param>
            /// <returns></returns>
        private bool[] checkExistence(String currentSourcePath)
        {
            bool[] results = new bool[2];

            //First, check if what we want to transfer is a file or a directory.
            results[0] = false;
            results[1] = false;

            //Checks if the file exists.
            if (File.Exists(currentSourcePath))
            {
                results[0] = true;
            }

            //Checks if the directory exists.
            if (Directory.Exists(currentSourcePath))
            {
                results[1] = true;
            }

            return results;
        }
        /*---------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="existenceResults"></param>
        /// <param name="index"></param>
        private void synchronise(SourceHost source, TargetHost target, bool[] existenceResults, int index)
        {
            //Test sharing and impersonating
            source.share(sourcePathTextBox.Text, 
                targetHostList[index].getTargetHostName() + targetPathTextBox.Text.Substring(2), 
                existenceResults);
        }
        /*------------------------------------------------------------------*/
        
        /// <summary>
        /// 
        /// </summary>
        private void updateTargetStatus()
        {
            long length = new FileInfo(sourcePathTextBox.Text).Length;

            long tempSize;

            while (!stopSynchronization)
            {
                foreach(TargetHost target in targetHostList)
                {
                    tempSize = new FileInfo(target.getTargetHostName() + targetPathTextBox.Text.Substring(2)).Length;

                    target.setHostStatus((length / tempSize) * 100 + " % tranferred.");
                }
            }
        }
        /*-------------------------------------------------------------------------------*/
    }
}
