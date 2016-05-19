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
        /// Domain name of the source host \\domain\.
        /// </summary>
        private String sourceHostName;

        /// <summary>
        /// Path of the source object going to be deployed.
        /// </summary>
        private String sourcePath;

        #endregion

        #region Class methods

        /// <summary>
        /// Constructor of the sourceHost class.
        /// </summary>
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
        /// Sets new path to source object.
        /// </summary>
        /// <param name="newSourcePath"></param>
        public void setSourcePath(String newSourcePath)
        {
            sourcePath = newSourcePath;
        }
        /*--------------------------------------------------------------*/

        #endregion

        #region Class Methods

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
                file.CopyTo(tempPath, true);
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
        /// source: http://stackoverflow.com/questions/468119/whats-the-best-way-to-calculate-the-size-of-a-directory-in-net
        /// Convenient way to calculate the size of a directory. 
        /// Completely based on the source. Only the variable names change.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private long calculateDirectorySize(DirectoryInfo directory)
        {
            long directorySize = 0;

            // Add file sizes.
            FileInfo[] directoryFilesArray = directory.GetFiles();
            foreach (FileInfo file in directoryFilesArray)
            {
                directorySize += file.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] directorySubdirectoriesArray = directory.GetDirectories();
            foreach (DirectoryInfo subdirectory in directorySubdirectoriesArray)
            {
                directorySize += calculateDirectorySize(subdirectory);
            }

            return directorySize;
        }
        /*-------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Method charged with managing a object transfer
        /// </summary>
        /// <param name="sourcePath">Source object</param>
        /// <param name="targetPath">Target object place</param>
        public void share(String sourcePath, String targetPath, bool[] existenceResults)
        {
            System.Windows.Forms.DialogResult result;

            String[] targetName = targetPath.Split('\\');

            //Case where it's not a directory or the directory doesn't exist.
            if(!existenceResults[1])
            {
                //Case where it's neither a file nor a directory -> path doesn't exist
                if (!existenceResults[0])
                {
                    //Do not display Error from here, it will show as many times as there are hosts to synchronise.
                    //Error taken care of in synchronizing method in gAndDForm class.
                }
                else  //Case where it is a file.
                {
                    //Files aren't the same in last modification time.
                    if (!compareExisting("file", sourcePath, targetPath))
                    {
                        File.Copy(sourcePath, targetPath, true);
                    }
                    else
                    {
                        FileInfo currentFile = new FileInfo(sourcePath);
                        FileInfo targetFile = new FileInfo(targetPath);

                        //Displays a multiline message box with information concerning a name conflict.
                        result = System.Windows.Forms.MessageBox.Show(
                            "File already exists, overwrite ?" + Environment.NewLine + Environment.NewLine +
                            "Source :" + Environment.NewLine +
                            "Size : " + formatSizeInteger(currentFile.Length) + Environment.NewLine +
                            "Last modification : " + currentFile.LastWriteTime + Environment.NewLine + Environment.NewLine +
                            "Destination :" + Environment.NewLine +
                            "Size : " + formatSizeInteger(targetFile.Length) + Environment.NewLine +
                            "Last modification : " + targetFile.LastWriteTime,
                            "Warning for host " + targetName[2],
                            System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Information);

                        if(result == System.Windows.Forms.DialogResult.Yes)
                        {
                            File.Copy(sourcePath, targetPath, true);

                            FileInfo overwrittenFile = new FileInfo(targetPath);
                            overwrittenFile.LastWriteTime = DateTime.Now;
                        }
                    }
                }
            }
            else //Case where it is a directory.
            {
                //Directories aren't the same in last modification time.
                if (!compareExisting("directory", sourcePath, targetPath))
                {
                    DirectoryCopy(sourcePath, targetPath, true);
                }
                else
                {
                    DirectoryInfo currentDirectory = new DirectoryInfo(sourcePath);
                    DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);

                    //Displays a multiline message box with information concerning a name conflict.
                    result = System.Windows.Forms.MessageBox.Show(
                            "Directory already exists, overwrite ?" + Environment.NewLine + Environment.NewLine +
                            "Source :" + Environment.NewLine +
                            "Size : " + formatSizeInteger(calculateDirectorySize(currentDirectory)) + Environment.NewLine +
                            "Last modification : " + currentDirectory.LastWriteTime + Environment.NewLine + Environment.NewLine +
                            "Destination :" + Environment.NewLine +
                            "Size : " + formatSizeInteger(calculateDirectorySize(targetDirectory)) + Environment.NewLine +
                            "Last modification : " + targetDirectory.LastWriteTime,
                            "Warning for host " + targetName[2],
                            System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Information);

                    if(result == System.Windows.Forms.DialogResult.Yes)
                    {
                        DirectoryCopy(sourcePath, targetPath, true);
                    }
                }
            }
        }
        /*---------------------------------------------------------------------------------*/

        /// <summary>
        /// Method verifying if a directory/file exists
        /// </summary>
        /// <param name="type">Directory or File</param>
        /// <param name="sourcePath">Source Directory/File</param>
        /// <param name="targetPath">Target Directory/File</param>
        /// <returns>Status true if it exists</returns>
        private bool compareExisting(String type, String sourcePath, String targetPath)
        {
            switch (type)
            {
                case "directory":
                    if(Directory.Exists(targetPath))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case "file":
                    if (File.Exists(targetPath))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return true;
            }
        }
        /*--------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Method that formats an integer into suffixed file sizes.
        /// </summary>
        /// <param name="input">Original size in octet unit.</param>
        /// <returns>The size in the correct octet prefixed unit.</returns>
        private String formatSizeInteger(long input)
        {
            if (input > Math.Pow(1000, 3))
            {
                return (input / Math.Pow(1000, 3)).ToString("F2") + " Go";
            }
            else
            {
                if(input > Math.Pow(1000, 2))
                {
                    return (input / Math.Pow(1000, 2)).ToString("F2") + " Mo";
                }
                else
                {
                    if(input > Math.Pow(1000, 1))
                    {
                        return (input / Math.Pow(1000, 1)).ToString("F2") + " ko";
                    }
                    else
                    {
                        return (input).ToString() + " o";
                    }
                }
            }
        }
        /*-----------------------------------------------------------------------------*/
        
        #endregion
    }
}
