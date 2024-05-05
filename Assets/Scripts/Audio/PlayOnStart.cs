using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnStart : MonoBehaviour {
    public AudioClip snd;
    public bool is3D;
    public bool onEnable;
    [Range(0, 1)] public float volume;
	// Use this for initialization
	void Start () {
        play();
    }
    private void OnEnable()
    {
        if (onEnable) play();
    }

    void play()
    {
        AudioSource audio;
        if (is3D)
        {
            audio = AudioControl.Singleton.PlaySound3D(snd, transform.position);
        }
        else
        {
            GameObject O = (GameObject)AudioControl.Singleton.PlaySound(snd);
            audio = O.GetComponent<AudioSource>();
        }
        audio.volume *= volume;
    }
}
