using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRaycaster : MonoBehaviour
{
    public LayerMask collisionMask;

    public float deacelerationRate, startSpeed;

    [Range (0, 2.0f)]
    public float rayLength = 1;

    public  Vector2 velocity;
    Vector2 aux;

    protected void SetVelocity (Vector2 direction) {
        velocity = direction.normalized * startSpeed;
        aux = velocity;
    }

    private void CheckColision () {
        RaycastHit2D hit = Physics2D.Raycast (this.transform.position, velocity.normalized, rayLength, collisionMask);


        if (hit) {
            velocity = Vector2.Reflect (velocity, hit.normal);
        }
    }

    public void MoveGhost () {
        if (velocity.magnitude > 0) {

            CheckColision ();

            transform.Translate (velocity * Time.fixedDeltaTime);
            velocity -= velocity.normalized * Mathf.Abs (deacelerationRate);

            if (velocity.magnitude < Mathf.Abs (deacelerationRate)) velocity = Vector2.zero;
        }

    }

    protected virtual void MoveHalfGhost () {
        if (velocity.magnitude > 0) {

            CheckColision ();

            transform.Translate (velocity/2 * Time.fixedDeltaTime);
            velocity -= velocity.normalized * Mathf.Abs (deacelerationRate/2);

            if (velocity.magnitude < Mathf.Abs (deacelerationRate/2)) velocity = Vector2.zero;
        }

    }


    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void FixedUpdate () {
    }
}
