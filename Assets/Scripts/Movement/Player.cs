using UnityEngine;
using System.Collections;
using System;
using System.Threading.Tasks;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
    Rigidbody2D rb;

    int dirMultiplier = 1;

    public AudioClip sndSpin, sndHitGround;

    [Header ("Speed")]
    [Range (0, 3.0f)]
    public float speedMultiplier = 1;
    public Vector3 velocity;

    [Header ("Dash")]
    public Vector2 dashForce = new Vector2 (30, 10);
    bool dashGrounder;
    public float imuneToGravity = 0;
    public float imuneToGravityTime;
    bool dashed = false;

    [Header ("Jump")]
    public bool resetDoubleJumpOnWalls;

    bool grounder;
    bool jumped;

    float FramesToLoseGrounder = 12, framesFalling;

    public int extraJumps;
    int jumpsWasted;
    [Range (0, 1.0f)]
    public float accelerationTimeAirborne = .2f, accelerationTimeGrounded = .1f;
    int dashAccelerationTimeLock = 0;
    float moveSpeed = 6;

    public float maxJumpVelocity, minJumpVelocity;
    public float maxJumpVelocityAirJump, minJumpVelocityAirJump;
    public float maxVerticalVelocity;

    [Range (-100, -0.1f)]
    public float gravity = -50;
    int gravityMultiplier = 1;

    [Header ("Wall Physics")]
    public int wallJumpUncontrollableFrames = 6;
    int currentWallJumpUncontrollableFrames = 0;

    public Transform leftAnchor;
    public Transform rightAnchor;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    float currentWallSlideSpeedMax = 0;
    public float wallStickTime = .25f;
    float timeToWallUnstick;
    float velocityXSmoothing;

    Controller2D controller;

    PlayerAnimatorControl playerAnimator;

    Vector2 directionalInput;
    int wallDirX;

    bool stuckIntoWall = false;

    bool wallSliding = false;

    public GameObject characterRender;
    bool isOnGround;

    Collider2D col;

    bool alive;
    public bool Alive {
        get { return alive; }
    }

    [Header ("Animations")]
    CharacterVisuals visuals;

    public int DirMultipler {
        get {
            return dirMultiplier;
        }
        set {
            if (value == 1 || value == -1) dirMultiplier = value;
        }
    }

    public bool IsOnGround () {
        return isOnGround;
    }

    public bool Grounded {
        get {
            return grounder;
        }
    }

    void Start () {
        if (GameController.Singleton.mainCharacter == null) {
            GameController.Singleton.mainCharacter = this;
        }
        else {
            Destroy (this.gameObject);
        }

        controller = GetComponent<Controller2D> ();

        alive = true;

        playerAnimator = GetComponent<PlayerAnimatorControl> ();

        col = controller.collider;

        currentWallSlideSpeedMax = wallSlideSpeedMax;

        rb = this.GetComponent<Rigidbody2D> ();

        jumpsWasted = 100;
    }

    public async void Die () {
        alive = false;

        velocity = Vector3.zero;
    }

    private void OnEnable () {
        alive = true;
    }

    void FixedUpdate () {
        if (alive) {
            if (currentWallJumpUncontrollableFrames > 0) currentWallJumpUncontrollableFrames--;

            GrounderCheck ();

            CalculateVelocity ();
            HandleWallSliding ();
            Dash ();

            controller.Move (velocity * Time.deltaTime, directionalInput, imuneToGravity > 0);

            if (imuneToGravity > 0) imuneToGravity -= Time.deltaTime;

            if (controller.collisions.above || controller.collisions.below) {
                if (controller.collisions.slidingDownMaxSlope) {
                    velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
                }
                else {
                    velocity.y = 0;
                }
            }
        }
    }

    public void Dash () {
        if (!dashed) return; //BtnPress
        dashed = false;

        if (Mathf.Abs (directionalInput.x) < 0.2f) return; //Directional
        if (!dashGrounder) return; //Grounder

        if (controller.collisions.left && directionalInput.x < 0) return; //can't dash on the direction of a wall yo'ure touching
        if (controller.collisions.right && directionalInput.x > 0) return;

        if (isOnGround)
            playerAnimator.Dash ();
        else
            playerAnimator.AirDash ();

        dashGrounder = false;
        imuneToGravity = imuneToGravityTime;
        dashAccelerationTimeLock = 2;

        SetForce (new Vector2 (dashForce.x * directionalInput.x, dashForce.y * gravityMultiplier));
    }

    public void SetForce (Vector2 f) {
        if (f.x != 0)
            velocity.x = f.x * dirMultiplier;

        if (f.y != 0)
            velocity.y = f.y;
    }

    public void SetDirectionalInput (Vector2 input) {
        directionalInput = input;
    }

    public void ApplyJumpInput () {

        playerAnimator.StartJump ();

        if (controller.collisions.slidingDownMaxSlope) {
            if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
                velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y * gravityMultiplier;
                velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
            }
        }
        else if (grounder) {
            velocity.y = maxJumpVelocity * gravityMultiplier;
            isOnGround = false;
        }
        else {
            velocity.y = maxJumpVelocityAirJump * gravityMultiplier;
            isOnGround = false;
        }
    }

    public void OnJumpInputDown () {

        RaycastHit2D hit;

        if (grounder) {
            jumped = true;
            ApplyJumpInput ();

            grounder = false;
        }
        else if (wallSliding) {
            /*if (wallDirX == directionalInput.x) {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (Mathf.Abs (directionalInput.x) < 0.15f) {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }

            if (Physics2D.Raycast (rightAnchor.position, rightAnchor.right, 0.3f, controller.collisionMask)) {
                velocity.x = wallJumpOff.x * -1;
                velocity.y = wallJumpOff.y;
            }
            else if (Physics2D.Raycast (leftAnchor.position, leftAnchor.right, 0.3f, controller.collisionMask)) {
                velocity.x = wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else {*/
            velocity.x = -wallDirX * wallLeap.x;

            velocity.y = wallLeap.y * gravityMultiplier;
            //}

            currentWallJumpUncontrollableFrames = wallJumpUncontrollableFrames;

            dashGrounder = true;
        }
        else if (Physics2D.Raycast (rightAnchor.position, rightAnchor.right, 0.1f, controller.collisionMask)) {

            velocity.x = wallJumpOff.x * -1;
            velocity.y = wallJumpOff.y;

            currentWallJumpUncontrollableFrames = wallJumpUncontrollableFrames;

            dashGrounder = true;
        }
        else if (Physics2D.Raycast (leftAnchor.position, leftAnchor.right, 0.1f, controller.collisionMask)) {

            velocity.x = wallJumpOff.x;
            velocity.y = wallJumpOff.y;

            currentWallJumpUncontrollableFrames = wallJumpUncontrollableFrames;

            dashGrounder = true;
        }
        else if (jumpsWasted < extraJumps) {
            ApplyJumpInput ();
            jumpsWasted++;
        }
    }

    public void OnJumpInputUp () {
        if (Mathf.Abs (velocity.y) > minJumpVelocity && Mathf.Sign (gravityMultiplier) == Mathf.Sign (velocity.y)) {
            velocity.y = minJumpVelocity * gravityMultiplier;
        }
    }

    public void StickIntoWall () {
        stuckIntoWall = true;
    }

    public void UnstickIntoWall () {
        stuckIntoWall = false;
    }

    void HandleWallSliding () {
        wallDirX = (controller.collisions.left) ? -1 : 1;

        if (Array.IndexOf (controller.collisions.horizontalTag, "Wall") > -1) { //checa se está na parede segurável
            wallSliding = true;
        }
        else {
            wallSliding = false;
        }

        playerAnimator.SetWallSlidingVariable (wallSliding);

        if (stuckIntoWall && wallSliding) { //checa se está segurando
            currentWallSlideSpeedMax = 0;
        }
        else {
            currentWallSlideSpeedMax = wallSlideSpeedMax;
        }

        bool floorCollision = CheckGroundCollision ();

        if ((controller.collisions.left || controller.collisions.right) && !CheckGroundCollision () && velocity.y * gravityMultiplier < 0) { //checa se efetivamente está na ação de deslizar
            if (resetDoubleJumpOnWalls) {
                jumpsWasted = 0;
            }

            if (Mathf.Abs (velocity.y) > currentWallSlideSpeedMax) {
                velocity.y = currentWallSlideSpeedMax * -gravityMultiplier;
            }

            if (timeToWallUnstick > 0) {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0 && currentWallSlideSpeedMax > 0) {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else {
                timeToWallUnstick = wallStickTime;
            }

        }
    }

    void GrounderCheck () {
        bool collisionBellow = false;

        if (Array.IndexOf (controller.collisions.verticalTag, "Floor") > -1 || Array.IndexOf (controller.collisions.verticalTag, "Wall") > -1) {
            collisionBellow = true;
            isOnGround = true;
        }
        isOnGround = CheckGroundCollision ();

        bool collisionCheck = CheckGroundCollision ();
        playerAnimator.SetGrounderVariable (isOnGround);

        if (collisionCheck && collisionBellow && !jumped) {
            if (!grounder) {
                playerAnimator.EndJump ();
                AudioControl.Singleton.PlaySound (sndHitGround);
            }

            GroundCharacter ();
            framesFalling = 0;
            ResetAirJump ();
            dashGrounder = true;
        }
        else if (FramesToLoseGrounder <= framesFalling) {
            grounder = false;
        }
        else if (grounder) {
            framesFalling++;
        }

        if (jumped) jumped = false;
    }

    void CalculateVelocity () {
        float targetVelocityX = directionalInput.x * moveSpeed * speedMultiplier;

        bool floorCollision = CheckGroundCollision ();

        if (currentWallJumpUncontrollableFrames > 0 && !floorCollision && targetVelocityX * velocity.x < 0) {
            targetVelocityX = 0;
        }

        if (dashAccelerationTimeLock > 0) {
            velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
            dashAccelerationTimeLock--;
        }
        else {
            velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (floorCollision) ? accelerationTimeGrounded : accelerationTimeAirborne);
        }

        if (Mathf.Abs (velocity.y) < Mathf.Abs (maxVerticalVelocity)) {
            velocity.y += gravity * Time.deltaTime * gravityMultiplier;
        }
    }

    bool CheckGroundCollision () {
        if (gravityMultiplier < 0)
            return controller.collisions.above;

        return controller.collisions.below;
    }

    void GroundCharacter () {
        grounder = true;
    }

    public void ResetAirJump () {
        jumpsWasted = 0;
    }

    public void SetDash () {
        dashed = true;
    }
}
