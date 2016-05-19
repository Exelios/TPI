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
            // 18.05.2016: Not needed anymore
            //sourceHostPassword = Interaction.InputBox(sourceHostName, "Password input");
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
        /// Taken from this source:
        /// https://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx
        /// </summary>
        /// <param name="sourceDirectoryPath"></param>
        /// <param name="TargetDirectoryPath"></param>
        /// <param name="copySubdirectories"></param>
        private static void DirectoryCopy(string sourceDirectoryPath, string TargetDirectoryPath, bool copySubdirectories)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirectoryPath);

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(TargetDirectoryPath))
            {
                Directory.CreateDirectory(TargetDirectoryPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(TargetDirectoryPath, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            // Uses recursion.
            if (copySubdirectories)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(TargetDirectoryPath, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubdirectories);
                }
            }
        }
        /*----------------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        public void share(String sourcePath, String targetPath, bool[] existenceResults)
        {
            //Case where it's not a directory or the directory doesn't exist.
            if(!existenceResults[1])
            {
                //Case where it's neither a fole nor a directory -> path doesn't exist
                if (!existenceResults[0])
                {
                    //Do not display Error from here, it will show as many times as there are hosts to synchronise.
                    //Error taken care of in synchronizing method in gAndDForm class.
                }
                else  //Case where it is a file.
                {
                    File.Copy(sourcePath, targetPath, true);
                }
            }
            else //Case where it is a directory.
            {
                DirectoryCopy(sourcePath, targetPath, true);
            }
        }
        /*---------------------------------------------------------------------------------*/

        #endregion
    }
}
