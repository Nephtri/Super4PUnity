using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class VideoController : InputController
{
    public Canvas VideoControlsUI;

    public MusicPanel MusicPanel;
    public CharacterPanel CharacterPanel;
    public PosePanel PosePanel;
    public HelpPanel HelpPanel;
    public VideoManager VideoManager;

    public bool IsLoaded;

    public int ActiveCharIndex;
    public int ActivePoseIndex;
    public int SelectedCharIndex;
    public int NextCharIndex;
    public int NextPoseIndex;

    private int CharCount;
    public int ActivePoseCount;
    public int NextPoseCount;
    private int SelectedPoseCount;

    public static VideoController VC;

    protected virtual void Awake()
    {
        //Singleton pattern
        if (VC == null) {
            VC = this;
        }
        else if (VC != this) {
            Destroy(VC);
            VC = this;
        }     
    }

    override protected void Start()
    {
        MainController.MC.SetState(GameState.Loading);
        base.Start();

        NextCharIndex = NextPoseIndex = -1;
        HelpPanel.Initialize("SMW-PrincessHelp");

        //Load Music and Audio
        MusicPanel.Initialize();
        StartCoroutine(MusicPanel.LoadMusic());

        //TODO: Move VideoManager below these and set initial load to one video if only one in characters
        //Initialize Videoplayers, Pose Panel (empty), and Character Panel (selects & sets next)
        VideoManager.Initialize();
        PosePanel.Initialize();
        CharCount = CharacterPanel.Initialize(); //Sets SelectedCharIndex


        CharacterPanel.SetNextCharacter(SelectedCharIndex, false);
        SetNextPoseIndex(0);
        PosePanel.OnSelectItem(NextPoseIndex, true); //Results in loading 1st video
    
        OnCycleComplete();
        SelectedCharIndex = -1;
    }

    public void OnLoadComplete()
    {
        if(!MusicPanel.IsLoaded || !VideoManager.IsLoaded) { return; }

        MainController.MC.SetState(GameState.Active);
        MusicPanel.PlayMusic();
        VideoManager.PlayVideos();
        IsLoaded = true;
    }

    //Current video just ended (or first cycle started)
    public void OnCycleComplete()
    {
        //Set ActiveCharIndex (ActivePoseCount) and ActivePoseIndex
        CharacterPanel.SetActivecharacter(NextCharIndex);
        ActivePoseIndex = NextPoseIndex;

        //If poses shown are for active character, update styling
        if(SelectedCharIndex == ActiveCharIndex || SelectedCharIndex == -1) {
            PosePanel.SetActivePose(ActivePoseIndex);
        }

        //Set NextCharacterIndex and NextPoseIndex
        OnToggleCharStyle();
    }

    public void OnToggleCharStyle()
    {
        var isCharChanged = false;

        if(PosePanel.PlaybackStyle == PlaybackStyle.Order)
        {
            if(ActivePoseIndex >= ActivePoseCount - 1) 
            {
                isCharChanged = CharacterPanel.SetNextCharacter(-1, false);
                //OnCharacterSelected(ActivePoseCount, false);
            }
            else {
                isCharChanged = CharacterPanel.SetNextCharacter(ActiveCharIndex, false);
            }
        }
        else {
            isCharChanged = CharacterPanel.SetNextCharacter(-1, false);
        }

        if(isCharChanged) {
            OnCharacterSelected(ActivePoseCount, false);
        }
        SetNextPoseIndex();
        // if(!CheckLooping()) {
            
        // }
        // SetNextPoseIndex(ActivePoseIndex >= NextPoseCount - 1 ? 0 : -1);
    }

    public void OnTogglePoseStyle()
    {
        //If Poses in order but not looping character, don't change character if not at end
        if(PosePanel.PlaybackStyle == PlaybackStyle.Order && CharacterPanel.PlaybackStyle != PlaybackStyle.Loop
            && ActivePoseIndex < ActivePoseCount - 1)
        {
            CharacterPanel.SetNextCharacter(ActiveCharIndex, false);
        }
        else if(PosePanel.PlaybackStyle != PlaybackStyle.Order && CharacterPanel.PlaybackStyle != PlaybackStyle.Loop)
        {
            CharacterPanel.SetNextCharacter(-1, false);
        }
        else
        {
            CharacterPanel.SetNextCharacter(NextCharIndex, false);
        }
        SetNextPoseIndex();
        PosePanel.StyleBtns((SelectedCharIndex == -1 || SelectedCharIndex == ActiveCharIndex), NextCharIndex == ActiveCharIndex);
        SelectedCharIndex = -1;
    }

    //Before this, NextCharIndex should be set!
    public void OnSetNextCharIndex()
    {
        SetNextPoseIndex();
    }

    public void SetNextPoseIndex(int index = -1)
    {
        if(index == -1)
        {
            index = ActivePoseIndex;
            switch(PosePanel.PlaybackStyle)
            {
                case(PlaybackStyle.Order):
                    if(index >= ActivePoseCount - 1) { index = 0; }
                    else { index++; }
                    break;

                case(PlaybackStyle.Random):
                    index = UnityEngine.Random.Range(0, NextPoseCount);
                    break;
            }
        }
        if(index >= NextPoseCount) { index = 0; }

        NextPoseIndex = index;
        
        VideoManager.LoadNextVideo(CharacterPanel.GetVideoPath(NextPoseIndex));

        if(SelectedCharIndex == ActiveCharIndex 
            || (SelectedCharIndex == -1 && NextCharIndex == ActiveCharIndex)) 
        { 
            PosePanel.SetNextPose(NextPoseIndex); 
        }
    }

    //Before this, SelectedCharIndex should be set!
    public void OnCharacterSelected(int poseCount, bool isManual = true)
    {
        if(isManual)
        {
            SelectedPoseCount = poseCount;
            PosePanel.CreatePoses(poseCount);
            PosePanel.StyleBtns(SelectedCharIndex == ActiveCharIndex, SelectedCharIndex == NextCharIndex);
        }
        else
        {
            PosePanel.CreatePoses(poseCount);
        }
    }

    //Set our next character based on the one currently selected
    public void OnPoseSelected(int index)
    {
        NextPoseIndex = index;
        CharacterPanel.SetNextCharacter(SelectedCharIndex > -1 ? SelectedCharIndex : ActiveCharIndex, false);
        PosePanel.StyleBtns(NextCharIndex == ActiveCharIndex, false);
        CharacterPanel.ClearSelectedItem();
        SelectedCharIndex = -1;
        VideoManager.LoadNextVideo(CharacterPanel.GetVideoPath(NextPoseIndex));
    }

    // private void Update() {
    //     var audioFrame = (long)math.round(VideoManager.mainPlayer.frameCount * ((MainController.MC.MusicPlayer.time % 4)/4));
    //     Debug.Log("Desync Frames: " + (VideoManager.mainPlayer.frame - audioFrame));
    // }

    public bool CheckLooping()
    {
        return CharacterPanel.PlaybackStyle == PlaybackStyle.Loop && PosePanel.PlaybackStyle == PlaybackStyle.Loop;
    }

    public void SetVideoVolume(float volume)
    {
        VideoManager.SetVolume(volume);
    }

    public void OnHelpClick()
    {
        HelpPanel.OnToggleHelp();
    }

    //--Player Input Events--
    override public void OnSwitchMusicPrev(CallbackContext context)
    {
        if(!context.performed || !IsLoaded) { return; }
        MusicPanel.NextMusicTrack(-1);
    }

    override public void OnSwitchMusicNext(CallbackContext context)
    {
        if(!context.performed || !IsLoaded) { return; }
        MusicPanel.NextMusicTrack(1);
    }

    override public void OnToggleMusic(CallbackContext context)
    {
        if(!context.performed || !IsLoaded) { return; }
        if(MainController.MC.MusicPlayer.volume == 0) {
            MusicPanel.RevertMusicVolume();
        } else {
            MusicPanel.SetMusicVolume(0);
        }
    }

    override public void OnToggleVideoSFX(CallbackContext context)
    {
        if(!context.performed || !IsLoaded) { return; }
        if(VideoManager.GetVolume() == 0) {
            VideoManager.RevertVolume();
        } else {
            VideoManager.SetVolume(0);
        }
    }

    override public void OnHideUI(CallbackContext context)
    {
        if(!context.performed || HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }

        var value = !CharacterPanel.isActiveAndEnabled;
        CharacterPanel.gameObject.SetActive(value);
        PosePanel.gameObject.SetActive(value);
        MusicPanel.gameObject.SetActive(value);
    }

    override public void OnAnyKey(CallbackContext context)
    {
        if(!context.performed || !IsLoaded || !HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        HelpPanel.OnToggleHelp();
    } 


    override public void OnToggleHelp(CallbackContext context)
    {
        if(!context.performed || !IsLoaded || HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        HelpPanel.OnToggleHelp();
    } 

    override public void OnFullscreen(CallbackContext context)
    {
        if(!context.performed || HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        MainController.MC.ToggleFullScreen(!Screen.fullScreen);
    } 

    override public void OnEscape(CallbackContext context)
    {
        if(!context.performed || !IsLoaded || HelpPanel.IsHelpOpen || !HelpPanel.IsHelpTransitionComplete) { return; }
        if(VideoControlsUI.gameObject.activeSelf) { VideoControlsUI.gameObject.SetActive(false); } 
        MainController.MC.GoToScene("Title");
    }
}