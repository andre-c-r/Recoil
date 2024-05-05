using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound3D : MonoBehaviour {
	float Duration;

	void Start () {
		this.gameObject.GetComponent<AudioSource>().Play ();
		Duration = this.gameObject.GetComponent<AudioSource> ().clip.length;
		Destroy (this.gameObject, Duration);
	}
}
