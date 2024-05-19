using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
    Player player;
    public ParticleSystem dust;
    public static Vector2 Checkpoint { get; set; } = Vector2.zero;

    private void Start() {
        player = this.GetComponent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider == null) return;

        if (collision.collider.CompareTag("Enemy")) {
            GameController.Singleton?.ResetLevel();
        }

        if (collision.collider.CompareTag("RubberFloor")) {
            player?.ReflectSpeed(collision.GetContact(0).normal);
        }

        if (collision.collider.CompareTag("Floor")) {
            Vector2 contactPoint = collision.GetContact(0).point;
            CreateDust(contactPoint);
        }

    }
    private void CreateDust(Vector2 position) {
        // Convert Vector2 position to Vector3 to match the ParticleSystem's transform position requirement
        Vector2 dustPosition = new Vector2(position.x, position.y);
        dust.transform.position = dustPosition; // Move the dust particle system to the contact point
        dust.Play(); // Play the particle effect
    }

}
