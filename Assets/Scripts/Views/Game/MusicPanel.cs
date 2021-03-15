using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Loads static content (music, characters, voice) & Populates UI elements
public class MusicPanel : MonoBehaviour
{
    public GameObject MainPanel;
    public Dropdown MusicDropdown;
    public Slider MusicSlider;
    public Slider VideoSlider;
    private List<AudioClip> MusicList;
    private int TrackIndex;
    private float PrevMusicVol;
    private bool IsVisible;
    public bool IsLoaded;
    private readonly int PANEL_HEIGHT = 80;

    public void Initialize()
    {
        MusicList = new List<AudioClip>();

        MusicSlider.value = MainController.MC.MusicPlayer.volume;
        VideoSlider.value = GameDataManager.GM.PlyrConfigData.VideoVolume;

        TrackIndex = 0;
        IsVisible = true;
        IsLoaded = false;
    }

    public void RevertMusicVolume()
    {
        SetMusicVolume(PrevMusicVol);
    }
    public void SetMusicVolume(float volume)
    {
        PrevMusicVol = MainController.MC.MusicPlayer.volume;
        MainController.MC.MusicPlayer.volume = volume;
    }
    public void SetVideoVolume(float volume)
    {
        VideoController.VC.SetVideoVolume(volume);
    }

    //Loads music, puts into dropdown, fills in cliplist for Ctrl
    public IEnumerator LoadMusic()
    {
        List<string> trackNames = new List<string>();
        
        foreach(string musicFile in GameDataManager.GM.SelectedMusic)
        {
            var trackPath = musicFile.Replace('\\', '/');
            AudioType audioType;
            switch(trackPath.Substring(trackPath.Length-3))
            {
                case("mp3"):
                    audioType = AudioType.MPEG;
                    break;

                default:
                    audioType = AudioType.OGGVORBIS;
                    break;
            }
            
            var trackFullPath = GameDataManager.GM.GetMusicPath(trackPath);
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + trackFullPath, audioType);
            request.SendWebRequest();

            //Iterate until finished loading music
            while (request.downloadProgress < 1) { yield return request; }
           
           var audioClip = ((DownloadHandlerAudioClip)request.downloadHandler).audioClip;
           MusicList.Add(audioClip);

            trackNames.Add(trackPath.Substring(0, trackPath.Length - 4));

            // var pathEndIndex = trackPath.LastIndexOf("/") + 1;
            // var trackName = trackPath.Substring(pathEndIndex, trackPath.Length - pathEndIndex - 4);
            // trackNames.Add(trackName);
        }

        MusicDropdown.ClearOptions();
        MusicDropdown.AddOptions(trackNames);

        IsLoaded = true;
        VideoController.VC.OnLoadComplete();
    }

    public void PlayMusic()
    {
        MainController.MC.MusicPlayer.Stop();
        MainController.MC.MusicPlayer.clip = MusicList[MusicDropdown.value];
        MainController.MC.MusicPlayer.time = 0;
        MainController.MC.MusicPlayer.Play();
    }

    public void SetMusicTrack(int index)
    {
        var musicPlayer = MainController.MC.MusicPlayer;

        var time = musicPlayer.time;
        musicPlayer.clip = MusicList[index];
        musicPlayer.time = time;
        musicPlayer.Play();

        TrackIndex = index;
    }

    public void NextMusicTrack(int delta)
    {
        if(TrackIndex + delta < 0) { TrackIndex = MusicList.Count-1; }
        else if(TrackIndex + delta >= MusicList.Count) { TrackIndex = 0; }
        else { TrackIndex += delta; }

        MusicDropdown.value = TrackIndex;
    }

    public void ToggleMusicPanel()
    {
        var rect = MainPanel.GetComponent<RectTransform>();
        if (IsVisible)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x, rect.offsetMin.y - PANEL_HEIGHT);
            rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y - PANEL_HEIGHT);
        } else
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x, rect.offsetMin.y + PANEL_HEIGHT);
            rect.offsetMax = new Vector2(rect.offsetMax.x, rect.offsetMax.y + PANEL_HEIGHT);
        }
        IsVisible = !IsVisible;
    }
}