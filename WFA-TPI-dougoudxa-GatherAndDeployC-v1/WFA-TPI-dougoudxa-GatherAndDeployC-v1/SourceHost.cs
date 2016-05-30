/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    Class used to manage every interaction with the source of a command.


using System;
using System.IO;

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
        private static String sourcePath;

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
        public static void setSourcePath(String newSourcePath)
        {
            sourcePath = newSourcePath;
        }
        /*--------------------------------------------------------------*/

        public static String getSourcePath()
        {
            return sourcePath;
        }

        /// <summary>
        /// Taken from this source:
        /// https://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx
        /// </summary>
        /// <param name="sourceDirectoryPath"></param>
        /// <param name="TargetDirectoryPath"></param>
        /// <param name="copySubdirectories"></param>
        private static void copyDirectory(String sourceDirectoryPath, String TargetDirectoryPath, bool copySubdirectories)
        {
            //Checking if the directory has been created.
            bool directoryCreated = false;

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirectoryPath);

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(TargetDirectoryPath))
            {
                //LOG entry here!
                gAndDForm.currentSource.sendLogEntry("Creating: " + Environment.NewLine + TargetDirectoryPath);

                Directory.CreateDirectory(TargetDirectoryPath);

                directoryCreated = true;
            }
            else
            {
                //Directory.Delete(TargetDirectoryPath, true);

                //directoryCreated = false;
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                String tempPath = Path.Combine(TargetDirectoryPath, file.Name);

                //LOG entry here!
                gAndDForm.currentSource.sendLogEntry("Copying: " + Environment.NewLine + tempPath);

                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            // Uses recursion.
            if (copySubdirectories)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    String temppath = Path.Combine(TargetDirectoryPath, subdir.Name);
                    copyDirectory(subdir.FullName, temppath, copySubdirectories);
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
        public static long calculateDirectorySize(DirectoryInfo directory)
        {
            System.Threading.Thread.Sleep(500);
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
        /// <param name="targetPath">Target object place containing target name in pole position</param>
        public void share(String sourcePath, String targetPath, bool[] existenceResults)
        {
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
                    //Files aren't the same.
                    if (!compareExisting("file", targetPath))
                    {
                        //LOG entry here!
                        sendLogEntry("Copying: " + Environment.NewLine + targetPath);

                        File.Copy(sourcePath, targetPath, true);
                    }
                    else
                    {
                        //Taken care of by syncState
                    }
                }
            }
            else //Case where it is a directory.
            {
                //Directories aren't the same in last modification time.
                if (!compareExisting("directory", targetPath))
                {
                    //LOG entry not here!

                    copyDirectory(sourcePath, targetPath, true);
                }
                else
                {
                    //Taken care of by syncState
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
        public static bool compareExisting(String type, String targetPath)
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
        public static String formatSizeInteger(long input)
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
        
        /// <summary>
        /// Appends a status text the main form logTextBox.
        /// </summary>
        /// <param name="newLogEntry">Text to append</param>
        private void sendLogEntry(String newLogEntry)
        {
            Program.Form.appendLogTextBox("\t" + newLogEntry + Environment.NewLine);
        }
        /*-----------------------------------------------*/
        #endregion
    }
}
