using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCamera : MonoBehaviour
{
    public GameObject player;
    public float camHeight;
    public float camDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Transform pt = player.transform;

        // Set camera Position;
        Vector3 camOffset = -pt.forward * camDistance + pt.up * camHeight;
        transform.position = pt.position + camOffset;

        // Set camera orientation
        transform.LookAt(pt.position, pt.up);
    }
}
