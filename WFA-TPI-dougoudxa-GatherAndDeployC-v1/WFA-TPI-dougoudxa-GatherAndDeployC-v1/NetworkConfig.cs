/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class managing every network related process.


using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    class NetworkConfig
    {
        #region Class attributes
        
        /// <summary>
        /// Number of machines per classroom.
        /// </summary>
        public const int MACHINE_AMOUNT = 16;

        /// <summary>
        /// Array of strings representing the differents classrooms at ETML.
        /// </summary>
        private static String[] roomListArray = { "", "N101", "N102", "N103", "N104", "N109", "N501", "N508b", "N509", "N510b", "N511", "N512a", "N512b" };

        /// <summary>
        /// Number of rooms at ETML.
        /// </summary>
        public static int roomAmount = roomListArray.Length;

        /// <summary>
        /// Array containing the different connection statuses.
        /// </summary>
        public static String[] connectionStatusArray = { "Connected", "Disconnected", "Offline" };

        #endregion

        #region Class methods

        /// <summary>
        /// Gets the room name.
        /// </summary>
        /// <param name="index">Index for the list</param>
        /// <returns>String room name</returns>
        public static String getRoom(int index)
        {
            return roomListArray[index];
        }
        /*---------------------------------------------------------------------*/

        /// <summary>
        /// Method that pings a host and returns a status string
        /// http://stackoverflow.com/questions/3689728/ping-a-hostname-on-the-network
        /// </summary>
        /// <param name="host">The target host getting pinged</param>
        /// <returns>Status string message</returns>
        public static String pingHost(String host)
        {
            //string to hold our return messge
            String returnMessage = String.Empty;

            //IPAddress instance for holding the returned host
            bool success = true;

            IPAddress address = null;

            int timeOut = 3000;

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
            

            byte[] buffer = new Byte[32];

            if (success)
            {    
                try
                {
                    //1) The IPAddress we are pinging
                    //2) The timeout value
                    //3) Buffer size.
                    PingReply pingReply = ping.Send(address, timeOut, buffer);

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
        public static void updateTargetStatus(TargetHost target)
        {
            String targetName = target.getTargetHostName();
            
            targetName = target.getTargetHostName().Split('\\')[2];

            target.setHostStatus(target.getTargetStatusLabel(), NetworkConfig.pingHost(targetName));
        }
        /*-------------------------------------------------------------------------------*/

        #endregion
    }
}
