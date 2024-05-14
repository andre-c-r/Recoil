using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathplane : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider == null) return;

        if (collision.collider.CompareTag("Player")) {
            GameController.Singleton?.ResetLevel();
        }
    }
}
