using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    protected float _lifetime = 8;

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Floor")) {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        Destroy(this.gameObject, _lifetime);
    }
}
