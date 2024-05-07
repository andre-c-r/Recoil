using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {
    public override void FireWeapon(Vector2 direction) {
        _currentAmmo--;

        if (projectilePrefab == null) return;

        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
    }
}
