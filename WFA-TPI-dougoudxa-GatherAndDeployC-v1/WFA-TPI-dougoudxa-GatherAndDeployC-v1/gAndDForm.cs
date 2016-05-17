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

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    public partial class gAndDForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private List<TargetHost> targetHostList = new List<TargetHost>();

        //Source is needed for file/directory transfers. Instanciated.
        private SourceHost currentSource = new SourceHost();

        /// <summary>
        /// 
        /// </summary>
        private bool stopSynchronization = false;

        private Thread syncUpdateThread;

        public gAndDForm()
        {
            InitializeComponent();

            //hardcoding - Transfer test
            TargetHost client1 = new TargetHost("\\\\WIN10-TPI-CLI-1", "ONLINE", 0);
            TargetHost client2 = new TargetHost("\\\\WIN10-TPI-CLI-2", "ONLINE", 1);

            targetHostList.Add(client1);
            targetHostList.Add(client2);

            hostPanelContainer.Controls.Add(targetHostList[0].getTargetHostPanel());
            hostPanelContainer.Controls.Add(targetHostList[1].getTargetHostPanel());

            syncUpdateThread = new Thread(updateTargetStatus);

            //Testing purposes.
            //int dummyEntries = 17;
            //TargetHost tempHost;
            //
            //for (int index = 0; index < dummyEntries; ++index)
            //{
            //    tempHost = new TargetHost("\\\\INF-N511-" + (index + 1).ToString("00"), "dummy test", index);
            //
            //    hostPanelContainer.Controls.Add(tempHost.getTargetHostPanel());
            //}
            //End of test
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void synchroniseButtonClick(object sender, EventArgs e)
        {
            if (synchroniseButton.Text == "Synchronise")
            {
                int index = 0;
                
                synchroniseButton.Text = "Interrupt";
                stopSynchronization = false;
                syncUpdateThread.Start();

                foreach (TargetHost target in targetHostList)
                {
                    synchronise(currentSource, target, index);
                    ++index;
                }
                stopSynchronization = true;
                syncUpdateThread.Abort();
                synchroniseButton.Text = "Synchronise";
            }
            else
            {
                syncUpdateThread.Abort();
                stopSynchronization = true;
                synchroniseButton.Text = "Synchronise";
            }
        }
        /*---------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="index"></param>
        private void synchronise(SourceHost source, TargetHost target, int index)
        {
            //Test sharing and impersonating
            source.DoWorkUnderImpersonation(targetHostList[index].getTargetHostName(),
                sourcePathTextBox.Text, targetHostList[index].getTargetHostName() + targetPathTextBox.Text.Substring(2));


        }
        /*------------------------------------------------------------------*/
        
        /// <summary>
        /// 
        /// </summary>
        private void updateTargetStatus()
        {
            long length = new System.IO.FileInfo(sourcePathTextBox.Text).Length;

            long tempSize;

            while (!stopSynchronization)
            {
                foreach(TargetHost target in targetHostList)
                {
                    tempSize = new System.IO.FileInfo(target.getTargetHostName() + targetPathTextBox.Text.Substring(2)).Length;

                    target.setHostStatus((length / tempSize) * 100 + " % tranferred.");
                }
            }
        }
        /*-------------------------------------------------------------------------------*/
    }
}
