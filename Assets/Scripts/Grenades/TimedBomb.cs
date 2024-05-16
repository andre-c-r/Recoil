using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedBomb : Grenade {
    public float delay = 3f;
    float countdown;

    // Start is called before the first frame update
    void Start() {
        countdown = delay;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate() {
        countdown -= Time.deltaTime;
        if (countdown <= 0f) {
            Explode();
        }
    }

}