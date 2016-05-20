/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class managing every network related process.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    class NetworkConfig
    {
        #region Class attributes

        /// <summary>
        /// 
        /// </summary>
        public const int roomAmount = 11;

        /// <summary>
        /// 
        /// </summary>
        public const int machineAmount = 17;

        /// <summary>
        /// 
        /// </summary>
        private static String[] roomListArray = { "N101", "N102", "N103", "N104", "N109", "N508b", "N509", "N510a", "N510b", "N511", "N512a" };

        /// <summary>
        /// 
        /// </summary>
        private static String[] connectionStatusArray = { "Connected", "Disconnected", "Unreachable" };
        
        /// <summary>
        /// 
        /// </summary>
        public static Thread analyzeThread = new Thread(update);

        #endregion

        #region Class methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static String getRoom(int index)
        {
            return roomListArray[index];
        }
        /*---------------------------------------------------------------------*/

        /// <summary>
        /// http://stackoverflow.com/questions/3689728/ping-a-hostname-on-the-network
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string PingHost(string host)
        {
            //string to hold our return messge
            string returnMessage = string.Empty;

            //IPAddress instance for holding the returned host
            bool success = true;

            IPAddress address = null;

            try
            {
                address = Dns.GetHostEntry(host).AddressList[0];
            }
            catch (SocketException ex)
            {
                success = false;
            }

            //create a new ping instance
            Ping ping = new Ping();

            if (success)
            {
                //here we will ping the host 4 times (standard)
                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        //send the ping 4 times to the host and record the returned data.
                        //The Send() method expects 4 items:
                        //1) The IPAddress we are pinging
                        //2) The timeout value
                        PingReply pingReply = ping.Send(address, 20);

                        //make sure we dont have a null reply
                        if (!(pingReply == null))
                        {
                            switch (pingReply.Status)
                            {
                                case IPStatus.Success:
                                    returnMessage = connectionStatusArray[0];
                                    break;
                                case IPStatus.TimedOut:
                                    returnMessage = connectionStatusArray[2];
                                    break;
                                default:
                                    returnMessage = connectionStatusArray[2];
                                    break;
                            }
                        }
                        else
                            returnMessage = connectionStatusArray[2];
                    }
                    catch (PingException ex)
                    {
                        returnMessage = connectionStatusArray[2];
                    }
                    catch (SocketException ex)
                    {
                        returnMessage = connectionStatusArray[2];
                    }
                }
            }
            else
            {
                returnMessage = connectionStatusArray[2];
            }
            //return the message
            return returnMessage;
        }
        /*-----------------------------------------------------------------------------------*/

        /// <summary>
        /// Method keeping the status of a host up to date.
        /// </summary>
        private static void updateTargetStatus()
        {
            String tempHostName;

            foreach (TargetHost target in gAndDForm.targetHostList)
            {
                tempHostName = target.getTargetHostName().Split('\\')[2];

                target.setHostStatus(NetworkConfig.PingHost(tempHostName));
            }

            Thread.Sleep(3000);
        }
        /*-------------------------------------------------------------------------------*/

        private static void update()
        {
            while (true)
            {
                updateTargetStatus();
            }
        }

        public static String getConnectionStatus(String hostName)
        {
            return null;
        }

        #endregion
    }
}
