using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    Weapon _equippedWeapon;
    public Weapon equippedWeapon {
        get { return _equippedWeapon; }
    }

    GrenadeThrower _equippedGrenadeThrower;
    public GrenadeThrower grenadeThrower {
        get { return _equippedGrenadeThrower; }
    }

    public void ReloadEquipment() {
        _equippedWeapon.FullReload();
        _equippedGrenadeThrower.Reload();
    }

    public void EquipWeapon(Weapon newWeapon) {
        if (_equippedWeapon != null)
            Destroy(_equippedWeapon.gameObject);

        _equippedWeapon = newWeapon;
    }

    public void EquipGrenade(GrenadeThrower newGrenade) {
        _equippedGrenadeThrower = newGrenade;
    }

}
