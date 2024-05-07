using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour {
    Rigidbody2D _rigidBody;

    [Header("Speed")]
    public Vector2 maxSpeed;

    [Header("WeaponControl")]
    public int framesToReload = 6;
    int _framesFromLastShot = 0;

    public Weapon _equipedWeapon;

    CollisionChecker _collisionChecker;

    private void Awake() {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _collisionChecker = this.GetComponent<CollisionChecker>();
    }

    private void FixedUpdate() {
        GrounderCheck();

        SpeedCheck();
    }

    public void ApplyExternalForce(Vector2 force) {
        _rigidBody.AddForce(force);
    }

    void SpeedCheck() {
        Vector2 newSpeed = _rigidBody.velocity;

        newSpeed.x = newSpeed.x > maxSpeed.x ? maxSpeed.x : newSpeed.x;
        newSpeed.y = newSpeed.y > maxSpeed.y ? maxSpeed.y : newSpeed.y;

        _rigidBody.velocity = newSpeed;
    }

    void GrounderCheck() {
        _collisionChecker.CheckGrounder(_rigidBody.velocity);

        if (_framesFromLastShot > 0) {//cooldown to reset jump to avoid double jump
            _framesFromLastShot--;
            return;
        }

        if (_collisionChecker.collisions.below && Array.IndexOf(_collisionChecker.collisions.verticalTag, "Floor") > -1) { //Cehck colision below and floor tag
            _equipedWeapon.FullReload();
        }
    }

    public void EquipWeapon(Weapon newWeapon) {
        _equipedWeapon = newWeapon;
    }

    public void FireWeapon(Vector2 direction) {
        if (_equipedWeapon == null) return;

        ApplyExternalForce(_equipedWeapon.recoilStrenght * direction.normalized * -1);
        _equipedWeapon.FireWeapon();

        _framesFromLastShot = framesToReload;
    }
}
