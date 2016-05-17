/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class used to manage every interaction with the source of a command.


using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;   // DllImport
using System.Security.Permissions;      // WindowsImpersonationContext
using System.Security.Principal;        // PermissionSetAttribute
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{

    /// <summary>
    /// Most of this code is from this link 
    /// http://stackoverflow.com/questions/5023607/how-to-use-logonuser-properly-to-impersonate-domain-user-from-workgroup-client
    /// </summary>
    public class SourceHost
    {
        #region Class attributes

        /// <summary>
        /// 
        /// </summary>
        private String sourceHostName;

        /// <summary>
        /// 
        /// </summary>
        private String sourceHostPassword;

        /// <summary>
        /// 
        /// </summary>
        private String sourcePath;
        #endregion

        #region Class methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceName"></param>
        public SourceHost()
        {
            sourceHostName = System.Environment.UserName;

            //http://stackoverflow.com/questions/10797774/messagebox-with-input-field
            //Doesn't hide the password. temporary solution.
            sourceHostPassword = Interaction.InputBox(sourceHostName, "Password input");
        }
        /*------------------------------------------*/

            /// <summary>
            /// 
            /// </summary>
            /// <param name="newSourcePath"></param>
        public void setSourcePath(String newSourcePath)
        {
            sourcePath = newSourcePath;
        }
        /*--------------------------------------------------------------*/

        /// <summary>
        /// obtains user token
        /// </summary>
        /// <param name="pszUsername"></param>
        /// <param name="pszDomain"></param>
        /// <param name="pszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        /*-------------------------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// closes open handes returned by LogonUser
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);
        /*-------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="hostPath"></param>
        /// <param name="targetPath"></param>
        public void DoWorkUnderImpersonation(String targetName, String hostPath, String targetPath)
        {
            //elevate privileges before doing file copy to handle domain security
            WindowsImpersonationContext impersonationContext = null;
            IntPtr userHandle = IntPtr.Zero;
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;

            String domain = targetName;
            String user = "Exelios";        //Admin account
            String password = "alendiel";   //Admin account


            //System.Windows.Forms.MessageBox.Show("Domaine name: " + targetName,
            //            null,
            //            System.Windows.Forms.MessageBoxButtons.OK,
            //            System.Windows.Forms.MessageBoxIcon.Information);

            try
            {
                // if domain name was blank, assume local machine
                if (domain == "")
                    domain = Environment.MachineName;

                // Call LogonUser to get a token for the user
                bool loggedOn = LogonUser(user, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref userHandle);

                if (!loggedOn)
                {
                    System.Windows.Forms.MessageBox.Show("Exception impersonating user, error code: " + Marshal.GetLastWin32Error(),
                        null,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }

                // Begin impersonating the user
                impersonationContext = WindowsIdentity.Impersonate(userHandle);

                //System.Windows.Forms.MessageBox.Show("Main() windows identify after impersonation: " + WindowsIdentity.GetCurrent().Name);

                //run the program with elevated privileges (like file copying from a domain server)
                share(hostPath, targetPath);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception impersonating user: " + ex.Message);
            }
            finally
            {
                // Clean up
                if (impersonationContext != null)
                {
                    impersonationContext.Undo();
                }

                if (userHandle != IntPtr.Zero)
                {
                    CloseHandle(userHandle);
                }
            }
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        private void share(String sourcePath, String targetPath)
        {
            File.Copy(sourcePath, targetPath, true);
        }
        /*---------------------------------------------------------------------------------*/

        #endregion
    }
}
