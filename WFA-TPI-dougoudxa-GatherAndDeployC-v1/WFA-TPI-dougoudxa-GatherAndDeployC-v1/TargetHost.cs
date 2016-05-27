/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class designed to represent Host with their names and status.

using System;
using System.Windows.Forms;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    /// <summary>
    /// Contains every element needed to represent a target host in the main form.
    /// </summary>
    public class TargetHost
    {
        #region Class Attributes

        /// <summary>
        /// First characters in hostStatus String. 
        /// Is of constant value.
        /// </summary>
        private const String STATUS_TEXT = "Status: ";

        /// <summary>
        /// Contains the host's name.
        /// </summary>
        private String hostName;

        /// <summary>
        /// contains the host's status.
        /// </summary>
        private String hostStatus;

        /// <summary>
        /// Path of destination for incoming file/directory transfers
        /// </summary>
        private String hostPath;

        /// <summary>
        /// Represents the host in the host list box in the main form.
        /// </summary>
        private Panel hostPanel = new Panel();

        /// <summary>
        /// Included in the hostPanel.
        /// </summary>
        private Label hostNameLabel = new Label();

        /// <summary>
        /// Included in the hostPanel.
        /// </summary>
        private Label hostStatusLabel = new Label();

        /// <summary>
        /// Included in the hostPanel
        /// </summary>
        private CheckBox syncHostCheckBox = new CheckBox();

        /// <summary>
        /// 
        /// </summary>
        private InfoProgressBar updateBar;

        /// <summary>
        /// Height of the labels in the targetHost labels.
        /// </summary>
        private const int LABEL_HEIGHT = 13;

        /// <summary>
        /// Width of the labels in the targetHost labels.
        /// </summary>
        private const int LABEL_WIDTH = 180;

        /// <summary>
        /// Height of targetHost's panel.
        /// </summary>
        private const int PANEL_HEIGHT = 50;

        /// <summary>
        /// Width of targetHost's panel.
        /// </summary>
        private const int PANEL_WIDTH = 197;

        /// <summary>
        /// Needed from http://stackoverflow.com/questions/14703698/invokedelegate
        /// </summary>
        /// <param name="label">Label needing a sync</param>
        /// <param name="newStatus">New label text</param>
        public delegate void crossThreadUpdateStatus(Control label, String newStatus);

        /// <summary>
        /// Delegate needed to sync the syncCheckBox from foreign thread
        /// </summary>
        /// <param name="checkBox">CheckBox needing to be synced</param>
        public delegate void crossThreadUpdateCheckBox(CheckBox checkBox);
        
        #endregion

        #region Class methods

        /// <summary>
        /// Constructor of the targetHost class.
        /// </summary>
        /// <param name="hostNameInput">Name given to this targetHost by another class of the application</param>
        /// <param name="hostStatusInput">Status given to this targetHost by another class of the application</param>
        public TargetHost(String hostNameInput, String hostStatusInput, int index)
        {
            this.hostName = hostNameInput;
            this.hostStatus = STATUS_TEXT + hostStatusInput;

            //Construction, positioning of the different form elements composing a host in the main form.

            hostPanel.Size = new System.Drawing.Size(PANEL_WIDTH, PANEL_HEIGHT);
            hostPanel.Location = new System.Drawing.Point(1, index * (PANEL_HEIGHT + 1));

            if (index % 2 == 0)
            {
                hostPanel.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                hostPanel.BackColor = System.Drawing.SystemColors.Control;
            }
            //will be added in the main panel by main class

            //Assigning text to labels
            hostNameLabel.Text = hostName;
            hostStatusLabel.Text = hostStatus;

            //Sizing the 2 labels
            hostNameLabel.Size = new System.Drawing.Size(LABEL_WIDTH - 80, LABEL_HEIGHT);
            hostStatusLabel.Size = new System.Drawing.Size(LABEL_WIDTH - 50, LABEL_HEIGHT);

            //Positionning the labels in the targetHostPanel
            hostNameLabel.Location = new System.Drawing.Point(0, 0);
            hostStatusLabel.Location = new System.Drawing.Point(20, 14);

            //Positionning and initializing the checkbox
            syncHostCheckBox.Location = new System.Drawing.Point(150, 0);
            syncHostCheckBox.Text = "Sync";

            //Adding InforProgressBar
            updateBar = new InfoProgressBar(0, 29, PANEL_WIDTH - 1, 17);
            updateBar.setVisible(true);

            //Adding the controls in the panel
            hostPanel.Controls.Add(hostNameLabel);
            hostPanel.Controls.Add(hostStatusLabel);
            hostPanel.Controls.Add(syncHostCheckBox);
            hostPanel.Controls.Add(updateBar.getProgressBar());
        }
        /*----------------------------------------------------*/

        /// <summary>
        /// Gets the host's current status.
        /// </summary>
        /// <returns>Host current status</returns>
        public String getHostStatus()
        {
            String[] returnStatArray = this.hostStatus.Split(' ');

            String returnStatus = "";

            //appends every substring to the status string. Skips the first one which is STATUS_TEXT.
            for(int index = 1; index < returnStatArray.Length; ++index)
            {
                returnStatus += (returnStatArray[index] + ' ');
            }//end of loop

            return returnStatus;
        }
        /*------------------------------------------*/

        /// <summary>
        /// Sets new value for the host's status.
        /// http://stackoverflow.com/questions/14703698/invokedelegate
        /// </summary>
        /// <param name="newStatus">New status overwriting the old one</param>
        public void setHostStatus(Control statusLabel, String newStatus)
        {
            if (!statusLabel.InvokeRequired)
            {
                this.hostStatus = TargetHost.STATUS_TEXT + newStatus;

                statusLabel.Text = this.hostStatus;
            }
            else
            {
                statusLabel.Invoke(new crossThreadUpdateStatus(setHostStatus), new object[] { statusLabel, newStatus });
            }
        }
        /*------------------------------------------------*/

        /// <summary>
        /// Gets the status label of a host.
        /// </summary>
        /// <returns>Control statusLabel</returns>
        public Control getTargetStatusLabel()
        {
            return hostStatusLabel;
        }

        /// <summary>
        /// Updates the host's different attributes in the main form.
        /// </summary>
        public void updateHostState()
        {
            hostStatusLabel.Text = hostStatus;
        }
        /*---------------------------------------------------*/

        /// <summary>
        /// Allows external class to get the hostPanel.
        /// </summary>
        /// <returns>hostPanel</returns>
        public Panel getTargetHostPanel()
        {
            return hostPanel;
        }
        /*---------------------------------------------------*/

        /// <summary>
        /// Getter fetching the name of the host
        /// </summary>
        /// <returns>Host's name</returns>
        public String getTargetHostName()
        {
            return hostName;
        }
        /*---------------------------------------------------*/

        /// <summary>
        /// Sets a new target path to host.
        /// </summary>
        /// <param name="newTargetPath">New path</param>
        public void setTargetPath(String newTargetPath)
        {
            hostPath = newTargetPath;
        }
        /*--------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getTargetPath()
        {
            return hostPath;
        }
        /*--------------------------------*/

        /// <summary>
        /// Gets the checked state of a CheckBox.
        /// </summary>
        /// <returns>True if CheckBox checked, otherwise false.</returns>
        public bool getSyncCheckBoxState()
        {
            if (syncHostCheckBox.Checked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*--------------------------------------------------------*/

        /// <summary>
        /// Gets the syncHostCheckBox.
        /// </summary>
        /// <returns>the currents hosts checkBox</returns>
        public CheckBox getSyncCheckBox()
        {
            return syncHostCheckBox;
        }
        /*------------------------------------------------------------------*/

        /// <summary>
        /// Sets the checkBox state to a new state.
        /// </summary>
        public void setSyncCheckBoxState(CheckBox checkBox)
        {
            if (!syncHostCheckBox.InvokeRequired)
            {
                if (this.getTargetStatusLabel().Text.Split(' ')[1] == NetworkConfig.connectionStatusArray[0])
                {
                    checkBox.Checked = true;
                }
                else
                {
                    checkBox.Checked = false;
                }
            }
            else
            {
                checkBox.Invoke(new crossThreadUpdateCheckBox(setSyncCheckBoxState), new object[] { checkBox });
            }
        }
        /*-------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public InfoProgressBar getInfoProgressBar()
        {
            return updateBar;
        }
        /*----------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newText"></param>
        public void setInfoProgressBarLabel(String newText)
        {
            this.updateBar.setLabel(this.updateBar.getLabel(), newText);
        }

        #endregion
    }
}
