using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public GameState GS_Current;
    public AudioSource MusicPlayer;
    public AudioSource VideoAudioPlayer;
    public AudioSource VoiceAudioPlayer;
    public AudioSource SFXAudioPlayer;
    public InputSystemUIInputModule UIInputModule;

    public delegate void OnUpdateDelegate();
    public event OnUpdateDelegate UpdateDelegate;
    private bool HasRegainedFocus;

    public int PrevSceneIndex;

    public static MainController MC;

    void Awake()
    {
        //Singleton pattern
        if (MC == null) { MC = this; }
        else if (MC != this) { Destroy(gameObject); }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        var GM = GameDataManager.GM;
        GM.Initialize();
        
        //All errors/logs will now go to logger
        Logger.LG.Initialize();

        SwitchResolution((int)GM.PlyrConfigData.AspectResolution);

        MusicPlayer.volume = GM.PlyrConfigData.MusicVolume;
        VoiceAudioPlayer.volume = GM.PlyrConfigData.VoiceVolume;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(UpdateDelegate != null) { UpdateDelegate(); }
    }

    public void Pause()
    {
        MusicPlayer.Pause();
        GS_Current = GameState.MenuOpen;
    }

    public void Unpause()
    {
        if(MusicPlayer.clip != null) { MusicPlayer.Play(); }
        GS_Current = GameState.Active;
    }

    // public void GoToScene(int sceneIndex, bool stopMusic = true)
    // {
    //     //if(stopMusic) { StopMusic(); }
        
    //     MainController.MC.GS_Current = GameState.Loading;
    //     var sceneLoader = SceneManager.LoadSceneAsync(sceneIndex);
    //     StartCoroutine(SceneLoadCoroutine(sceneLoader));
    // }

    public void GoToScene(string sceneName, bool stopMusic = true)
    {
        if(stopMusic) { StopMusic(); }
        
        MainController.MC.GS_Current = GameState.Loading;
        var sceneLoader = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(SceneLoadCoroutine(sceneLoader));
    }

    private IEnumerator SceneLoadCoroutine(AsyncOperation sceneLoader)
    {
        sceneLoader.allowSceneActivation = false;

        while(sceneLoader.progress < 0.9f) {
            yield return null;
        }

        foreach(var objPooler in FindObjectsOfType<ObjectPooler>()) {
            objPooler.RepoolAllObjects();
        }

        sceneLoader.allowSceneActivation = true;

        while(!sceneLoader.isDone) {
            yield return null;
        }

        if(UIInputModule == null) {
            UIInputModule = GameObject.FindObjectOfType<InputSystemUIInputModule>();
        }
        //Unpause();
    }

    public void StopMusic()
    {
        MusicPlayer.time = VoiceAudioPlayer.time = 0;
        MusicPlayer.Stop();
        VoiceAudioPlayer.Stop();
        MusicPlayer.clip = VoiceAudioPlayer.clip = null;
    }

    public void SwitchResolution(int res)
    {
        var aspectRes = (AspectResolution)res;
        var isFullscreen = GameDataManager.GM.PlyrConfigData.IsFullscreen;

        switch(aspectRes) {
            case AspectResolution.FiveSeventy:
                SwitchResolutionChecker(1024, 576, isFullscreen);
                break;
            case AspectResolution.SevenTwenty:
                SwitchResolutionChecker(1280, 720, isFullscreen);
                break;
            case AspectResolution.SevenSixty:
                SwitchResolutionChecker(1366, 768, isFullscreen);
                break;
            case AspectResolution.NineHundred:
                SwitchResolutionChecker(1600, 900, isFullscreen);
                break;
            case AspectResolution.TenEighty:
                SwitchResolutionChecker(1920, 1080, isFullscreen);
                break;
            case AspectResolution.ElevenForty:
                SwitchResolutionChecker(2560, 1140, isFullscreen);
                break;
            case AspectResolution.FourK:
                SwitchResolutionChecker(3840, 2160, isFullscreen);
                break;
        }
        GameDataManager.GM.PlyrConfigData.AspectResolution = aspectRes;
    }

    private void SwitchResolutionChecker(int width, int height, bool isFullscreen)
    {
        if(Screen.width != width || Screen.height != height || isFullscreen != Screen.fullScreen)
        {
            if(isFullscreen) {
                width = Screen.currentResolution.width;
                height = Screen.currentResolution.height;
            }
            Screen.SetResolution(width, height, isFullscreen);
        }
    }

    public void ToggleFullScreen(bool isFullscreen)
    {
        if(Screen.fullScreen != isFullscreen) 
        { 
            GameDataManager.GM.PlyrConfigData.IsFullscreen = isFullscreen;

            if(isFullscreen && Screen.height != Screen.currentResolution.height) {
                SwitchResolutionChecker(Screen.currentResolution.width, Screen.currentResolution.height, isFullscreen);
            } 
            else if(!isFullscreen) {
                SwitchResolution((int)GameDataManager.GM.PlyrConfigData.AspectResolution);
            }
            else {
                Screen.fullScreen = isFullscreen; 
            }
        }
    }

    public void SetState(GameState gameState)
    {
        MainController.MC.GS_Current = gameState;
        if(UIInputModule == null) {
            UIInputModule = GameObject.FindObjectOfType<InputSystemUIInputModule>();
        }
        
        if(gameState == GameState.Loading) {
            UIInputModule.enabled = false;
        } else {
            UIInputModule.enabled = true;
        }
    }

    //Stop clicks from registering when re-entering window
    void OnApplicationFocus(bool focusStatus) {
        if(UIInputModule != null) {
            UIInputModule.enabled = focusStatus;
        }
    }

    //Save Data automatically when game closes
    void OnApplicationQuit() 
    {
        var plyrConfigData = GameDataManager.GM.PlyrConfigData;

        plyrConfigData.MusicVolume = MusicPlayer.volume;
        plyrConfigData.VoiceVolume = VoiceAudioPlayer.volume;
            
        GameDataManager.GM.SavePlyrCfgData();
    }
}
