using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePhysics : TimedBomb {

    public override void Explode() {
        //Show effect
        if (explosionEffect != null) {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            explosion.transform.localScale = new Vector3(radius, radius, radius);
            Destroy(explosion, 5);
        }

        //Add Force
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D nearbyObject in colliders) {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null) {
                Vector2 direction = rb.transform.position - transform.position; // Direction from grenade to object
                direction.Normalize(); // Normalize the direction vector
                float forceMagnitude = force; // You can adjust this to control the strength of the launch
                rb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);
            }
        }
        // Damage

        //Remove Grenade
        Destroy(gameObject);
    }
}
