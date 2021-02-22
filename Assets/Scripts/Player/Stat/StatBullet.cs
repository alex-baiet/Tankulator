using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class StatBullet
{
    public string name;
    public string spritePath = "";

    public float speed = 4f;
    public float speedVar = 0f;
    public float speedMin = float.MinValue;
    public float speedMax = float.MaxValue;
    public float speedUp = 0f;

    public float lifeTime = 60f;
    public float colliderRadius = 1f;
    public float damage = 10f;

    public bool isRotating = false;

    public string destroyedAnimPath = "";
    public float explosionRadius = 0f;
    public float explosionDamage = 0f;

    public string weaponName = "";
    public string weaponDestroyName = "";


}
