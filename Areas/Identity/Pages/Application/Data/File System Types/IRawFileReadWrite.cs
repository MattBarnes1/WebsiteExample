using System;
using System.Collections.Generic;
using System.Text;

public interface IRawFileReadWrite
{
    bool ReadFile(String aFile,ref byte[] myReadFile);
    bool Write(byte[] myData, String File);
	String[] GetDirectoryStructure();
	bool TraverseToDirectory(String DirectoryString);
    String GetErrors();
	bool CreateDirectoryFromRoot(String myDirectory);
    bool CheckDirectoryExists(string v);
    bool CheckFileExists(string v);
    bool CreateFileFromRoot(string v);
    string[] GetAllFilenamesInDirectory(string aDir);
}

