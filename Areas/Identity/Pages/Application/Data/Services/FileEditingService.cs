using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public static class FileEditingService
    {
        struct DocumentChange
        {
            DateTime myChange;
            String FileDirectory;
        }


        static IRawFileReadWrite myFilesystem;

        static List<String> MyActivelyUsedDirectories = new List<string>();

        public static void InitializeLookupService(IRawFileReadWrite myRW, UserManager<UserData> myManager)
        {
            Debug.Assert(myFilesystem == null);
            myFilesystem = myRW;
            VerifyFilesystemSetup(myManager);
            VerifyChangeLogDirectories();
            VerifyLoreDirectory();
        }

        public static String[] GetFilenamesInDirectory(String aDir)
        {
            return myFilesystem.GetAllFilenamesInDirectory(aDir);
        }


        private static void VerifyLoreDirectory()
        {
            if (!myFilesystem.CheckDirectoryExists("/Lore"))
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/Lore"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
            if (!myFilesystem.CheckDirectoryExists("/Lore/Enacted"))
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/Lore/Enacted"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
        }

        internal static bool DoesFileExist(string v)
        {
            throw new NotImplementedException();
        }

        public  static void WriteStringToFile(string Data, string File)
        {
            throw new NotImplementedException();
        }

        private static void VerifyChangeLogDirectories()
        {
            if (!myFilesystem.CheckDirectoryExists("/ChangeLog"))
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/ChangeLog"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
            if (!myFilesystem.CheckFileExists("/ChangeLog/Website.txt"))
            {
                if (!myFilesystem.CreateFileFromRoot("/ChangeLog/Website.txt"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
            if (!myFilesystem.CheckFileExists("/ChangeLog/Engine.txt"))
            {
                if (!myFilesystem.CreateFileFromRoot("/ChangeLog/Engine.txt"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
        }

        public static byte[] ReadFileAsBytes(string v)
        {
            byte[] myArray = null;
            myFilesystem.ReadFile(v, ref myArray);
            return myArray;
        }

        public static String ReadFileAsString(String v)
        {
            byte[] myBytes = ReadFileAsBytes(v);
            String RetVal = "";
            for (int i = 0; i < myBytes.Length; i++)
            {
                RetVal += (char)myBytes[i];
            }
            return RetVal;
        }

        private static void VerifyFilesystemSetup(UserManager<UserData> myManager)
        {
            foreach (UserData A in myManager.Users)
            {
                var Roles = myManager.GetRolesAsync(A);
                Roles.Wait();
                CreateUserHome(A, Roles.Result.ToList());
            }
        }


        public static void CreateUserHome(UserData id, List<String> myRoles)
        {
            if (!myFilesystem.CheckDirectoryExists("/Home/" + id.Id))
            {
                if(!myFilesystem.CreateDirectoryFromRoot("/Home/" + id.Id))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }            
            if(myRoles.Contains("Modeler") || myRoles.Contains("LeadModeler"))
            {
                if (!myFilesystem.CheckDirectoryExists("/Home/" + id.Id + "/Models")) //Mail System
                {
                    if (!myFilesystem.CreateDirectoryFromRoot("/Home/" + id.Id + "/Models"))
                    {
                        throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                    }
                }
            }
            if (!myFilesystem.CheckDirectoryExists("/Home/" + id.Id + "/DevBlogs")) //Mail System
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/Home/" + id.Id + "/DevBlogs"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
            if (!myFilesystem.CheckDirectoryExists("/Home/" + id.Id + "/Lore")) //Mail System
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/Home/" + id.Id + "/Lore"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
            if (!myFilesystem.CheckDirectoryExists("/Home/" + id.Id + "/Lore/Proposed")) //Mail System
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/Home/" + id.Id + "/Lore/Proposed"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
            if (!myFilesystem.CheckDirectoryExists("/Home/" + id.Id + "/Lore/Unproposed")) //Mail System
            {
                if (!myFilesystem.CreateDirectoryFromRoot("/Home/" + id.Id + "/Lore/Unproposed"))
                {
                    throw new Exception("Error: Could not create Home Directory Server Reports: " + myFilesystem.GetErrors());
                }
            }
        }
    }
}
