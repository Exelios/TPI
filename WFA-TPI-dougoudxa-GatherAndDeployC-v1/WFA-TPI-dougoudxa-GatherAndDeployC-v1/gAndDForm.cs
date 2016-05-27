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
        public static SourceHost currentSource = new SourceHost();

        /// <summary>
        /// Variable necessary to stop synchronization process.
        /// </summary>
        private static bool stopSynchronization = false;

        /// <summary>
        /// Variable allowing or stopping the status update of all the hosts.
        /// </summary>
        private volatile static bool stopUpdating = true;

        /// <summary>
        /// Tells the event methd to add a scroll bar when needed.
        /// </summary>
        private bool needLogScrollBar = false;

        /// <summary>
        /// 
        /// </summary>
        public static String logText = "";

        /// <summary>
        /// 
        /// </summary>
        //private Thread updateLog = new Thread(updateLogText);

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
        }
        /*-------------------------------------------------------------------*/

        /// <summary>
        /// Event method when synchronizeButton is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void synchronizeButtonClick(object sender, EventArgs e)
        {

            String[] fileNameArray = sourcePathTextBox.Text.Split('\\');

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
                    //Shows the deployment message whether it is a file or directory.
                    if (File.Exists(sourcePathTextBox.Text))
                    {
                        logTextBox.AppendText("Deploying file - " + fileNameArray[fileNameArray.Length - 1] + ":" + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        logTextBox.AppendText("Deploying directory - " + fileNameArray[fileNameArray.Length - 1] + ":" + Environment.NewLine + Environment.NewLine);
                    }

                    //Transfers file/directory to every host chosen.
                    foreach (TargetHost target in targetHostList)
                    {
                        if (target.getSyncCheckBoxState())
                        {
                            logTextBox.AppendText("    " + targetHostList[index].getTargetHostName() + " syncing... " + Environment.NewLine
                                + "******************************************************" + Environment.NewLine);

                            synchronize(currentSource, target, existenceResults, index);

                            logTextBox.AppendText(Environment.NewLine + "\tDone " + DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToLongTimeString() + Environment.NewLine + Environment.NewLine);
                        }

                        ++index;
                    }

                    logTextBox.AppendText("Synchronization finished!" + Environment.NewLine + Environment.NewLine);
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
        /// Method diffrenciating a file from a directory.
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
            if (!stopSynchronization)
            {
                if (target.getHostStatus() == NetworkConfig.connectionStatusArray[0] + ' ' && target.getSyncCheckBoxState())
                {
                    gAndDForm.logText = "";

                    source.share(sourcePathTextBox.Text,
                        targetHostList[index].getTargetHostName() + targetPathTextBox.Text.Substring(2),
                        existenceResults);

                    logTextBox.AppendText(gAndDForm.logText);
                }
                else
                {
                    //void
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Check your input values.", "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            SourceHost.setSourcePath(sourcePathTextBox.Text);

            updateSourceInfoLabel(sourcePathTextBox.Text);

            //Needed to represent correctly manual hosts and room hosts.
            int hostIndex = 0;

            String tempRoom;

            List<String> manualHost = new List<String>();

            bool manualHostEmpty = true;

            //Retrieves all the manual hosts typed in.
            if(manualHostTextBox.Text != "")
            {
                int index = 0;
                String[] tempArray = manualHostTextBox.Text.Split('\n');

                foreach(String hostName in tempArray)
                {
                    index = hostName.IndexOf("\r");

                    //if/else needed to trime unwanted chars from all names except the last one.
                    if (index > 0)
                    {
                        manualHost.Add(hostName.Substring(0, index));
                    }
                    else
                    {
                        manualHost.Add(hostName);
                    }
                }

                manualHostEmpty = false;
            }

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

            //Assignes all the room hosts.
            if (tempRoom != "")
            {
                for (int index = 0; index < NetworkConfig.MACHINE_AMOUNT; index++)
                {
                    tempHostName = "\\\\INF-" + tempRoom + "-" + (index + 1).ToString("00");

                    targetHostList.Add(new TargetHost(tempHostName, "Pinging...", hostIndex));

                    targetHostList[hostIndex].setTargetPath(targetHostList[hostIndex].getTargetHostName() + targetPathTextBox.Text.Substring(2));

                    hostPanelContainer.Controls.Add(targetHostList[hostIndex].getTargetHostPanel());

                    //Keeps count of the hosts for next part of code.
                    ++hostIndex;
                }
            }

            //Assignes all the manual hosts.
            if (!manualHostEmpty)
            {
                foreach(String hostName in manualHost)
                {
                    if (hostName != "" && hostName != "\r")
                    {
                        //Condition allowing both "\\INF-N511-09" and "INF-N511-09" to be accepted
                        if (hostName.Substring(0, 2) == "\\\\")
                        {
                            targetHostList.Add(new TargetHost(hostName, "Pinging...", hostIndex));

                            targetHostList[hostIndex].setTargetPath(targetHostList[hostIndex].getTargetHostName() + targetPathTextBox.Text.Substring(2));

                            hostPanelContainer.Controls.Add(targetHostList[hostIndex].getTargetHostPanel());

                            ++hostIndex;
                        }
                        else
                        {
                            targetHostList.Add(new TargetHost("\\\\" + hostName, "Pinging...", hostIndex));

                            targetHostList[hostIndex].setTargetPath(targetHostList[hostIndex].getTargetHostName() + targetPathTextBox.Text.Substring(2));

                            hostPanelContainer.Controls.Add(targetHostList[hostIndex].getTargetHostPanel());

                            ++hostIndex;
                        }
                    }
                }
            }

            startUpdateThreads();
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

            analyzeSyncedState(targetHostList[(int)index]);
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
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int getTargetHostIndex(String name)
        {
            int index = -1;

            foreach(TargetHost target in targetHostList)
            {
                if(target.getTargetHostName() != name)
                {
                    ++index;
                }
            }
            return index + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        private static void analyzeSyncedState(TargetHost target)
        {
            String targetPath = target.getTargetPath();

            stopSynchronization = false;

            if(SourceHost.compareExisting("file", targetPath))
            {
                FileInfo targetFile = new FileInfo(targetPath);
                FileInfo sourceFile = new FileInfo(SourceHost.getSourcePath());

                //Verify that both paths lead to a same type of file

                if (targetFile.Extension == sourceFile.Extension)
                {
                    if (targetFile.Length < sourceFile.Length || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                    {
                        target.setInfoProgressBarLabel("Need sync: " +
                                SourceHost.formatSizeInteger(targetFile.Length) + " " +
                                targetFile.LastWriteTime);
                    }
                    else
                    {
                        target.setInfoProgressBarLabel("Synced: " +
                                SourceHost.formatSizeInteger(targetFile.Length) + " " +
                                targetFile.LastWriteTime);
                    }
                }
                //Error in the destination and source paths, they don't match to a same type of object.
                else
                {
                    System.Windows.Forms.MessageBox.Show("Extension mismatch", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    stopSynchronization = true;
                }
            }
            else
            {
                if(SourceHost.compareExisting("directory", targetPath))
                {
                    DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);
                    DirectoryInfo sourceDirectory = new DirectoryInfo(SourceHost.getSourcePath());

                    if (targetDirectory.Extension == sourceDirectory.Extension)
                    {

                        if (SourceHost.calculateDirectorySize(targetDirectory) < SourceHost.calculateDirectorySize(sourceDirectory)
                            || targetDirectory.LastWriteTime < sourceDirectory.LastWriteTime)
                        {
                            target.setInfoProgressBarLabel("Need sync: " +
                                SourceHost.formatSizeInteger(SourceHost.calculateDirectorySize(targetDirectory)) + " " +
                                targetDirectory.LastWriteTime);
                        }
                        else
                        {
                            target.setInfoProgressBarLabel("Synced: " +
                                SourceHost.formatSizeInteger(SourceHost.calculateDirectorySize(targetDirectory)) + " " +
                                targetDirectory.LastWriteTime);
                        }
                    }
                    //Error in the destination and source paths, they don't match to a same type of object.
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Source or target isn't a directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        stopSynchronization = true;
                    }
                }
                else
                {
                    //No file or directory with the source name exists -> need to sync
                    if (target.getSyncCheckBoxState())
                    {
                        target.setInfoProgressBarLabel("Need Sync");
                    }
                    else
                    {
                        target.setInfoProgressBarLabel("");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        private void updateSourceInfoLabel(String sourcePath)
        {
            if (File.Exists(sourcePath))
            {
                FileInfo sourceFile = new FileInfo(sourcePath);

                sourceInfoLabel.Text = "Source information: " + sourceFile.Name +
                    " / Size: " + SourceHost.formatSizeInteger(sourceFile.Length) +
                    " / Last write time: " + sourceFile.LastWriteTime;
            }
            else
            {
                DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);

                sourceInfoLabel.Text = "Source information: " + sourceDirectory.Name + 
                    " / Size: " + SourceHost.formatSizeInteger(SourceHost.calculateDirectorySize(sourceDirectory)) + 
                    " / Last write time: " + sourceDirectory.LastWriteTime;
            }
        }
        
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logSaveButtonClick(object sender, EventArgs e)
        {
            String logFilePath = "C:\\Users\\" + (String)Environment.UserName + "\\Desktop\\g&d_log.txt";

            FileStream stream = new FileStream(logFilePath, FileMode.Create);

            StreamWriter write = new StreamWriter(stream, System.Text.Encoding.Default);
            write.Write(logTextBox.Text);

            write.Close();

            System.Windows.Forms.MessageBox.Show("LOG saved as " + logFilePath);
        }
        /*-----------------------------------------------------------*/

        /// <summary>
        /// Adds a vertical scrollbar to logTextBox when number of lines 
        /// is greater than the amount of lines the textBox can show.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!needLogScrollBar)
            {
                if (logTextBox.Lines.Length > 22)
                {
                    needLogScrollBar = true;

                    logTextBox.ScrollBars = ScrollBars.Vertical;
                }
            }
        }
        /*-----------------------------------------------------------------*/

        public void appendLogTextBox(String additionnalText)
        {
            logTextBox.AppendText(additionnalText);
        }
        #endregion

    }
}
