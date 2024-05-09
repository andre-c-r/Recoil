using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armory", menuName = "ScriptableObjects/Armory", order = 1)]
public class Armory : ScriptableObject {
    public GameObject defaultWeapon;

    public GameObject[] weaponPrefabs;
}
