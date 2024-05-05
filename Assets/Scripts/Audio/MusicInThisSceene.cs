using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInThisSceene : MonoBehaviour
{
    public AudioClip Music;
    [Range(0,1)]public float volume = 1;
    void Start()
    {        
        AudioControl.Singleton.Playmusic(Music,volume);
    }

    
}
