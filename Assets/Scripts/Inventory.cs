using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    Weapon _equippedWeapon;
    public Weapon equippedWeapon {
        get { return _equippedWeapon; }
    }

    public void EquipWeapon(Weapon newWeapon) {
        if(_equippedWeapon != null)
            Destroy(_equippedWeapon.gameObject);

        _equippedWeapon = newWeapon;
    }
}
