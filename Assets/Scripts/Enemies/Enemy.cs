using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    int maxLife = 2;

    [SerializeField]
    bool shieldedUnity = false;

    [SerializeField]
    GameObject shield;

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
            GameController.Singleton?.playerinventory.ReloadEquipment();
            this.gameObject.SetActive(false);
        }
    }
}
