using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

//Idea 1: Refresh functionality? On Title Screen click to reload mods, presets, etc...
//Refactor 1: Remove ParsedPresetData and func to separate data manager (only on title)
public class GameDataManager : MonoBehaviour {

    public PlayerConfigData PlyrConfigData;
    public Dictionary<string, string> ParsedPresetData;

    private string PLAYER_DIR = "/config";
    private string MODS_DIR = "/mods";
    private string MUSIC_DIR = "/mods/music";

    private readonly string PLAYERCONFIG_PATH = "/playerConfig.json";
    private readonly string PRESET_PATH = "/presets.json";

    public List<string> CharDirs;
    public List<string> SelectedMods;
    public List<string> SelectedMusic;

    public static bool IsPlayerDataLoaded;

    public static GameDataManager GM;
    
    void Awake() {
        //Singleton pattern
        if (GM == null) { GM = this; }
        else if (GM != this) { Destroy(gameObject); }
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        if(IsPlayerDataLoaded) { return; }
        
        //Set all Directories (for rest of runtime)
        
        #if UNITY_STANDALONE_OSX
            PLAYER_DIR = Application.dataPath + "/MacOS" + PLAYER_DIR;
            MODS_DIR = Application.dataPath + "/MacOS" + MODS_DIR;
            MUSIC_DIR = Application.dataPath +  "/MacOS" + MUSIC_DIR;
        #elif UNITY_ANDROID
            PLAYER_DIR = Application.persistentDataPath + PLAYER_DIR;
            MODS_DIR = Application.persistentDataPath + MODS_DIR;
            MUSIC_DIR = Application.persistentDataPath + MUSIC_DIR;

            if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
            if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)) {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
        #else
            PLAYER_DIR = Application.dataPath + "/.." + PLAYER_DIR;
            MODS_DIR = Application.dataPath + "/.." + MODS_DIR;
            MUSIC_DIR = Application.dataPath +  "/.." + MUSIC_DIR;
        #endif 
    
        var playerConfigFilePath = PLAYER_DIR + PLAYERCONFIG_PATH;

        if(!DataUtility.CheckDirectoryExists(PLAYER_DIR)) {  Directory.CreateDirectory(PLAYER_DIR); }

        if(!DataUtility.CheckFileExists(playerConfigFilePath, false))
        {
            PlyrConfigData = CreateInitialPlyrCfgData();
            SavePlyrCfgData();
        }
        else { 
            PlyrConfigData = DataUtility.LoadFromJSON<PlayerConfigData>(playerConfigFilePath, false); 
            ValidatePlyrCfgData();
        }

        //Set Directory Paths
        //TODO [Android]: Need to make DIRs non-static to support multiplatform & set based on user input for Android
        try
        {
            CharDirs = Directory.GetDirectories(MODS_DIR, "CHAR_*").ToList();
        }
        catch(Exception)
        {
            Debug.LogWarning("Mods folder not found (char search): " + MODS_DIR);
        }

        
        // else
        // {
        //     if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) && !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        //     {
        //         // Ask for permission or proceed without the functionality enabled.
        //         Permission.RequestUserPermission(Permission.ExternalStorageRead);
        //         Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        //     }   
        //     if(!Directory.Exists(GameDataManager.GM.GetAndroidInternalFilesDir() + "/.4PUnity_V2")) {
        //         Directory.CreateDirectory(GameDataManager.GM.GetAndroidInternalFilesDir() + "/.4PUnity_V2");
        //     }
        // }

        IsPlayerDataLoaded = true;
    }
    public string GetMusicPath(string musicName)
    {
        return MUSIC_DIR + "/" + musicName;
    }

    public List<string> GetMusicFilePaths()
    {
        var musicFilePaths = new List<string>();

        try
        {
            musicFilePaths.AddRange(Directory.GetFiles(MUSIC_DIR, "*.ogg"));
            musicFilePaths.AddRange(Directory.GetFiles(MUSIC_DIR, "*.mp3"));
        }
        catch(Exception)
        {
            Debug.LogWarning("Mods folder not found (music search): " + MODS_DIR);
        }
        return musicFilePaths;
    }

    //Set Music based on results of Title Screen
    public void SetSelectedMusic(string selectedMusicStr)
    {
        SelectedMusic = selectedMusicStr.Split(',').OrderBy(x => x).ToList();

        if(SelectedMusic.Any(x => x.Contains("BBSa_")))
        {
            var prioritizedMusic = SelectedMusic.Where(x => x.Contains("BBSa_")).ToList();
            foreach(var music in prioritizedMusic)
            {
                SelectedMusic.Remove(music);
            }
            SelectedMusic.InsertRange(0, prioritizedMusic);
        }
    }

    //Set Mods based on results of Title Screen
    public void SetSelectedMods(string selectedModStr)
    {
        SelectedMods = selectedModStr.Split(',').OrderBy(x => x).ToList();
    }

    public string GetModsPath()
    {
        return MODS_DIR;
    }
    public string GetMusicPath()
    {
        return MUSIC_DIR;
    }

    public List<string> GetVideoPaths(string modName)
    {
        var videoPaths = Directory.GetFiles(MODS_DIR + "/CHAR_" + modName, "*.mp4").ToList();
        videoPaths.AddRange(Directory.GetFiles(MODS_DIR + "/CHAR_" + modName, "*.m4v"));
        return videoPaths;
    }

    public void LoadPresetData()
    {
        ParsedPresetData = new Dictionary<string, string>();
        var playerPresetPath = PLAYER_DIR + PRESET_PATH;

        if(!DataUtility.CheckFileExists(playerPresetPath, false)) {
            SavePresetData();
        }
        else { 
            //TODO: Don't store this permanently, not needed
            var presetData = DataUtility.LoadFromJSON<PresetData>(playerPresetPath, false); 
            if(string.IsNullOrWhiteSpace(presetData.Presets)) { return; }
            
            foreach(var preset in presetData.Presets.Split('|'))
            {
                var innerPreset = preset.Split('-');
                var presetKey = innerPreset[0].Trim();
                var presetValue = innerPreset[1].Trim();

                ParsedPresetData.Add(presetKey, presetValue);
            }
        }
    }

    public void SavePresetData()
    {
        var presetData = new PresetData();
        bool isFirstIteration = true;

        foreach(var key in ParsedPresetData.Keys)
        {
            if(!isFirstIteration) { presetData.Presets += "|"; }
            presetData.Presets += key + "-" + ParsedPresetData[key];

            if(isFirstIteration) { isFirstIteration = false; }
        }

        DataUtility.SaveToJSON(PLAYER_DIR + PRESET_PATH, presetData, false);
    }

    public void AddPresetData(string presetName, string modListStr, string musicListStr)
    {
        if(ParsedPresetData.ContainsKey(presetName)) { ParsedPresetData[presetName] = modListStr; }
        else { ParsedPresetData.Add(presetName, modListStr + '>' + musicListStr); }

        SavePresetData();
    }

    public void DeletePresetData(string presetName)
    {
        ParsedPresetData.Remove(presetName);
        SavePresetData();
    }

    public void SavePlyrCfgData()
    {
        DataUtility.SaveToJSON(PLAYER_DIR + PLAYERCONFIG_PATH, PlyrConfigData, false);
    }

    private PlayerConfigData CreateInitialPlyrCfgData()
    {
        return new PlayerConfigData()
        {
            AspectResolution = AspectResolution.SevenTwenty,
            IsFullscreen = false,
            MusicVolume = 1f,
            VideoVolume = 1f,
            VoiceVolume = 1f,
            CharPlaybackStyle = PlaybackStyle.Loop,
            PosePlaybackStyle = PlaybackStyle.Loop,
            Version = Application.version
        };
    }
    
    //Check if PlyrConfigData has null values, if so
    private void ValidatePlyrCfgData()
    {
        PlayerConfigData initialPlyrConfig = null;

        foreach(var field in PlyrConfigData.GetType().GetFields())
        {
            if(field.GetValue(PlyrConfigData) == null) {
                if(initialPlyrConfig == null) {
                    initialPlyrConfig = CreateInitialPlyrCfgData();
                }
                field.SetValue(PlyrConfigData, field.GetValue(initialPlyrConfig));
            }
        }

        PlyrConfigData.Version = Application.version;
    }

    public string GetAndroidInternalFilesDir()
    {
        string[] potentialDirectories = new string[]
        {
            "/mnt/sdcard",
            "/sdcard",
            "/storage/sdcard0",
            "/storage/sdcard1",
            "/storage/emulated/0"
        };

        if (Application.platform == RuntimePlatform.Android)
        {
            for (int i = 0; i < potentialDirectories.Length; i++)
            {
                if (Directory.Exists(potentialDirectories[i]))
                {
                    return potentialDirectories[i];
                }
            }
        }
        return "";
    }   
}