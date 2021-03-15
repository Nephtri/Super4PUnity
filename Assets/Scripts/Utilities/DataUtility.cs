using UnityEngine;
using System.IO;

public static class DataUtility
{
    public static bool CheckDirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public static bool CheckFileExists(string path, bool isPersistentPath)
    {
        var absPath = (isPersistentPath ? Application.persistentDataPath : string.Empty) + path;
        return File.Exists(absPath);
    }

    public static T LoadFromJSON<T>(string path, bool isPersistentPath)
    {
        var absPath = (isPersistentPath ? Application.persistentDataPath : string.Empty) + path;

        if(File.Exists(absPath)) {
            var jsonData = File.ReadAllText(absPath);
            return JsonUtility.FromJson<T>(jsonData);
        } else {
            Debug.Log("Error! Could not load JSON from path: " + absPath);
            return default(T);
        }
    }

    public static void SaveToJSON(string path, object dataObj, bool isPersistentPath)
    {
        var absPath = (isPersistentPath ? Application.persistentDataPath : string.Empty) + path;
        var jsonData = JsonUtility.ToJson(dataObj);
        File.WriteAllText(absPath, jsonData);
    }
}