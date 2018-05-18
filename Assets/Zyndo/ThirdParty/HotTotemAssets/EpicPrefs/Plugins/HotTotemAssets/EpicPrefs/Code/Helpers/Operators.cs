using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;

public static class Operators {
    public static bool IsInteger(string sValue)
    {
        if (sValue == "-" || sValue == "" || sValue == " ")
            return false;
        else
            return true;
    }
    public static bool IsFloat(string sValue)
    {
        if (sValue == "-" || sValue == "" || sValue == " " || sValue == "." || sValue == "-.")
            return false;
        else
            return true;
    }
    public static bool ToBool(string value)
    {
        if (value.ToLower().Trim() == "true" || value == "1")
            return true;
        else
            return false;
    }
    public static float ToFloat(string value)
    {
        float newFloat = 0;
        string minus = "-";
        bool negative = false;
        if (value.Length > 0)
            if (value[0] == minus[0])
                negative = true;
        value = Regex.Replace(value, "[^0-9.]", "");
        if (value.Contains("."))
        {
            string preDot = "";
            string afterDot = "";
            preDot = value.Substring(0, 1 + value.IndexOf("."));
            afterDot = value.Substring(value.IndexOf(".") + 1);
            afterDot = Regex.Replace(afterDot, "[^0-9]", "");
            value = preDot + afterDot;
        }
        if (negative)
            value = "-" + value;
        if (value != "" && value != "-")
            newFloat = Convert.ToSingle(value);
        return newFloat;
    }
    public static double ToDouble(string value)
    {
        double newDouble = 0;
        string minus = "-";
        bool negative = false;
        if (value.Length > 0)
            if (value[0] == minus[0])
                negative = true;
        value = Regex.Replace(value, "[^0-9.]", "");
        if (value.Contains("."))
        {
            string preDot = "";
            string afterDot = "";
            preDot = value.Substring(0, 1 + value.IndexOf("."));
            afterDot = value.Substring(value.IndexOf(".") + 1);
            afterDot = Regex.Replace(afterDot, "[^0-9]", "");
            value = preDot + afterDot;
        }
        if (negative)
            value = "-" + value;
        if (value != "" && value != "-")
            newDouble = Convert.ToDouble(value);
        return newDouble;
    }
    public static int ToInt(string value)
    {
        int newInt = 0;
        string minus = "-";
        bool negative = false;
        if (value.Length > 0)
            if (value[0] == minus[0])
                negative = true;
        value = Regex.Replace(value, "[^0-9]", "");
        if (negative)
            value = "-" + value;
        if (value != "" && value != "-")
            newInt = Convert.ToInt32(value);
        return newInt;
    }
    public static long ToLong(string value)
    {
        long newLong = 0;
        string minus = "-";
        bool negative = false;
        if (value.Length > 0)
            if (value[0] == minus[0])
                negative = true;
        value = Regex.Replace(value, "[^0-9]", "");
        if (negative)
            value = "-" + value;
        if (value != "" && value != "-")
            newLong = Convert.ToInt64(value);
        return newLong;
    }
	public static Color StringToColor(string value) {
		float r = 0;
		float g = 0;
		float b = 0;
		float a = 0;
        if(value.Length != 0){
            r = ToFloat(value.Substring (3, value.IndexOf ("[g]") - 3)); // [r]0.2[g]0.8[b]0.9
            g = ToFloat(value.Substring (value.IndexOf ("[g]") + 3, value.IndexOf ("[b]") - (value.IndexOf ("[g]") + 3))); // [r]0.2[g]0.8[b]0.9
            b = ToFloat (value.Substring (value.IndexOf ("[b]") + 3, value.IndexOf ("[a]") - (value.IndexOf ("[b]") + 3)));
            a = ToFloat (value.Substring (value.IndexOf ("[a]") + 3));
            if(r<0f)
                r = 0f;
            if(g<0f)
                g = 0f;
            if(b<0f)
                b = 0f;
            if(a<0f)
                a = 0f;
        }
		Color newColor = new Color (r,g,b,a);
		return newColor;
	}
	public static string ColorToString(Color value){
		float r = value.r;
		float g = value.g;
		float b = value.b;
		float a = value.a;
		string strCol = "[r]" + r.ToString () + "[g]" + g.ToString () + "[b]" + b.ToString () + "[a]" + a.ToString ();
        return strCol;
	}
     public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath +"/"+ sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }
        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(Application.dataPath +"/Resources/"+ destDirName))
        {
            Directory.CreateDirectory(Application.dataPath +"/Resources/"+  destDirName);
        } else {
            Directory.Delete(Application.dataPath +"/Resources/"+  destDirName,true);        
            Directory.CreateDirectory(Application.dataPath +"/Resources/"+  destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string newFileName = file.Name;
            Directory.CreateDirectory(Application.dataPath + "/Resources/HotTotemAssets/EpicPrefs/Settings/");  
            if(newFileName.Contains("."))
                File.AppendAllText(Application.dataPath + "/Resources/HotTotemAssets/EpicPrefs/Settings/ResourcesFiles.txt", Path.Combine(destDirName, newFileName.Substring(0,newFileName.LastIndexOf(".")))+ Environment.NewLine);
            else        
                File.AppendAllText(Application.dataPath + "/Resources/HotTotemAssets/EpicPrefs/Settings/ResourcesFiles.txt", Path.Combine(destDirName, newFileName)+ Environment.NewLine);
            File.AppendAllText(Application.dataPath + "/Resources/HotTotemAssets/EpicPrefs/Settings/DestinationFiles.txt",Path.Combine(sourceDirName,newFileName) + Environment.NewLine);
            if(newFileName.Contains("."))
                newFileName = newFileName.Substring(0,newFileName.IndexOf(".")) + ".bytes";
            else 
                newFileName = newFileName + ".bytes";
            string temppath = Path.Combine(Application.dataPath +"/Resources/"+  destDirName, newFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(temppath));
            file.CopyTo(temppath, true);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                string tmpPath = Path.Combine(sourceDirName,subdir.Name);
                DirectoryCopy(tmpPath, temppath, copySubDirs);
            }
        }
    }
    public static void DeleteDirectory(string path,bool recursively){
        if(Directory.Exists(path))
            Directory.Delete(path,recursively);  
    }
    public static void setupPrefs(){
        if(!EpicPrefs.GetBool("HotTotemEpicPrefsInitialization",false)){
            EpicPrefs.SetBool("HotTotemEpicPrefsInitialization",true);
            TextAsset sourceTexts = Resources.Load("HotTotemAssets/EpicPrefs/Settings/ResourcesFiles") as TextAsset;
            TextAsset destTexts = Resources.Load("HotTotemAssets/EpicPrefs/Settings/DestinationFiles") as TextAsset;
            string[] source = sourceTexts.text.Split("\n"[0]);
            string[] destination = destTexts.text.Split("\n"[0]);
            for(int i = 0; i< source.Length;i++)
                copyPrefsToApplication(source[i],destination[i]);
        }
    }
    private static void copyPrefsToApplication(string fromPath,string toPath){
        TextAsset file = Resources.Load(fromPath) as TextAsset;
        if(file != null){
            Directory.CreateDirectory(@Path.GetDirectoryName(Application.persistentDataPath +"/"+ toPath));
            File.WriteAllBytes(Application.persistentDataPath +"/"+ toPath,file.bytes);   
        }   
    }
}
