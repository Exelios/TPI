using System;
using System.Collections.Generic;
using System.Threading;
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
        /// List of update threads updating an individual host.
        /// </summary>
        public static List<Thread> updateThreadList = new List<Thread>();

        /// <summary>
        /// Source is needed for file/directory transfers. Instanciated.
        /// </summary>
        private SourceHost currentSource = new SourceHost();

        /// <summary>
        /// Variable necessary to stop synchronization process.
        /// </summary>
        private static bool stopSynchronization = false;

        /// <summary>
        /// Variable allowing or stopping the status update of all the hosts.
        /// </summary>
        private volatile static bool stopUpdating = true;

        /// <summary>
        /// String containing the names of all the offline hosts.
        /// </summary>
        private static String offlineHostNames = null;

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
                roomComboBox.Items.Add(NetworkConfig.getRoom(index));
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

            if (synchronizeButton.Text == "Synchronize")
            {
                int index = 0;
                
                synchronizeButton.Text = "Interrupt";
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

                    //Will contain error message in the future.
                    //System.Windows.Forms.MessageBox.Show("The following hosts were unreachable : " 
                    //    + Environment.NewLine + offlineHostNames, "Synchronization Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //stopSynchronization = true;
                synchronizeButton.Text = "Synchronize";
            }
            else
            {
                //stopSynchronization = true;
                synchronizeButton.Text = "Synchronize";
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

            if (target.getHostStatus() == NetworkConfig.connectionStatusArray[0] + ' ' && target.getSyncCheckBoxState())
            {
                source.share(sourcePathTextBox.Text,
                    targetHostList[index].getTargetHostName() + targetPathTextBox.Text.Substring(2),
                    existenceResults);
            }
            else
            {
                offlineHostNames = "No online hosts selected";
            }
        }
        /*------------------------------------------------------------------*/
        

        /// <summary>
        /// Method handling the "analyzeButton click" event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analyzeButtonClick(object sender, EventArgs e)
        {
            //Empties the hostPanel
            hostPanelContainer.Controls.Clear();

            targetHostList.Clear();

            String tempRoom;

            //To only get the first four charactors
            if (Convert.ToString(roomComboBox.SelectedItem).Length > 3)
            {
                tempRoom = Convert.ToString(roomComboBox.SelectedItem).Substring(0, 4);
            }
            else
            {
                tempRoom = Convert.ToString(roomComboBox.SelectedItem);
            }

            String tempHostName;

            for (int index = 0; index < NetworkConfig.MACHINE_AMOUNT; index++)
            {
                tempHostName = "\\\\INF-" + tempRoom + "-" + (index + 1).ToString("00");

                targetHostList.Add(new TargetHost(tempHostName, "Pinging...", index));

                hostPanelContainer.Controls.Add(targetHostList[index].getTargetHostPanel());
            }

            startUpdateThreads();

            //for(int index = 0; index < NetworkConfig.machineAmount; ++index)
            //{
            //    updateThreadList[index].Start();
            //}
        }
        /*-----------------------------------------------------------------------------*/

        /// <summary>
        /// Method starting the status update threads. Used for parameterized start.
        /// http://stackoverflow.com/questions/1195896/threadstart-with-parameters
        /// </summary>
        private static void updateTargets(object index)
        {
            //Updating the status
            NetworkConfig.updateTargetStatus(targetHostList[(int)index]);

            //Updating the checkbox
            targetHostList[(int)index].setSyncCheckBoxState(targetHostList[(int)index].getSyncCheckBox());
        }
        /*-----------------------------------------------------------------------------*/

        /// <summary>
        /// Method starting the update threads.
        /// </summary>
        private static void startUpdateThreads()
        {
            //Empties the updateThreadList
            if (updateThreadList.Count != 0)
            {
                cleanUpdateThreads();
            }

            //Creates and starts the new threads
            for (int startIndex = 0; startIndex < targetHostList.Count; startIndex++)
            {
                //http://stackoverflow.com/questions/1195896/threadstart-with-parameters
                //Spencer Ruport's answer led to this solution.
                Thread thread = new Thread(updateTargets);
                thread.Name = targetHostList[startIndex].getTargetHostName();
                
                updateThreadList.Add(thread);
                
                updateThreadList[startIndex].Start(startIndex);
            }
        }
        /*-----------------------------------------------------------------------------------------*/

        /// <summary>
        /// Cleans all the updateThreads.
        /// </summary>
        private static void cleanUpdateThreads()
        {
            for (int index = 0; index < updateThreadList.Count; index++)
            {
                if (updateThreadList[index].IsAlive)
                {
                    updateThreadList[index].Join();

                    updateThreadList[index] = null;
                }
            }
            updateThreadList.Clear();
        }
        /*--------------------------------------------------------------------------------*/

        /// <summary>
        /// Method handelong the form closing procedure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gAndDFormFormClosing(object sender, FormClosingEventArgs e)
        {
            cleanUpdateThreads();

            this.Close();
        }
        /*-------------------------------------------------------------------*/


        #endregion
        
    }
}
