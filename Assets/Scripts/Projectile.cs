using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float _lifetime = 8;

    private void Start() {
        Destroy(this.gameObject, _lifetime);
    }
}
