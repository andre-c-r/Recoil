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

    public float extraVerticalImpulseFactor = 1;

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

        Debug.Log(_rigidBody.velocity);
    }

    public void ApplyExternalForce(Vector2 force) {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);

        SpeedCheck();
    }

    void SpeedCheck() {
        Vector2 newSpeed = _rigidBody.velocity;

        newSpeed.x = MathF.Abs(newSpeed.x) > maxSpeed.x ? maxSpeed.x * Mathf.Sign(newSpeed.x) : newSpeed.x;
        newSpeed.y = MathF.Abs(newSpeed.y) > maxSpeed.y ? maxSpeed.y * Mathf.Sign(newSpeed.y) : newSpeed.y;

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

        //Debug.Log(_aimDirection.normalized + "  " + _equipedWeapon.recoilStrenght * new Vector2(_aimDirection.normalized.x, _aimDirection.normalized.y * _rigidBody.gravityScale) * -1);

        ApplyExternalForce(_equipedWeapon.recoilStrenght * new Vector2(_aimDirection.normalized.x,
            _aimDirection.normalized.y * extraVerticalImpulseFactor * _rigidBody.gravityScale) * -1);

        _equipedWeapon.FireWeapon(_aimDirection);

        _framesFromLastShot = framesToReload;
    }
}
