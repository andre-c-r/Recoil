using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarS : MonoBehaviour
{
    public Slider slider;

    public void SetMaxAmmo(int ammo)
    {
        slider.maxValue = ammo;
        slider.value = ammo;
    }

    public void SetAmmoLeft(int ammo)
    {
        slider.value = ammo;
    }

}
