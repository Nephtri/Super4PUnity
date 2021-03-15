using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer mainPlayer;
    public VideoPlayer subPlayer;
    public VideoPlayer tertiaryPlayer;
    public Camera mainCam;
    public Camera subCam;

    private float UpdateBuffer;
    public float UpdateBufferLimit;
    public bool IsLoaded;
    private bool IsPlaying;
    private bool IsLooping;
    private bool UseTertiaryPlayer;
    private float PrevVideoVol;

    public void Initialize()
    {
        IsLoaded = IsPlaying = IsLooping = false;
        mainPlayer.prepareCompleted += OnVideoPrepared;
        mainPlayer.loopPointReached += OnVideoFinished;
        subPlayer.prepareCompleted += OnVideoPrepared;
        subPlayer.loopPointReached += OnVideoFinished;
        tertiaryPlayer.prepareCompleted += OnVideoPrepared;
        tertiaryPlayer.loopPointReached += OnVideoFinished;
    }

    public void LoadNextVideo(string videoPath, string voicePath = "")
    {
        //First execution
        if(mainPlayer.url == string.Empty)
        {
            PreparePlayer(mainPlayer, videoPath);
            IsLooping = true;
        }
        //Video is already playing, set for loop and return
        else if(videoPath == mainPlayer.url) {
             IsLooping = true;
        }
        //Already loading video
        else if(UseTertiaryPlayer && videoPath != tertiaryPlayer.url)
        {
            PreparePlayer(tertiaryPlayer, videoPath);
            IsLooping = false;
        }
        else if(videoPath != subPlayer.url) {
            PreparePlayer(subPlayer, videoPath);
            IsLooping = false;
        }
        else {
            tertiaryPlayer.Stop();
            IsLooping = false;
        }
    }

    private void PreparePlayer(VideoPlayer videoPlayer, string videoPath)
    {
        videoPlayer.Stop();
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer videoPlayer)
    {
        // if(UseTertiaryPlayer && videoPlayer == tertiaryPlayer) { 
        //     SwapSubPlayer();
        //     return; 
        // }
        if(!IsLoaded)
        {
            IsLoaded = true;
            VideoController.VC.OnLoadComplete();
        }
    }

    private void OnVideoFinished(VideoPlayer videoPlayer)
    {
        if(!IsLooping)
        {
            if(UseTertiaryPlayer) {
                SwapSubPlayer();
            }

            SwapMainPlayer();
            UseTertiaryPlayer = true;

            //Required to keep videos in the same plane but give focus to main after pause
            mainPlayer.transform.SetAsLastSibling();
            mainPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

            UpdateBuffer = 0;
            MainController.MC.UpdateDelegate += HideSubVideoPlayer;

            mainPlayer.targetCamera = mainCam;
            
            subPlayer.Pause();
            mainPlayer.Play();
            
            SyncMainPlayer();

            IsLooping = VideoController.VC.CheckLooping();
            VideoController.VC.OnCycleComplete();
        }
        else {
            SyncMainPlayer();
            VideoController.VC.OnCycleComplete();
        }
    }

    private void HideSubVideoPlayer()
    {
        UpdateBuffer += Time.deltaTime;

        if(UpdateBuffer >= UpdateBufferLimit)
        {
            subPlayer.targetCamera = subCam;
            MainController.MC.UpdateDelegate -= HideSubVideoPlayer;
        }
    }

    public void PlayVideos()
    {
        if(IsPlaying) { return; }

        mainPlayer.Play();
        IsPlaying = true;
    }

    public void SyncMainPlayer()
    {
        var frameBuffer = (long)math.round(mainPlayer.frameCount * ((MainController.MC.MusicPlayer.time % 4)/4));
        mainPlayer.frame = frameBuffer > 60 ? mainPlayer.frame = 0 : mainPlayer.frame = frameBuffer + 5;
        
        if(frameBuffer >= 15 && frameBuffer <= 15)
        {
            Debug.Log("Desync Detected. Frames: " + frameBuffer);
        }
    }

    public void RevertVolume()
    {
        SetVolume(PrevVideoVol);
    }

    public float GetVolume()
    {
        return mainPlayer.GetDirectAudioVolume(0);
    }

    public void SetVolume(float volume)
    {
        PrevVideoVol = mainPlayer.GetDirectAudioVolume(0);

        mainPlayer.SetDirectAudioVolume(0, volume);
        subPlayer.SetDirectAudioVolume(0, volume);
        tertiaryPlayer.SetDirectAudioVolume(0, volume);

        GameDataManager.GM.PlyrConfigData.VideoVolume = volume;
    }

    private void SwapMainPlayer()
    {
        var tmpPlayer = mainPlayer;
        mainPlayer = subPlayer;
        subPlayer = tmpPlayer;
    }
    private void SwapSubPlayer()
    {
        var tmpPlayer = subPlayer;
        subPlayer = tertiaryPlayer;
        tertiaryPlayer = tmpPlayer;
    }
}