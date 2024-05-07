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
    Vector2 _aimDirection;

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

    public void AimWeapon(Vector2 aimDirection) {
        _aimDirection = aimDirection;

        Vector2 realAimLocation = new Vector2(_equipedWeapon.transform.position.x + aimDirection.x, _equipedWeapon.transform.position.y + aimDirection.y);

        _equipedWeapon.AimWeapon(realAimLocation);
    }

    public void EquipWeapon(Weapon newWeapon) {
        _equipedWeapon = newWeapon;
    }

    public void FireWeapon() {
        if (_equipedWeapon == null) return;

        ApplyExternalForce(_equipedWeapon.recoilStrenght * _aimDirection.normalized * -1);
        _equipedWeapon.FireWeapon(_aimDirection);

        _framesFromLastShot = framesToReload;
    }
}
