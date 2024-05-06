using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Rigidbody2D _rigidBody;

    [Header("Speed")]
    public Vector2 maxSpeed;

    private void Awake() {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        SpeedCheck();
    }

    public void ApplyExternalForce(Vector2 force) {
        Debug.Log(force.magnitude);

        _rigidBody.AddForce(force);
    }

    void SpeedCheck() {
        Vector2 newSpeed = _rigidBody.velocity;

        newSpeed.x = newSpeed.x > maxSpeed.x ? maxSpeed.x : newSpeed.x;
        newSpeed.y = newSpeed.y > maxSpeed.y ? maxSpeed.y : newSpeed.y;

        _rigidBody.velocity = newSpeed;
    }
}
