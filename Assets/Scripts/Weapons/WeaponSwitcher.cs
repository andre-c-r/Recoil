using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : Collectable {

    public int weaponId;

    protected override void CollectableEffect() {
        GameController.Singleton?.mainCharacter.EquipWeapon(GameController.Singleton?.armory.weaponPrefabs[weaponId]);
    }
}
