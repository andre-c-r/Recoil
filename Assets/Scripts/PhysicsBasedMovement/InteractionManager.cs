using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    Player player;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider == null) return;

        if (collision.collider.attachedRigidbody.CompareTag("Enemy")) {
            GameController.Singleton?.ResetLevel();
        }
    }
}
