using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
	public float FadeIncrement;
	public float FadePercent;
	public bool Fading;
	int Aux = 1;
	public AudioSource audioS;
    float initialVolume, TargetVolume;
	// Update is called once per frame
	void Awake()
	{
		audioS = this.GetComponent<AudioSource> ();
		FadePercent = 0;
	}

	void Update () {

		if (Fading) 
		{	
			FadePercent += FadeIncrement * Aux;
			audioS.volume = (TargetVolume-initialVolume) * FadePercent;
			if (FadePercent > 1)
				StopFade ();
			else if (FadePercent < 0)
				Destroy (this.gameObject);
			
		}

	}

	public void StartFade(float time,float from,float to)
	{
        initialVolume = from;
        TargetVolume = to;
		FadeIncrement = 1 / time * Time.deltaTime;

        //if (FadePercent > 0) Aux = -1;
        FadePercent = 0;
		Fading = true;
	}
	void StopFade()
	{
		Fading = false;
		FadePercent = 1.0f;
	}
}
