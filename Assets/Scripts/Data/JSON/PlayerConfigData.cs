using System;

[Serializable]
public class PlayerConfigData
{
    public AspectResolution AspectResolution = AspectResolution.SevenTwenty;
    public bool IsFullscreen = false;
    
    public float MusicVolume = 1f;
    public float VideoVolume = 1f;
    public float VoiceVolume = 1f;

    public PlaybackStyle CharPlaybackStyle = PlaybackStyle.Loop;
    public PlaybackStyle PosePlaybackStyle = PlaybackStyle.Loop;

    public string Version;
}