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
        /// 
        /// </summary>
        private ProgressBar syncBar = new ProgressBar();

        /// <summary>
        /// 
        /// </summary>
        private int xPosition;

        /// <summary>
        /// 
        /// </summary>
        private int yPosition;

        /// <summary>
        /// 
        /// </summary>
        private int xLength;

        /// <summary>
        /// 
        /// </summary>
        private int yLength;

        /// <summary>
        /// 
        /// </summary>
        private Label syncBarLabel = new Label();

        /// <summary>
        /// 
        /// </summary>
        private float percentage = 0;

        public delegate void crossThreadSyncState(Control Label, String newText); 

        #endregion

        #region Class methods

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isVisible()
        {
            return syncBar.Visible;
        }
        /*-------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVisibility"></param>
        public void setVisible(bool newVisibility)
        {
            syncBar.Visible = newVisibility;
        }
        /*--------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProgressBar getProgressBar()
        {
            return syncBar;
        }
        /*--------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Label getLabel()
        {
            return syncBarLabel;
        }
        /*----------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newText"></param>
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
