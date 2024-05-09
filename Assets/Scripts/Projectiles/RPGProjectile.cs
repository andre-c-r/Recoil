using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGProjectile : Projectile {
    [SerializeField]
    float radius = 4;

    [SerializeField]
    float shockwaveMagnitude = 5;

    [SerializeField]
    LayerMask damageMask;

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Floor")) { //ADICIONAR INIMIGO TAMBÉM ASSIM QUE PASSAREM A EXISTIR
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius, damageMask);

            foreach (Collider2D collider in colliders) {
                if (collider.CompareTag("Player")) collider.attachedRigidbody.gameObject.GetComponent<Player>().ApplyExternalForce(
                    (collider.attachedRigidbody.gameObject.transform.position - this.transform.position).normalized * shockwaveMagnitude);
            }

            Destroy(this.gameObject);
        }
    }
}
