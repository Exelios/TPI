/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   27.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class managing progression outputs.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    public class InfoProgressBar
    {
        #region Class attributes

        /// <summary>
        /// ProgressBar
        /// </summary>
        private ProgressBar syncBar = new ProgressBar();

        /// <summary>
        /// ProgressBar x position
        /// </summary>
        private int xPosition;

        /// <summary>
        /// ProgressBar y position
        /// </summary>
        private int yPosition;

        /// <summary>
        /// ProgressBar width
        /// </summary>
        private int xLength;

        /// <summary>
        /// ProgressBar height
        /// </summary>
        private int yLength;

        /// <summary>
        /// Label containing information text
        /// </summary>
        private Label syncBarLabel = new Label();
        
        /// <summary>
        /// Delegate allowing cross thread modifications
        /// </summary>
        /// <param name="Label">Control needing modification</param>
        /// <param name="newText">Modification</param>
        public delegate void crossThreadSyncState(Control Label, String newText); 

        #endregion

        #region Class methods

        /// <summary>
        /// Constructor of the InfoProgressBar class
        /// </summary>
        public InfoProgressBar(int posX, int posY, int sizeX, int sizeY)
        {
            xPosition = posX;
            yPosition = posY;
            xLength = sizeX;
            yLength = sizeY;

            syncBar.Location = new System.Drawing.Point(xPosition, yPosition);
            syncBar.Size = new System.Drawing.Size(xLength, yLength);

            syncBarLabel.Location = new System.Drawing.Point(xPosition + 2, yPosition + 1);
            syncBarLabel.Size = new System.Drawing.Size(xLength-2, 13);
        }
        /*---------------------------------------------------*/

        /// <summary>
        /// Tells if the Progress of InfoProgressBar instance is shown or not.
        /// </summary>
        /// <returns>True means visible, otherwise false.</returns>
        public bool isVisible()
        {
            return syncBar.Visible;
        }
        /*-------------------------------------------------------*/

        /// <summary>
        /// Sets Visibility of ProgressBar form instance of InfoProgressBar
        /// </summary>
        /// <param name="newVisibility">True shows the ProgressBar, else false.</param>
        public void setVisible(bool newVisibility)
        {
            syncBar.Visible = newVisibility;
        }
        /*--------------------------------------------------------*/

        /// <summary>
        /// Gets the ProgressBar of an InfoProgressBar
        /// </summary>
        /// <returns>ProgressBar of InfoProgressBar</returns>
        public ProgressBar getProgressBar()
        {
            return syncBar;
        }
        /*--------------------------------------*/

        /// <summary>
        /// Gets the label of an InfoProgressBar
        /// </summary>
        /// <returns>Label of InfoProgressBar</returns>
        public Label getLabel()
        {
            return syncBarLabel;
        }
        /*----------------------------------------*/

        /// <summary>
        /// Writes text in progress bar
        /// </summary>
        /// <param name="newText">Text to be shown</param>
        public void setLabel(Control Label, String newText)
        {
            if (!Label.InvokeRequired)
            {
                Label.Text = newText;

                syncBar.CreateGraphics().DrawString(Label.Text, new System.Drawing.Font("Microsoft sanserif", (float)8.25, System.Drawing.FontStyle.Regular)
                , System.Drawing.Brushes.Black, new System.Drawing.Point(2, 1));
            }
            else
            {
                Label.Invoke(new crossThreadSyncState(setLabel), new object[] { Label, newText });
            }
        }
        /*-------------------------------------*/
        #endregion
    }
}
