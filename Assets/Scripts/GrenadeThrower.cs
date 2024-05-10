using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 20f;
    public GameObject grenadeTimedPrefab;
    public InputMaster controls;

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.E))
       {
        ThrowGrenade();
       }
    }


    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadeTimedPrefab, transform.position, transform.rotation);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.forward * throwForce);
    }


}