using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class AudioLibrary : MonoBehaviour
{
    public List<AudioClip> AudioList;

    public static AudioLibrary AL;

    void Awake()
    {
        //Singleton pattern
        if (AL == null) {
            AL = this;
        }
        else if (AL != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public AudioClip GetAudioClipByName(string name)
    {
        return AudioList.Find(x => x.name == name);
    }
}