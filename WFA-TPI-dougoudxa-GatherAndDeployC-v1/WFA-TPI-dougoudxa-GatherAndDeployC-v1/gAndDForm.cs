/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class managing every interaction with user.

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
        /// Give general progression status
        /// </summary>
        private InfoProgressBar generalProgressBar;

        /// <summary>
        /// Variable necessary to stop synchronization process.
        /// </summary>
        private static bool stopSynchronization = false;

        /// <summary>
        /// Thread syncing the hosts
        /// </summary>
        private Thread syncThread;
                
        /// <summary>
        /// Tells the event methd to add a scroll bar when needed.
        /// </summary>
        private bool needLogScrollBar = false;

        /// <summary>
        /// Temporary string container.
        /// </summary>
        public static String logText = "";

        /// <summary>
        /// Says if we can analyse the targets according to a source path or not.
        /// </summary>
        private bool analyzable;

        /// <summary>
        /// Variable used to activate or disable the analyzeButton
        /// </summary>
        public bool usableAnalysisButton = true;

        /// <summary>
        /// Cross thread delegate enabling buttons
        /// </summary>
        /// <param name="button">Button to be enabled</param>
        /// <param name="state">True enables the button, otherwise false</param>
        private delegate void crossThreadButtonEnabler(Control button, bool state);

        /// <summary>
        ///  Cross thread delegate updating log
        /// </summary>
        /// <param name="log">Log TextBox</param>
        /// <param name="text">text to be appended</param>
        private delegate void crossThreadLogUpdater(TextBox log, String text);

        /// <summary>
        /// Cross thread delegate updating control texts
        /// </summary>
        /// <param name="control">Control to be updated</param>
        /// <param name="Text">New text</param>
        private delegate void crossThreadControlTextUpdater(Control control, String Text);

        /// <summary>
        /// Cross thread bar updater
        /// </summary>
        /// <param name="bar">ProgressBar to be updated</param>
        /// <param name="value">New value</param>
        private delegate void crossThreadProgressBarUpdater(ProgressBar bar, int value);

        /// <summary>
        /// Cross thread CheckBox enabler
        /// </summary>
        /// <param name="box">CheckBox to be enabled</param>
        /// <param name="state">True enables the CheckBox, otherwise false</param>
        private delegate void crossThreadEnableCheckBox(CheckBox box, bool state);
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

            sourceInfoLabel.Hide();

            generalProgressBar = new InfoProgressBar(5, 424, 593, 26);
            generalProgressBar.setVisible(true);

            this.Controls.Add(generalProgressBar.getProgressBar());
        }
        /*-------------------------------------------------------------------*/

        /// <summary>
        /// Event method when synchronizeButton is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void synchronizeButtonClick(object sender, EventArgs e)
        {
            if (Program.Form.synchronizeButton.Text == "Synchronize")
            {
                this.analyzeButton.Enabled = false;

                syncThread = new Thread(synchronizeThread);
                syncThread.Start();

                //realTimeThread = new Thread(updateInRealTime);
                //realTimeThread.Start();
                
                Program.Form.synchronizeButton.Text = "Interrupt";

                stopSynchronization = false;
            }
            else
            {
                this.analyzeButton.Enabled = true;

                syncThread.Abort();

                syncThread = null;

                updateLogText(this.logTextBox, Environment.NewLine + "Synchronization interrupted" + Environment.NewLine + Environment.NewLine);



                Program.Form.synchronizeButton.Text = "Synchronize";
            }
        }
        /*---------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        private static void synchronizeThread()
        {
            String[] fileNameArray = Program.Form.sourcePathTextBox.Text.Split('\\');
                
            int index = 0;

            //Checks if the source exists
            bool[] existenceResults = Program.Form.checkExistence(Program.Form.sourcePathTextBox.Text);

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
                if (File.Exists(Program.Form.sourcePathTextBox.Text))
                {
                    Program.Form.updateLogText(Program.Form.logTextBox, "Deploying file - " + 
                        fileNameArray[fileNameArray.Length - 1] + ":" + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    Program.Form.updateLogText(Program.Form.logTextBox, "Deploying directory - " + 
                        fileNameArray[fileNameArray.Length - 1] + ":" + Environment.NewLine + Environment.NewLine);
                }

                //Stopwatch to time sync to one host
                System.Diagnostics.Stopwatch timeAmount = new System.Diagnostics.Stopwatch();

                //Number of syncing hosts.
                int syncingHosts = 0;

                String sourceInfo = Program.Form.sourceInfoLabel.Text;

                //Syncing host amount
                foreach(TargetHost target in targetHostList)
                {
                    if (target.getSyncCheckBoxState())
                    {
                        ++syncingHosts;
                    }
                }

                int totalHosts = syncingHosts;

                //Transfers file/directory to every host chosen.
                foreach (TargetHost target in targetHostList)
                {
                    if(index == 0)
                    {
                        timeAmount.Start();
                    }

                    if (target.getSyncCheckBoxState())
                    {
                       

                        if (Directory.Exists(target.getTargetHostName() + Program.Form.targetPathTextBox.Text.Substring(2)))
                            {
                                 Directory.Delete(target.getTargetHostName() + Program.Form.targetPathTextBox.Text.Substring(2), true);
                            }

                        Program.Form.updateLogText(Program.Form.logTextBox, 
                            "    " + targetHostList[index].getTargetHostName() + 
                            " syncing... " + Environment.NewLine
                            + "******************************************************" + Environment.NewLine);

                        Program.Form.synchronize(currentSource, target, existenceResults, index);

                        Program.Form.updateLogText(Program.Form.logTextBox, 
                            Environment.NewLine + "\tDone " + DateTime.Now.ToShortDateString() +
                           ' ' + DateTime.Now.ToLongTimeString() + Environment.NewLine + Environment.NewLine);

                        //One host less to sync
                        --syncingHosts;
                    }

                    int transferTime = 0;

                    if (timeAmount.IsRunning)
                    {
                        timeAmount.Stop();

                        transferTime = (int)timeAmount.Elapsed.TotalSeconds;
                    }

                    transferTime = (int)timeAmount.Elapsed.TotalSeconds * syncingHosts;

                    Program.Form.generalProgressBar.getProgressBar().Refresh();

                    //Not functionnal
                    //updateProgressBar(Program.Form.generalProgressBar.getProgressBar(),
                    //  (totalHosts - syncingHosts) * 100 / totalHosts);

                    Program.Form.generalProgressBar.setLabel(
                        Program.Form.generalProgressBar.getLabel(),
                        "Sync progress : " + (totalHosts - syncingHosts) * 100 / totalHosts  + "%  -  " +  
                        "Time left : " + Convert.ToString(transferTime) + " seconds.");


                    ++index;
                }

                Program.Form.updateLogText(Program.Form.logTextBox, 
                    "Synchronization finished!" + Environment.NewLine + Environment.NewLine);

                //Allows user to save the log.
                Program.Form.enableButton(Program.Form.logSaveButton, true);
            }

            Program.Form.updateControlText(Program.Form.synchronizeButton,"Synchronize");

            if (Program.Form.synchronizeButton.Text == "Synchronize")
            {
                Program.Form.enableButton(Program.Form.analyzeButton, true);
            }
        }
        /*---------------------------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="text"></param>
        public void updateLogText(TextBox log, String text)
        {
            if (!log.InvokeRequired)
            {
                log.AppendText(text);
            }
            else
            {
                log.Invoke(new crossThreadLogUpdater(updateLogText), new object[] { log, text });
            }
        }
        /*---------------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        public void updateControlText(Control control, String text)
        {
            if (!control.InvokeRequired)
            {
                control.Text = text;
            }
            else
            {
                control.Invoke(new crossThreadControlTextUpdater(updateControlText), new object[] { control, text });
            }
        }

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
            //If the synchronize button has already been enabled form previous sync
            if (synchronizeButton.Enabled == true || logSaveButton.Enabled == true)
            {
                synchronizeButton.Enabled = false;
                logSaveButton.Enabled = false;
            }

            //Prevents deactivation of analysis button if no host were selected.
            if (manualHostTextBox.Text != "" || roomComboBox.Text != "")
            {
                //Prevents an overflow of analysis.
                analyzeButton.Enabled = false;
                usableAnalysisButton = false;
            }

            //Empties the hostPanel
            hostPanelContainer.Controls.Clear();

            targetHostList.Clear();

            SourceHost.setSourcePath(sourcePathTextBox.Text);

            updateSourceInfoLabel(sourcePathTextBox.Text);

            //Clear the text in the general progress bar.
            generalProgressBar.getProgressBar().Refresh();

            //Write the correct information.
            generalProgressBar.setLabel(generalProgressBar.getLabel(), sourceInfoLabel.Text);

            if (analyzable)
            {
                //Needed to represent correctly manual hosts and room hosts.
                int hostIndex = 0;

                String tempRoom;

                List<String> manualHost = new List<String>();

                bool manualHostEmpty = true;

                //Retrieves all the manual hosts typed in.
                if (manualHostTextBox.Text != "")
                {
                    //index of unwanted substring in string
                    int index = 0;
                    String[] tempArray = manualHostTextBox.Text.Split('\n');

                    foreach (String hostName in tempArray)
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
                    List<TargetHost> tempList = new List<TargetHost>();

                    foreach (TargetHost target in targetHostList)
                    {
                        // Copy the current content of the target host list.
                        tempList.Add(target);
                    }
                    

                    foreach (String hostName in manualHost)
                    {
                        //Boolean of existance status used below.
                        bool alreadyExists = false;

                        //Verifiey if a host had already been added by the room selection process.
                        foreach(TargetHost target in tempList)
                        {
                            if (hostName == target.getTargetHostName().Split('\\')[2])
                            {
                                alreadyExists = true;
                                break;
                            }
                        }


                        if(!alreadyExists)
                        {
                            //condition where visually blacks inputs exist, we don't want them.
                            if (hostName != "" && hostName != "\r")
                            {
                                //Condition allowing both "\\INF-N511-09" and "INF-N511-09" spellings to be accepted
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
                }

                startUpdateThreads();
            }
            //Allows to use the synchronizeButton.
            synchronizeButton.Enabled = true;

            analyzable = false;
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

            if(targetHostList[(int)index].getHostStatus().Split(' ')[0] != NetworkConfig.connectionStatusArray[0])
            {
                enableCheckBox(targetHostList[(int)index].getSyncCheckBox(), false);
            }

            //Updates the Synced state of very host
            analyzeSyncedState(targetHostList[(int)index]);


            if (targetHostList[(int)index].getInfoProgressBar().getLabel().Text.Split(':')[0] == "Need sync")
            {
                //Updating the checkbox
                targetHostList[(int)index].setSyncCheckBoxState(targetHostList[(int)index].getSyncCheckBox());
            }          

            if (Program.Form.usableAnalysisButton)
            {
                Program.Form.enableButton(Program.Form.analyzeButton, true);
            }
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

                while (!updateThreadList[startIndex].IsAlive)
                {
                    updateThreadList[startIndex].Start(startIndex);
                }

                if (startIndex == targetHostList.Count - 1)
                {
                    Program.Form.usableAnalysisButton = true;
                }
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
                //Stops the threads then deletes them
                if (updateThreadList[index].IsAlive)
                {
                    updateThreadList[index].Join();

                    updateThreadList[index] = null;
                }
                else//Deletes non-running threads
                {
                    updateThreadList[index] = null;
                }
            }

            //Empties the threadList.
            updateThreadList.Clear();
        }
        /*--------------------------------------------------------------------------------*/

        /// <summary>
        /// Gets the index of a given target name in the targetHostList
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
        /*-----------------------------------------------------------------*/

        /// <summary>
        /// Analyses each target and sees if it's synced or not with the source.
        /// </summary>
        /// <param name="target">target passed by parameter.</param>
        private static void analyzeSyncedState(TargetHost target)
        {
            String targetPath = target.getTargetPath();

            //stopSynchronization = false;

            //Case transfer concernes a file
            if (SourceHost.compareExisting("file", targetPath))
            {
                FileInfo targetFile = new FileInfo(targetPath);
                FileInfo sourceFile = new FileInfo(SourceHost.getSourcePath());

                //Verify that both paths lead to a same type of file
                if (targetFile.Extension == sourceFile.Extension)
                {
                    //if the target was created before last update of source you need to sync
                    if (targetFile.Length < sourceFile.Length || targetFile.CreationTime < sourceFile.LastWriteTime)
                    {
                        target.setInfoProgressBarLabel("Need sync: " +
                                SourceHost.formatSizeInteger(targetFile.Length) + " " +
                                targetFile.CreationTime);
                    }
                    else
                    {
                        target.setInfoProgressBarLabel("Synced: " +
                                SourceHost.formatSizeInteger(targetFile.Length) + " " +
                                targetFile.CreationTime);
                    }
                }
                //Error in the destination and source paths, they don't match to a same type of object.
                else
                {
                    System.Windows.Forms.MessageBox.Show("Extension mismatch", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //stopSynchronization = true;
                }
            }
            else
            {
                //Case transfer concernes a directory
                if(SourceHost.compareExisting("directory", targetPath))
                {
                    DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);
                    DirectoryInfo sourceDirectory = new DirectoryInfo(SourceHost.getSourcePath());

                    if (targetDirectory.Extension == sourceDirectory.Extension)
                    {
                        //if the target was created before last update of source you need to sync
                        if (SourceHost.calculateDirectorySize(targetDirectory) < SourceHost.calculateDirectorySize(sourceDirectory)
                            || targetDirectory.CreationTime < sourceDirectory.LastWriteTime)
                        {
                            target.setInfoProgressBarLabel("Need sync: " +
                                SourceHost.formatSizeInteger(SourceHost.calculateDirectorySize(targetDirectory)) + " " +
                                targetDirectory.CreationTime);
                        }
                        else
                        {
                            target.setInfoProgressBarLabel("Synced: " +
                                SourceHost.formatSizeInteger(SourceHost.calculateDirectorySize(targetDirectory)) + " " +
                                targetDirectory.CreationTime);
                        }
                    }
                    //Error in the destination and source paths, they don't match to a same type of object.
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Source or target isn't a directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //stopSynchronization = true;
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
        /*---------------------------------------------------------------------------------*/

        /// <summary>
        /// Updates the source info label.
        /// </summary>
        /// <param name="sourcePath">Path of source being shown in the label</param>
        private void updateSourceInfoLabel(String sourcePath)
        {
            //Case source is a file.
            if (File.Exists(sourcePath))
            {
                FileInfo sourceFile = new FileInfo(sourcePath);

                sourceInfoLabel.Text = "Source information: " + sourceFile.Name +
                    " / Size: " + SourceHost.formatSizeInteger(sourceFile.Length) +
                    " / Last write time: " + sourceFile.LastWriteTime;

                analyzable = true;
            }
            else
            {
                //Case source is a file.
                if (Directory.Exists(sourcePath))
                {
                    DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);

                    sourceInfoLabel.Text = "Source information: " + sourceDirectory.Name +
                        " / Size: " + SourceHost.formatSizeInteger(SourceHost.calculateDirectorySize(sourceDirectory)) +
                        " / Last write time: " + sourceDirectory.LastWriteTime;

                    analyzable = true;
                }
                //Case source doesn't exist.
                else
                {
                    analyzable = false;

                    System.Windows.Forms.MessageBox.Show("No such file or directory.", "Source Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Incase source doesn't exist reactivates the analyze button
                    Program.Form.enableButton(Program.Form.analyzeButton, true);
                }
            }
        }
        /*--------------------------------------------------------------------------------------------------*/
        
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
            String logFilePath = "C:\\Users\\" + (String)Environment.UserName + "\\Desktop\\g&d_log_" + 
               DateTime.Now.ToShortDateString() + '_' + DateTime.Now.ToLongTimeString().Replace(':','.') + ".txt";

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

        /// <summary>
        /// Appends status text in the logTextBox
        /// </summary>
        /// <param name="additionnalText">appended text to log</param>
        public void appendLogTextBox(String additionnalText)
        {
            Program.Form.updateLogText(Program.Form.logTextBox, additionnalText);
        }
        /*----------------------------------------------*/

        /// <summary>
        /// Method that enables or disables a button
        /// </summary>
        /// <param name="button">Button to be activated</param>
        /// <param name="state">True will enable the button</param>
        public void enableButton(Control button, bool state)
        {
            if (!button.InvokeRequired)
            {
                button.Enabled = state;
            }
            else
            {
                button.Invoke(new crossThreadButtonEnabler(enableButton), new object[] { button, state });
            }
        }
        /*---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Not operationnal
        /// </summary>
        /// <param name="targetHost"></param>
        /// <returns></returns>
        private double calculateTransferedAmount(TargetHost targetHost)
        {
            return getTransferSize(targetHost.getTargetPath()) / getTransferSize(sourcePathTextBox.Text);
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// Not operationnal
        /// </summary>
        private static void updateInRealTime()
        {
            int syncingHosts = 0;

            long dataLength = 0;
            while (!stopSynchronization)
            {
                foreach (TargetHost target in targetHostList)
                {
                    if (target.getSyncCheckBoxState())
                    {
                        ++syncingHosts;



                        //Thread.Sleep(1000);

                        updateProgressBar(target.getInfoProgressBar().getProgressBar(),
                            100 * Convert.ToInt32(Program.Form.calculateTransferedAmount(target)));
                    }
                }
            }

            //dataLength = syncingHosts * getTransferSize(Program.Form.sourcePathTextBox.Text);
        }
        /*-----------------------------------------------------------------------------------------*/

        /// <summary>
        /// Not operationnal
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static long getTransferSize(String path)
        {
            if (path != "")
            {
                //source is a file
                if (File.Exists(path))
                {
                    FileInfo source = new FileInfo(path);

                    return source.Length;
                }
                else   //source is a directory.
                {
                    DirectoryInfo source = new DirectoryInfo(path);

                    return SourceHost.calculateDirectorySize(source);
                }
            }
            else
            {
                return 0;
            }
        }
        /*------------------------------------------------------------*/

        /// <summary>
        /// Sets progress bar value.
        /// </summary>
        /// <param name="bar">Needs a value change</param>
        /// <param name="value">New value</param>
        private static void updateProgressBar(ProgressBar bar, int value)
        {
            if (!bar.InvokeRequired)
            {
                bar.Value = value;
            }
            else
            {
                bar.Invoke(new crossThreadProgressBarUpdater(updateProgressBar), new object[] { bar, value });
            }
        }
        /*-----------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Enables or disables a CheckBox
        /// </summary>
        /// <param name="box">Checkbox going to be modified</param>
        /// <param name="state">True enables the CheckBox</param>
        private static void enableCheckBox(CheckBox box, bool state)
        {
            if (!box.InvokeRequired)
            {
                box.Enabled = state;
            }
            else
            {
                box.Invoke(new crossThreadEnableCheckBox(enableCheckBox), new object[] { box, state });
            }
        }
        #endregion

    }
}
