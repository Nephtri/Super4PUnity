using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class Logger : MonoBehaviour 
{
    private readonly string LOG_DIR = "/../logs";
    private readonly int LOG_LIMIT = 150;
    private string logFile;
    private bool HasCheckedLimit;
    public static Logger LG;
    
    void Awake() {
        //Singleton pattern
        if (LG == null) { LG = this; }
        else if (LG != this) { Destroy(gameObject); }
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        var logDirPath = Application.dataPath + LOG_DIR;

        //Create Directory Paths
        if (Application.platform != RuntimePlatform.Android)
        {
            if(!DataUtility.CheckDirectoryExists(logDirPath)) {  Directory.CreateDirectory(logDirPath); }
        } 
        else
        {
            if (!Directory.Exists(GameDataManager.GM.GetAndroidInternalFilesDir() + "/.4PUnity/logs")) {
                Directory.CreateDirectory(GameDataManager.GM.GetAndroidInternalFilesDir() + "/.4PUnity/logs");
            }
        }
        Application.logMessageReceived += WriteToLog;
    }

    //Write directly to log file (create if none so far)
    public void WriteToLog(string logString, string stackTrace, LogType type)
    {
        var logDirPath = Application.dataPath + LOG_DIR;

        if(!HasCheckedLimit) {
            var allLogs = Directory.GetFiles(logDirPath);
            if(allLogs.Length >= LOG_LIMIT) { File.Delete(allLogs.First()); }
            HasCheckedLimit = true;
        }

        if(logFile == null) { logFile = logDirPath + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"; }

        File.AppendAllText(logFile, 
            Enum.GetName(typeof(LogType), type) + logString + "\r\n" + 
            stackTrace + 
            "------------------------------------------------------------\r\n");
    }
}