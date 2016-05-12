using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.DirectoryServices;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    public partial class appForm : Form
    {
        public appForm()
        {
            InitializeComponent();

            //////for testing purposes only////////////////////////////////////////////////
            int nb_hosts = 0;

            List<String> hostEntryList = new List<String>();
            

            //http://stackoverflow.com/questions/2557551/how-get-list-of-local-network-computers
            //Author: Cynfeal
            DirectoryEntry root = new DirectoryEntry("WinNT:");
            foreach (DirectoryEntry computers in root.Children)
            {
                foreach (DirectoryEntry computer in computers.Children)
                {
                    if (computer.Name != "Schema")
                    {
                        hostEntryList.Add(computer.Name);
                        ++nb_hosts;
                    }
                }
            }
            /*--------------------------------------------------------------------------*/
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
            TargetHost tempTargetHost;
            IPHostEntry tempHostEntry;
            IPAddress tempHostIP;

            Ping testPing = new Ping();
            PingReply reply;
                                                                                        
            for(int i = 0; i < nb_hosts-1; ++i)                                           
            {
                //tempHostEntry = Dns.GetHostEntry(hostEntryList[i]);
                //tempHostIP = tempHostEntry.AddressList[0];

                //reply = testPing.Send(tempHostIP);

                //if (reply.Status == IPStatus.Success)
                //{

                    tempTargetHost = new TargetHost(hostEntryList[i], "Inactive test", i);

                    hostPanelContainer.Controls.Add(tempTargetHost.getTargetHostPanel());
                //}
            }

            hostEntryList.Clear();
        } 
    }
}
