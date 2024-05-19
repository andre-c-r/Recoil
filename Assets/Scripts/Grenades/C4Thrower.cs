using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GrenadeThrower", menuName = "ScriptableObjects/C4Thrower", order = 3)]
public class C4Thrower : GrenadeThrower {

    GameObject grenade = null;

    public override void ThrowGrenade(Vector2 direction, Transform firePoint) {
        if (!_ammo) return;
        if (grenade != null) {
            C4 aux = grenade.GetComponent<C4>();
            aux.Explode();
        }
        else {
            _ammo = false;

            grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * throwForce);
        }

    }
}