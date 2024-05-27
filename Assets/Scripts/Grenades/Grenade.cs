using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grenade : MonoBehaviour {
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected float force = 50f;
    public GameObject explosionEffect;

    public abstract void  Explode();
}
