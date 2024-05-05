using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour {

	public static AudioControl Singleton;
	public GameObject SoundPrefab;
	public GameObject MusicPrefab;
	public GameObject CurrentMusic;
	public music[] MusicList;    
    bool observing;
    GameObject insertSound;
    float targetVolume;
	List<soundAndPitch> soundQueue;
	GameObject playingQueued;

    struct soundAndPitch
    {
        public AudioClip clip;
        public float pitch;
        public float time;
    }

    [Serializable]
    public struct music
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume;
    }

	[Range(0.0f, 1.0f)]
	public float MasterVolume,MusicVolume,FXVolume,VoiceVolume;
	int i = 0;

	void Awake() {
        soundQueue = new List<soundAndPitch>();
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("multiplos Controladores de audio, Auto Delete");
            Destroy(this.gameObject);
        }
    }


	void Update () {
		if(observing)
        {
            if(insertSound==null)
            {
                observing = false;
                CurrentMusic.GetComponent<Music>().StartFade(1,0, MasterVolume * MusicVolume);
            }
        }
		if(soundQueue.Count>0)
		{
			if(playingQueued==null){
                if(soundQueue[0].time+0.5f>Time.time || soundQueue[0].clip.length > 0.7f)
                {
                    playingQueued = PlayVoice(soundQueue[0].clip, soundQueue[0].pitch);
                    soundQueue.RemoveAt(0);
                }
                else
                {
                    soundQueue.RemoveAt(0);
                }
                
			}
		}
	}
    //Toca som global uma vez
	public GameObject PlaySound(AudioClip Clip)
	{
        if(Clip!=null)
        {
            GameObject newSound;
            newSound = Instantiate(SoundPrefab, this.transform);
            AudioSource As = newSound.GetComponent<AudioSource>();
            SetFx(Clip, As);
            As.spatialBlend = 0;
            As.volume = FXVolume * MasterVolume;
            return newSound;
        }
        return null;
        
    }
    public GameObject PlayVoice(AudioClip Clip,float pitch)
    {
        GameObject newSound;
        newSound = Instantiate(SoundPrefab, this.transform);
        AudioSource As = newSound.GetComponent<AudioSource>();
        SetFx(Clip, As);
        As.spatialBlend = 0;
        As.volume = VoiceVolume * MasterVolume;
        As.pitch = pitch;
        return newSound;
    }
    //toca som 3D uma vez
    public AudioSource PlaySound3D(AudioClip Clip,Vector3 Position)
	{
        if (Clip != null)
        {
            GameObject newSound;
            newSound = Instantiate(SoundPrefab, Position, new Quaternion(0, 0, 0, 0));
            SetFx(Clip, newSound.GetComponent<AudioSource>());
            return newSound.GetComponent<AudioSource>();
        }
        return null;
	}

    public void Playmusic(int musicId)
    {
        if (musicId < MusicList.Length)
        {
            Playmusic(MusicList[musicId]);
        }        
    }

    // toca música com transição entre a música anterior
	public void Playmusic(music music)
	{
        Playmusic(music.clip, music.volume);
	}
    public void Playmusic (AudioClip clip, float musicVolume)
    {
        bool needChangeMusic = true;
        if (CurrentMusic != null)
        {
            var oldmusic = CurrentMusic.GetComponent<Music>();
            if (oldmusic.audioS.clip != clip)
            {
                CurrentMusic.GetComponent<Music>().StartFade(1, MasterVolume * MusicVolume, 0);
            }
            else
            {
                needChangeMusic = false;
            }
        }
        if (needChangeMusic)
        {
            GameObject newmusic;
            newmusic = Instantiate(MusicPrefab, this.transform);
            newmusic.GetComponent<AudioSource>().clip = clip;
            CurrentMusic = newmusic;
            newmusic.GetComponent<Music>().StartFade(3, 0, MasterVolume * MusicVolume * musicVolume);
            newmusic.transform.parent = this.transform;
            newmusic.GetComponent<AudioSource>().Play();
        }
    }
    //Pausa a música enquanto um som novo é tocado
    public void InsertSound(AudioClip snd)
    {
        CurrentMusic.GetComponent<Music>().StartFade(1, MasterVolume * MusicVolume, 0);
        insertSound = PlaySound(snd);
        Music M = insertSound.AddComponent<Music>();
        M.StartFade(0.5f, 0, FXVolume * MasterVolume);
        observing = true;
    }

    void SetFx(AudioClip clip, AudioSource FX)
    {
        FX.clip = clip;
        FX.volume = MasterVolume * FXVolume;
    }
    public void changeMusicVolume()
    {
        if (CurrentMusic != null)
        {
            var AS = CurrentMusic.GetComponent<AudioSource>();
            if (AS != null)
            {
                AS.volume = MusicVolume * MasterVolume;
            }
        }
    }
		public void QueueSound(AudioClip snd,float pitch)
		{        
        soundAndPitch sound = new soundAndPitch();
        sound.clip = snd;
        sound.pitch = pitch;
        sound.time = Time.time;
        if (soundQueue.Count>0)
        {
            if (soundQueue[0].clip.length > 1.0f && snd.length < 1.0f)
            {
                PlayVoice(snd, pitch);
            }
            else
            {
                soundQueue.Add(sound);
            }
        }
        else soundQueue.Add(sound);
        }
		public void QueuePlayNow(AudioClip snd,float pitch)
		{
        soundQueue.Clear();
			if(playingQueued!=null) Destroy(playingQueued.gameObject);
			QueueSound(snd,pitch);
		}

    public void ToggleMute()
    {
        if (MasterVolume > 0)
        {
            MasterVolume = 0;
        }
        else
        {
            MasterVolume = 1;
        }           
        
    }
}
