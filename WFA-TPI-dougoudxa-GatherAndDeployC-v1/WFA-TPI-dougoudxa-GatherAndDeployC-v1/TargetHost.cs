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
    class TargetHost
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
        #endregion

        #region Class methods
        /// <summary>
        /// Constructor of the targetHost class.
        /// </summary>
        /// <param name="hostNameInput">Name given to this targetHost by another class of the application</param>
        /// <param name="hostStatusInput">Status given to this targetHost by another class of the application</param>
        public TargetHost(string hostNameInput, string hostStatusInput)
        {
            this.hostName = hostNameInput;
            this.hostStatus = hostStatusInput;

            //Construction, positioning of the different form elements composing a host in the main form.
            //to be defined


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
        /// <param name="newStatus"></param>
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
        #endregion
    }
}
