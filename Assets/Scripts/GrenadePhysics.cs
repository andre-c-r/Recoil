using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePhysics : MonoBehaviour
{
    [SerializeField] public float radius = 5f;
    [SerializeField] public float force = 700f;
    [SerializeField] public GameObject explosionEffect;

    public TimedBomb timedBomb;

    // Start is called before the first frame update
    void Start()
    {
        timedBomb = GetComponent<TimedBomb>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timedBomb.hasExploded == true)
        {
            Explode();
        }
    }

    void Explode ()
    {
        //Show effect
                if (!timedBomb.hasExploded)
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