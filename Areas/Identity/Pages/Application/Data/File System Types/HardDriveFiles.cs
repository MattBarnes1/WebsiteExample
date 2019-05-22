using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server;
public class HardDriveFiles : IRawFileReadWrite
{
    public string myCurrentDirectory = "~/";
    private IHostingEnvironment fileReadWriteManager;
    readonly string ContentRoot;
    public HardDriveFiles(IHostingEnvironment fileReadWriteManager)
    {
        ContentRoot = myCurrentDirectory = fileReadWriteManager.ContentRootPath;
        this.fileReadWriteManager = fileReadWriteManager;
    }

    public bool ReadFile(String aFile, ref byte[] myReadFile)
    {
        if (!HandleDirectoryConversion(ref aFile)) return false;
        if(File.Exists(aFile))
        {
            FileStream aStream = File.OpenRead(aFile);
            myReadFile = new byte[aStream.Length];
            aStream.Read(myReadFile);
            aStream.Close();
            return true;
        }
        else
        {
            ReportedError = "No such file found: " + aFile;
            return false;
        }
    }

    public bool Write(byte[] myData, String aFile)
    {
        if (!HandleDirectoryConversion(ref aFile)) return false;
        if (File.Exists(aFile))
        {
            
            FileStream aStream = File.Create(aFile);
            return true;
        }
        else
        {
            ReportedError = "No such file found: " + aFile;
            return false;
        }
    }



    public String[] GetDirectoryStructure()
    {
        List<String> myStrings = new List<string>();
        var myDirEnum = Directory.EnumerateDirectories(myCurrentDirectory);
        var myEnum = myDirEnum.GetEnumerator();
        while (myEnum.MoveNext())
        {
            myStrings.Add(myEnum.Current);
        }
        return myStrings.ToArray();
    }


    private bool HandleDirectoryConversion(ref String MyString)
    {
        MyString = MyString.Replace('/', Path.DirectorySeparatorChar);
        MyString = MyString.Replace('\\', Path.DirectorySeparatorChar);
        if (MyString[0] == '~' && MyString[1] == Path.DirectorySeparatorChar)
        {
            MyString = MyString.Remove(0, 1);
            MyString = ContentRoot + MyString;
        }
        else if (MyString[0] == '.')
        {
            if (MyString[1] == Path.DirectorySeparatorChar)
            {
                MyString = MyString.Remove(0, 2);
                MyString = myCurrentDirectory + MyString;
            }
            else if(MyString[1] == '.' && MyString[2] == Path.DirectorySeparatorChar)
            {
               if(myCurrentDirectory.CompareTo("~" + Path.DirectorySeparatorChar) == 0)
               {
                    ReportedError = "Tried to leave root path!";
                    return false;
               }
               else
               {
                    String PriorPath = myCurrentDirectory.Remove(myCurrentDirectory.LastIndexOf(Path.DirectorySeparatorChar), myCurrentDirectory.Length - 1 - myCurrentDirectory.LastIndexOf(Path.DirectorySeparatorChar) - myCurrentDirectory.Length);
                    MyString = MyString.Remove(0, 3);
                    MyString = PriorPath + Path.DirectorySeparatorChar + MyString;
               }
            }
            else
            {
                ReportedError = "Malformed Directory!";
                return false;
            }
        }
        else if(MyString[0] == Path.DirectorySeparatorChar)
        {
            MyString = ContentRoot + MyString;
        }
        //if we don't have a file, then we're adjusting for missed /
        if(MyString[MyString.Length - 4] != '.' && MyString[MyString.Length-1] != Path.DirectorySeparatorChar)
        {
            MyString += Path.DirectorySeparatorChar;
        }
            return true;
    }

    public bool TraverseToDirectory(String DirectoryString)
    {
        if(HandleDirectoryConversion(ref DirectoryString))
        {
            if(Directory.Exists(DirectoryString))
            {
                myCurrentDirectory = DirectoryString;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
	}

    String ReportedError = "";
	public String GetErrors()
	{
        var TempError = ReportedError;
        ReportedError = "";
        return TempError;
	}

	public bool CreateDirectoryFromRoot(String myDirectory)
    {
        if (!HandleDirectoryConversion(ref myDirectory)) return false;
        Directory.CreateDirectory(myDirectory);
        return true;
    }



    public bool CheckDirectoryExists(string v)
    {
        if (!HandleDirectoryConversion(ref v)) return false;
        return Directory.Exists(v);
    }

    public bool CheckFileExists(string v)
    {
        if (!HandleDirectoryConversion(ref v)) return false;
        return File.Exists(v);
    }

    public bool CreateFileFromRoot(string v)
    {
        if (!HandleDirectoryConversion(ref v)) return false;
        if (File.Exists(v))
        {
            ReportedError += "File Exists!\n";
            return false;
        } else
        {
            var myOpenedFile = File.Create(v);
            myOpenedFile.Close();
            return true;
        }
    }

    public string[] GetAllFilenamesInDirectory(string aDir)
    {
        if (!HandleDirectoryConversion(ref aDir)) return new string[0];
        if (!Directory.Exists(aDir)) return new string[0];
        String[] myFiles = Directory.GetFiles(aDir);
        for(int i = 0; i < myFiles.Length; i++)
        {
            myFiles[i] = myFiles[i].Substring(myFiles[i].LastIndexOf("/") + 1);
        }
        return myFiles;
    }
}

