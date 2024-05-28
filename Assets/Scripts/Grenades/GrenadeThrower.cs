using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GrenadeThrower", menuName = "ScriptableObjects/GrenadeThrower", order = 2)]
public class GrenadeThrower : ScriptableObject {
    public float throwForce = 20f;

    public GameObject grenadePrefab;

    protected bool _ammo = false;
    
    public bool ammo {
        get {
            return _ammo;
        }
    }

    public void Reload() {
        _ammo = true;
    }

    public virtual void ThrowGrenade(Vector2 direction, Transform firePoint) {
        if (!_ammo) return;

        _ammo = false;

        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.AddForce(direction.normalized * throwForce);
    }
}