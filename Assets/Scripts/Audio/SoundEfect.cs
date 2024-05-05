using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEfect : MonoBehaviour {
    public AudioClip snd;
    public AudioClip[] otherSounds;
    public bool is3d;
    public enum Type { Start, Destroy, JustReference }

    public Type type;

    // Use this for initialization
    void Start () {
        if (type.ToString() == "Start") Play();
	}

	// Update is called once per frame
	void OnDestroy () {
        if (type.ToString() == "Destroy") Play();
    }

    public void Play()
    {
        if (is3d) play3D(snd);
        else play2D(snd);
    }

    void play3D(AudioClip clip)
    {
        AudioControl.Singleton.PlaySound3D(clip, transform.position);
    }
    void play2D(AudioClip clip)
    {
        AudioControl.Singleton.PlaySound(clip);
    }
    void PlayOther(int id)
    {
        if (is3d) play3D(otherSounds[id]);
        else play2D(otherSounds[id]);
    }
}
