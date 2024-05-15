using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour {
    public AudioClip snd;

    protected virtual void OnTriggerEnter2D (Collider2D collision) {
        if (collision.CompareTag ("Player")) {

            CollectableEffect ();

            if (snd != null)
                AudioControl.Singleton.PlaySound (snd);

            this.gameObject.SetActive(false);
        }
    }

    protected abstract void CollectableEffect ();
}
