using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
    InputMaster _controls;

    Player _player;

    public float deadzone = 0.2f;

    Vector2 _axisToMouse, _axisLeftTrigger, _axisRightTrigger;
    public Vector2 _controllerAxisGrenade, _controllerAxisGun;

    public bool mnK = true;

    public Vector2 aimAxis {
        get { return _axisToMouse; }
    }

    private void Awake() {
        _player = this.GetComponent<Player>();

        SetupInputSystem();
    }

    private void OnEnable() {
        _controls.Player.Enable();
    }

    private void OnDisable() {
        _controls.Player.Disable();
    }

    private void SetupInputSystem() {
        _controls = new InputMaster();

        _controls.Player.Fire.performed += ctx => _player.FireWeapon();

        _controls.Player.FireGranade.performed += ctx => _player.FireGranade();

        //Die to reset
        _controls.Player.Reset.performed += ctx => GameController.Singleton?.ResetLevel();

        _controls.Player.LeftAxis.performed += ctx => _axisLeftTrigger = ctx.ReadValue<Vector2>();

        _controls.Player.RightAxis.performed += ctx => _axisRightTrigger = ctx.ReadValue<Vector2>();
    }

    private void Update() {
        if (GameController.Singleton != null)
           mnK = !GameController.Singleton.controller;

        _axisToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

        if (mnK) {
            _controllerAxisGun = _axisToMouse.normalized;
            _controllerAxisGrenade = _axisToMouse.normalized;
        }
        else {
            Vector2 aux = _axisRightTrigger.normalized;

            aux = aux.magnitude > deadzone ? aux : _controllerAxisGun;

            _controllerAxisGun = aux;
        }

        _player.SetAimAxis(_controllerAxisGun, _controllerAxisGrenade);
    }
}
