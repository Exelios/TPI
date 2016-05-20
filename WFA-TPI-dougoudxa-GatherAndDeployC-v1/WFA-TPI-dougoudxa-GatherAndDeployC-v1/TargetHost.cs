/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class designed to represent Host with their names and status.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const string STATUS_TEXT = "Status: ";

        /// <summary>
        /// Contains the host's name.
        /// </summary>
        private string hostName;

        /// <summary>
        /// contains the host's status.
        /// </summary>
        private string hostStatus;

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
        private const int PANEL_HEIGHT = 35;

        /// <summary>
        /// Width of targetHost's panel.
        /// </summary>
        private const int PANEL_WIDTH = 183;
        
        #endregion

        #region Class methods

        /// <summary>
        /// Constructor of the targetHost class.
        /// </summary>
        /// <param name="hostNameInput">Name given to this targetHost by another class of the application</param>
        /// <param name="hostStatusInput">Status given to this targetHost by another class of the application</param>
        public TargetHost(string hostNameInput, string hostStatusInput, int index)
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
            hostNameLabel.Size = new System.Drawing.Size(LABEL_WIDTH, LABEL_HEIGHT);
            hostStatusLabel.Size = new System.Drawing.Size(LABEL_WIDTH - 20, LABEL_HEIGHT);

            //Positionning the labels in the targetHostPanel
            hostNameLabel.Location = new System.Drawing.Point(0, 0);
            hostStatusLabel.Location = new System.Drawing.Point(20, 14);

            //Adding the controls in the panel
            hostPanel.Controls.Add(hostNameLabel);
            hostPanel.Controls.Add(hostStatusLabel);

        }
        /*----------------------------------------------------*/

        /// <summary>
        /// Gets the host's current status.
        /// </summary>
        /// <returns>Host current status</returns>
        public string getHostStatus()
        {
            string[] returnStatArray = this.hostStatus.Split(' ');

            string returnStatus = "";

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
        /// </summary>
        /// <param name="newStatus">New status overwriting the old one</param>
        public void setHostStatus(string newStatus)
        {
            this.hostStatus = TargetHost.STATUS_TEXT + newStatus;
        }
        /*------------------------------------------------*/

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
        /// Sets a new target path to host
        /// </summary>
        /// <param name="newTargetPath">New path</param>
        public void setTargetPath(String newTargetPath)
        {
            hostPath = newTargetPath;
        }
        /*--------------------------------------------------------*/
        #endregion
    }
}
