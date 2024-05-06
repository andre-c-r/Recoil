using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePhysics : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public GameObject explosionEffect;
    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode ()
    {
        //Show effect
                if (!hasExploded)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        //Add Force
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
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
