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
    [Range(0.0f, 1.0f)]
    public float resetFactorOnTurnAround = 1;

    [Range(0.0f, 10.0f)]
    public float extraVerticalImpulseFactor = 1;

    public Transform weaponPivotPoint;

    public Armory armory;
    Inventory _inventory;
    Vector2 _aimDirection;

    [Header("Grounder")]
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask grounderMask;

    private void Awake() {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    private void Start() {
        if (GameController.Singleton != null) {
            _inventory = GameController.Singleton.playerinventory;
            armory = GameController.Singleton.armory;
            GameController.Singleton.mainCharacter = this;
        }
        else {
            _inventory = this.gameObject.AddComponent<Inventory>();
        }

        EquipWeapon(armory.defaultWeapon);
    }

    public void ReflectSpeed(Vector2 reflectionNormal) {
        _rigidBody.velocity = Vector2.Reflect(new Vector2(maxSpeed.x * reflectionNormal.x * -1, maxSpeed.y), reflectionNormal);

        _rigidBody.velocity *= maxSpeed;

        SpeedCheck();
    }

    private void FixedUpdate() {
        GrounderCheck();

        SpeedCheck();

        AimWeapon();
    }

    public void ApplyExternalForce(Vector2 force) {

        Vector2 newSpeed = _rigidBody.velocity;

        newSpeed.x = Mathf.Sign(force.x) == Mathf.Sign(newSpeed.x) ? newSpeed.x : newSpeed.x * resetFactorOnTurnAround;
        newSpeed.y = Mathf.Sign(force.y) == Mathf.Sign(newSpeed.y) ? newSpeed.y : newSpeed.y * resetFactorOnTurnAround;

        _rigidBody.velocity = newSpeed;


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
        if (_framesFromLastShot > 0) {//cooldown to reset jump to avoid double jump
            _framesFromLastShot--;
            return;
        }


        if (_framesFromLastShot <= 0 && Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, grounderMask)) {
            _inventory.equippedWeapon.FullReload();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position - (transform.up * castDistance), boxSize);
    }

    public void SetAimAxis(Vector2 aimDirection) {
        _aimDirection = aimDirection;
    }

    void AimWeapon() {
        Vector2 realAimLocation = new Vector2(_inventory.equippedWeapon.transform.position.x + _aimDirection.x,
            _inventory.equippedWeapon.transform.position.y + _aimDirection.y);

        _inventory.equippedWeapon.AimWeapon(realAimLocation);
    }

    public void EquipWeapon(GameObject prefab) {
        Weapon weaponAux = Instantiate(prefab, weaponPivotPoint).GetComponent<Weapon>();

        _inventory.EquipWeapon(weaponAux);
    }

    public void FireWeapon() {
        if (_inventory.equippedWeapon == null) return;
        if (!_inventory.equippedWeapon.IsReadyToShoot) return;

        //Debug.Log(_aimDirection.normalized + "  " + _equipedWeapon.recoilStrenght * new Vector2(_aimDirection.normalized.x, _aimDirection.normalized.y * _rigidBody.gravityScale) * -1);

        ApplyExternalForce(_inventory.equippedWeapon.recoilStrenght * new Vector2(_aimDirection.normalized.x,
            _aimDirection.normalized.y * extraVerticalImpulseFactor * _rigidBody.gravityScale) * -1);

        _inventory.equippedWeapon.FireWeapon(_aimDirection);

        _framesFromLastShot = framesToReload;
    }
}
