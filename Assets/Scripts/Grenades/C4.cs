using UnityEngine;

public class C4 : Grenade {

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
                Vector2 direction = rb.transform.position - transform.position;

                float distance = direction.magnitude;
                float forceMagnitude = Mathf.Clamp01((radius - distance) / radius) * force;

                rb.AddForce(direction.normalized * forceMagnitude);
            }
        }
        // Damage

        //Remove Grenade
        Destroy(gameObject);
    }
}
