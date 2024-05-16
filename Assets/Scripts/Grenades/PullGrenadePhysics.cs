using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PullGrenadePhysics : TimedBomb {
    [SerializeField] public float pullRadius = 100f;
    [SerializeField] public float pullForce = 1000f;
    [SerializeField] public GameObject explosionEffect;

    public override void Explode() {
        //Show effect
        if (!hasExploded) {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        //Add pullForce
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pullRadius);

        foreach (Collider2D nearbyObject in colliders) {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null) {
                Vector2 direction = transform.position - rb.transform.position;

                float distance = direction.magnitude;
                float pullForceMagnitude = Mathf.Clamp01((pullRadius - distance) / pullRadius) * pullForce;

                rb.AddForce(direction.normalized * pullForceMagnitude);
            }
        }
        // Damage

        //Remove Grenade
        Destroy(gameObject);
    }
}