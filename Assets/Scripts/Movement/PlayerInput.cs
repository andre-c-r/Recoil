using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
using System;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {

    Player player;

    PlayerAnimatorControl playerAnimator;

    InputMaster controls;

    Vector2 axis = Vector2.zero, rawAxis = Vector2.zero;

    public float deadzone = 0.2f;

    InteractiveHandler interactiveHandler;

    Vector3 initPosition;

    //GetSet
    public Vector2 Axis {
        get {
            return axis;
        }
        set {
            axis = value;
        }
    }

    public Vector2 RawAxis {
        get {
            return rawAxis;
        }
    }

    public void Dash () {
        player.SetDash();
    }

    private void OnEnable () {
        controls.Player.Enable ();
    }

    private void OnDisable () {
        controls.Player.Disable ();
    }

    private void Awake () {
        controls = new InputMaster ();

        controls.Player.Jump.performed += ctx => player.OnJumpInputDown ();
        controls.Player.Jump.canceled += ctx => player.OnJumpInputUp ();

        controls.Player.Dash.performed += ctx => Dash ();

        //Die to reset
        //controls.Player.Reset.performed += ctx => GameController.Singleton.Die ();

        controls.Player.Movement.performed += ctx => axis = ctx.ReadValue<Vector2> ();
        controls.Player.Movement.canceled += ctx => axis = Vector2.zero;
    }

    void Start () {
        player = GetComponent<Player> ();
        playerAnimator = GetComponent<PlayerAnimatorControl> ();
        interactiveHandler = GetComponent<InteractiveHandler> ();

        GameController.Singleton.mainCharacterInput = this;

        initPosition = this.transform.position;
    }

    void Update () {
    }

    Vector2 TransformIntoRaw (Vector2 vec, float deadzone = 0.2f) {
        Vector2 r = Vector2.zero;

        if (Math.Abs (vec.x) > deadzone) {
            if (vec.x > 0) {
                r.x = 1;
            }
            else if (vec.x < 0) {
                r.x = -1;
            }
        }
        if (Math.Abs (vec.y) > deadzone) {
            if (vec.y > 0) {
                r.y = 1;
            }
            else if (vec.x < 0) {
                r.y = -1;
            }
        }

        return r;
    }

    private void FixedUpdate () {
        rawAxis = TransformIntoRaw (axis, deadzone);

        if (player.Alive) {
            player.SetDirectionalInput (rawAxis);
        }
        else {
            player.SetDirectionalInput (Vector2.zero);
        }

    }

}
