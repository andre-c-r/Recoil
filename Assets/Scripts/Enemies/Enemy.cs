using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Enemy : MonoBehaviour {
    [SerializeField]
    int maxLife = 2;

    [SerializeField]
    bool shieldedUnity = false;

    [SerializeField]
    GameObject shield;

    [SerializeField]
    GameObject deathExplosion;

    public ParticleSystem DeathParticles;

    protected void Start() {
        shield.SetActive(shieldedUnity);
    }

    public void TakeDamage(int damageTaken = 2) {
        if (shieldedUnity && shield != null && shield.activeSelf) {
            shield.SetActive(false);
            return;
        }

        maxLife -= damageTaken;

        if (maxLife <= 0) {
            DeathParticles.Play();
            GameController.Singleton?.playerinventory.ReloadEquipment();

            if (deathExplosion != null) {
                GameObject explosion = Instantiate(deathExplosion, this.transform.position, this.transform.rotation);
                Destroy(explosion, 5);
            }
            if (DeathParticles != null) {
                ParticleSystem ps = Instantiate(DeathParticles, transform.position, Quaternion.identity);
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration);
            }
            this.gameObject.SetActive(false);
        }
    }
}
