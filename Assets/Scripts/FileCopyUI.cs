using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using TMPro;

public class FileCopyUI : MonoBehaviour
{
    public TMP_InputField SourceDirectoryInput;
    public TMP_InputField DestinationDirectoryInput;
    public TMP_InputField NewFolderInput;
    string sourceDir;
    string destDir;
    string NewFolderName;


    public void CopyFiles()
    {
        sourceDir = SourceDirectoryInput.text;
        destDir = DestinationDirectoryInput.text;
        NewFolderName = NewFolderInput.text;

        System.IO.DirectoryInfo source_di = new System.IO.DirectoryInfo(sourceDir);

        //---- Create new Subfolder in the Destination Directory with desired name ----
        destDir = System.IO.Path.Combine(destDir, NewFolderName);
        System.IO.Directory.CreateDirectory(destDir);
        //System.IO.DirectoryInfo dest_di = new System.IO.DirectoryInfo(destDir);

        WalkDirectoryTree(source_di);

        //---- Note this only works for Windows!!! ---
        OpenFolder(destDir);
    }

    public void WalkDirectoryTree(System.IO.DirectoryInfo root)
    {
        System.IO.FileInfo[] files = null;
        System.IO.DirectoryInfo[] subDirs = null;

        files = root.GetFiles("*.*");
        

        if (files != null)
        {
            foreach (System.IO.FileInfo fi in files)
            {
                if(fi.Name.Substring(fi.Name.Length - 3) == "jpg")
                {
                    //UnityEngine.Debug.Log(fi.FullName);
                    File.SetAttributes(fi.FullName, FileAttributes.Normal);
                    File.SetAttributes(destDir, FileAttributes.Normal);
                    //string destinationFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg"; // Name the copied files with respect to time.
                    File.Copy(fi.FullName, Path.Combine(destDir, fi.Name), true);
                    //fi.CopyTo(destDir, true); //---- Function that actually copies the file in question!!! ----
                }

            }

            // Now find all the subdirectories under this directory.
            subDirs = root.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo);
            }
        }
     }

    public void Quit()
    {
        Application.Quit();
    }


    //---- REMOVE FOR MACS ---
    private void OpenFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(folderPath);
            //startInfo.Filename = "explorer.exe";
            System.Diagnostics.Process.Start(startInfo);
        }
        else
        {
            UnityEngine.Debug.Log(string.Format("{0} Directory does not exist!", folderPath));
        }
    }
    
}


/* ---- Backup ----
     static void WalkDirectoryTree(System.IO.DirectoryInfo root)
    {
        System.IO.FileInfo[] files = null;
        System.IO.DirectoryInfo[] subDirs = null;

        files = root.GetFiles("*.*");
        

        if (files != null)
        {
            foreach (System.IO.FileInfo fi in files)
            {
                // In this example, we only access the existing FileInfo object. If we
                // want to open, delete or modify the file, then
                // a try-catch block is required here to handle the case
                // where the file has been deleted since the call to TraverseTree().
                Debug.Log(fi.FullName);
            }

            // Now find all the subdirectories under this directory.
            subDirs = root.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo);
            }
        }
     }
 */



/* ---- For Reference (Original Scripts from Web) ----
public class RecursiveFileSearch
{
    static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();

    static void Main()
    {
        // Start with drives if you have to search the entire computer.
        string[] drives = System.Environment.GetLogicalDrives();

        foreach (string dr in drives)
        {
            System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

            // Here we skip the drive if it is not ready to be read. This
            // is not necessarily the appropriate action in all scenarios.
            if (!di.IsReady)
            {
                Console.WriteLine("The drive {0} could not be read", di.Name);
                continue;
            }
            System.IO.DirectoryInfo rootDir = di.RootDirectory;
            WalkDirectoryTree(rootDir);
        }

        // Write out all the files that could not be processed.
        Console.WriteLine("Files with restricted access:");
        foreach (string s in log)
        {
            Console.WriteLine(s);
        }
        // Keep the console window open in debug mode.
        Console.WriteLine("Press any key");
        Console.ReadKey();
    }

    static void WalkDirectoryTree(System.IO.DirectoryInfo root)
    {
        System.IO.FileInfo[] files = null;
        System.IO.DirectoryInfo[] subDirs = null;

        // First, process all the files directly under this folder
        try
        {
            files = root.GetFiles("*.*");
        }
        // This is thrown if even one of the files requires permissions greater
        // than the application provides.
        catch (UnauthorizedAccessException e)
        {
            // This code just writes out the message and continues to recurse.
            // You may decide to do something different here. For example, you
            // can try to elevate your privileges and access the file again.
            log.Add(e.Message);
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }

        if (files != null)
        {
            foreach (System.IO.FileInfo fi in files)
            {
                // In this example, we only access the existing FileInfo object. If we
                // want to open, delete or modify the file, then
                // a try-catch block is required here to handle the case
                // where the file has been deleted since the call to TraverseTree().
                Console.WriteLine(fi.FullName);
            }

            // Now find all the subdirectories under this directory.
            subDirs = root.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                WalkDirectoryTree(dirInfo);
            }
        }
    }
}

 */
