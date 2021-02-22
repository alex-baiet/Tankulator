using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class StatWeapon
{
    public string name;
    public string[] ammo;

    public bool randomAmmoOrder = false;

    public string shootAnimPath = "";
    public bool shootAnimRepeat = false;

    public float rateOfFire = 10f;
    public bool alwaysReloading = false;

    public float inaccuracy = 0f;
    public bool orderedInaccuracy = false;
    public int bulletNbr = 1;

    public float rotationStart = 0f;
    public float rotationEnd = 0f;
    public float rotationSpeed = 0f; //degrees/second
    public bool rotationMirror = false;

}
