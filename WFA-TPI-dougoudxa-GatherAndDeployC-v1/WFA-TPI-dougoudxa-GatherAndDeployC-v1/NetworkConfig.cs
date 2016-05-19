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
using System.Threading.Tasks;

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
        private static String[] roomListArray = { "N101", "N102", "N103", "N104", "N109", "N508b", "N509", "N510a", "N510b", "N511", "N512a" };

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

        #endregion
    }
}
