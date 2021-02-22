using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatWeaponLoaded : StatWeapon
{
    public List<string> bullets = new List<string>();
    public AnimationMod shootAnim = null;

    public Weapon weapon = null;
    public Weapon weaponDestroy = null;
}
