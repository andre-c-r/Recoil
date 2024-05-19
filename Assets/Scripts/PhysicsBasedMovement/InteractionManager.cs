using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
    Player player;

    private void Start() {
        player = this.GetComponent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider == null) return;

        if (collision.collider.CompareTag("Enemy")) {
            // GameController.Singleton?.ResetLevel();
            GameController.Singleton?.ResetToCheckpoint();
        }

        if (collision.collider.CompareTag("RubberFloor")) {
            player?.ReflectSpeed(collision.GetContact(0).normal);
        }
    }
}
