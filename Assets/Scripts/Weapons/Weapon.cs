using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    [SerializeField]
    protected float _recoilStrenght;
    [SerializeField]
    protected int maxAmmo;

    int _currentAmmo;
    public int currentAmmo {
        get { return _currentAmmo; }
    }

    public Projectile projectile;
    public Transform firePoint;

    public float recoilStrenght {
        get { return _recoilStrenght; }
    }

    public abstract void FireWeapon();

    public virtual void Reload(int ammoAmount) {
        _currentAmmo += ammoAmount;
    }

    public virtual void FullReload() {
        _currentAmmo = maxAmmo;
    }
}
