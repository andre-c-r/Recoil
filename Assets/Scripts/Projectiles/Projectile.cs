using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    protected float _lifetime = 8;

    [SerializeField]
    protected int damage = 2;

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider == null) return;
        
        if (collision.collider.CompareTag("Floor")) {
            Destroy(this.gameObject);
        }
        

        if (collision.collider.CompareTag("Enemy")) {
            collision.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);

            Destroy(this.gameObject);
        }
    }

    private void Start() {
        Destroy(this.gameObject, _lifetime);
    }
}
