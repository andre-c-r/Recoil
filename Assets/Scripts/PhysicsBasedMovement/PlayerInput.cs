using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
    InputMaster _controls;

    Player _player;

    public float deadzone = 0.2f;

    Vector2 _axisToMouse;

    public Vector2 aimAxis {
        get { return _axisToMouse; }
    }

    bool mouseAndKeyboard = true;

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

        if (mouseAndKeyboard)
            _controls.Player.Fire.performed += ctx => _player.FireWeapon();
        //controls.Player.Jump.canceled += ctx => player.OnJumpInputUp();

        //Die to reset
        //controls.Player.Reset.performed += ctx => GameController.Singleton.Die ();

        //_controls.Player.Movement.performed += ctx => axis = ctx.ReadValue<Vector2>();
        //_controls.Player.Movement.canceled += ctx => axis = Vector2.zero;
    }

    private void Update() {
        _axisToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

        _player.AimWeapon(_axisToMouse.normalized);
    }
}
