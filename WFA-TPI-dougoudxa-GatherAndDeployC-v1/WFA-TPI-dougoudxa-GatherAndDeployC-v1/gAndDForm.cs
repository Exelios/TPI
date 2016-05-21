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
        #region Class attributes
        /// <summary>
        /// List of the selected targetHosts operator wants to synchronize
        /// </summary>
        public static List<TargetHost> targetHostList = new List<TargetHost>();

        /// <summary>
        /// Source is needed for file/directory transfers. Instanciated.
        /// </summary>
        private SourceHost currentSource = new SourceHost();

        /// <summary>
        /// Variable necessary to stop synchronization process.
        /// </summary>
        private static bool stopSynchronization = false;

        /// <summary>
        /// 
        /// </summary>
        private static bool stopUpdating = true;

        /// <summary>
        /// 
        /// </summary>
        private static Thread updateStatusThread = new Thread(updateTargets);

        /// <summary>
        /// 
        /// </summary>
        String offlineHostNames = null;

        #endregion

        #region Class methods

        /// <summary>
        /// Constructor of the gAndDForm class.
        /// </summary>
        public gAndDForm()
        {
            InitializeComponent();

            //Loop writing all the rooms in the establishment.
            for (int index = 0; index < NetworkConfig.roomAmount; ++index)
            {
                roomListBox.Items.Add(NetworkConfig.getRoom(index));
            }
            


            ////Testing purposes.
            //int dummyEntries = 17;
            //TargetHost tempHost;

            //for (int index = 0; index < dummyEntries; ++index)
            //{
            //    tempHost = new TargetHost("\\\\INF-N511-" + (index + 1).ToString("00"), "dummy test", index + 2);
                
            //    hostPanelContainer.Controls.Add(tempHost.getTargetHostPanel());
            //}
            //End of test
        }

        /// <summary>
        /// Event method when synchronizeButton is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void synchronizeButtonClick(object sender, EventArgs e)
        {
            offlineHostNames = null;    //empties the string.

            if (synchroniseButton.Text == "Synchronize")
            {
                int index = 0;
                
                synchroniseButton.Text = "Interrupt";
                //stopSynchronization = false;

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
                        synchronize(currentSource, target, existenceResults, index);
                        ++index;
                    }

                    System.Windows.Forms.MessageBox.Show("The following hosts were unreachable : " 
                        + Environment.NewLine + offlineHostNames, "Synchronization Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //stopSynchronization = true;
                synchroniseButton.Text = "Synchronize";
            }
            else
            {
                //stopSynchronization = true;
                synchroniseButton.Text = "Synchronize";
            }
        }
        /*---------------------------------------------------------------*/

        /// <summary>
        /// Method verifying existence of an object
        /// </summary>
        /// <param name="currentSourcePath">Object path</param>
        /// <returns>Array where indexes 0 and 1 are for files respectivily directories</returns>
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
        /// Method called from event on the synchronize button
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="target">Target path</param>
        /// <param name="existenceResults">Boolean array</param>
        /// <param name="index">Relitive to hosts indexes</param>
        private void synchronize(SourceHost source, TargetHost target, bool[] existenceResults, int index)
        {
            //Shares file / directory and asks if we want to overwrite an existing file / directory 
            //Works for connected hosts.

            String currentStatus = target.getHostStatus();

            if (currentStatus == NetworkConfig.connectionStatusArray[0]+ ' ')
            {
                source.share(sourcePathTextBox.Text,
                    targetHostList[index].getTargetHostName() + targetPathTextBox.Text.Substring(2),
                    existenceResults);
            }
            else
            {
                offlineHostNames += targetHostList[index].getTargetHostName() + "\n";
            }
        }
        /*------------------------------------------------------------------*/
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analyseButtonClick(object sender, EventArgs e)
        {
            if (updateStatusThread.ThreadState == ThreadState.Running)
            {
                stopUpdating = true;

                updateStatusThread.Abort();
            }

            //Empties the hostPanel
            hostPanelContainer.Controls.Clear();

            targetHostList.Clear();

            String tempRoom = Convert.ToString(roomListBox.SelectedItem).Substring(0,4);

            String tempHostName;

            for (int index = 0; index < NetworkConfig.machineAmount; ++index)
            {
                tempHostName = "\\\\INF-" + tempRoom + "-" + (index + 1).ToString("00");

                targetHostList.Add(new TargetHost(tempHostName, "Waiting for status...", index));

                hostPanelContainer.Controls.Add(targetHostList[index].getTargetHostPanel());
            }

            stopUpdating = false;

            updateStatusThread.Start();
        }
        /*-----------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        private static void updateTargets()
        {
            while (!stopUpdating)
            {
                foreach (TargetHost target in targetHostList)
                {
                    NetworkConfig.updateTargetStatus(target);
                }

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        ///// <param name="e"></param>
        private void gAndDFormFormClosing(object sender, FormClosingEventArgs e)
        {
            stopUpdating = true;

            if (updateStatusThread.ThreadState == ThreadState.Running)
                updateStatusThread.Abort();
        }
        /*-------------------------------------------------------------------*/

        #endregion


    }
}
