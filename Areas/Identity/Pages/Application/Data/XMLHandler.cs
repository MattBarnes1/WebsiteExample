using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

public class XMLHandler
{
    XmlDocument myDoc;

    public XMLHandler(byte[] File)
	{
        myDoc = new XmlDocument();
        char[] myCharacter = new char[File.Length];
        String myString = "";
        for (int i = 0; i < File.Length; i++)
        {
            myString += Convert.ToChar(File[i]);
        }
        myDoc.LoadXml(myString);
        Debug.Write(myDoc.ToString());
	}

	public string GetTagTreeNodeData(String TagTree)
	{
        String[] Path = TagTree.Split("/");
        return "TODO:";//TODO:
	}
}

