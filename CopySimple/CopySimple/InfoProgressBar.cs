using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopySimple
{
    class InfoProgressBar
    {
        #region Class attributes

        /// <summary>
        /// 
        /// </summary>
        private ProgressBar syncBar = new ProgressBar();
        
        /// <summary>
        /// 
        /// </summary>
        private Label syncBarLabel = new Label();
        
        /// <summary>
        /// 
        /// </summary>
        private String infoOutput = null;
        
        /// <summary>
        /// 
        /// </summary>
        private float purcentage = 0;
        
        /// <summary>
        /// 
        /// </summary>
        private bool visiblility = false;

        #endregion

        #region Class methods

        /// <summary>
        /// 
        /// </summary>
        public InfoProgressBar()
        {

        }
        /*---------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isVisible()
        {
            return visiblility;
        }
        /*-------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVisibility"></param>
        public void setVisible(bool newVisibility)
        {
            visiblility = newVisibility;
        }
        /*--------------------------------------------------------*/
        #endregion
    }
}   
