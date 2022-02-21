using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//A class to save & load file
public class FileManager
{
    public void Save(string fileName, object target, string dir)
    {
        var serializeData = JsonUtility.ToJson(target);
        var filePath = Application.dataPath + dir;

        Directory.CreateDirectory(filePath);
        File.WriteAllText(filePath + fileName + ".sav", serializeData);
    }

    public PlayerData Load(string path, string name)
    {
        var filePath = Application.dataPath + path + name + ".sav";
        var deserializeData = (string)(null);

        try
        {
            deserializeData = File.ReadAllText(filePath);
        }
        catch (System.IO.FileNotFoundException)
        {
            return null;
        }
        catch (System.IO.DirectoryNotFoundException)
        {
            Directory.CreateDirectory(Application.dataPath + path);
            return null;
        }

        return JsonUtility.FromJson<PlayerData>(deserializeData);
    }

    public List<string> LoadDirFiles(string dir, string type)
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + dir);
        List<string> fileList = new List<string>();

        if (di.GetFiles(type).Length == 0)
        {
            return fileList;
        }
        else
        {
            foreach (var f in di.GetFiles(type))
            {
                fileList.Add(f.Name);
            }
        }

        return fileList;
    }

}
