using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBomb : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    public bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            hasExploded = true;
        }
    }

}