using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon {
    public bool initialized = false;

    private StatWeaponLoaded statWeapon;
    private Transform transformParent;
    private AnimationMod animShoot;
    private Stat stat;

    private float clockLastShoot;
    private float clock;
    private int ammoId;
    private float rotWeapon;
    private GameObject bulletBase;
    private float shootDistance;
    private int playerId;
    private int rotWeaponCoef;
    [HideInInspector] public bool active = false;

    private void ChangeWeaponRotation()
    {
        
        if (statWeapon.rotationSpeed != 0)
        {
            Debug.Log(rotWeapon);
            rotWeapon += statWeapon.rotationSpeed / statWeapon.rateOfFire * rotWeaponCoef;
            Debug.Log(rotWeapon);
            if (!(rotWeapon >= statWeapon.rotationStart && rotWeapon <= statWeapon.rotationEnd) &&
                !(rotWeapon <= statWeapon.rotationStart && rotWeapon >= statWeapon.rotationEnd))
            {
                if (statWeapon.rotationMirror)
                {
                    rotWeaponCoef = -rotWeaponCoef;
                    rotWeapon += statWeapon.rotationSpeed / statWeapon.rateOfFire * rotWeaponCoef * 2;
                }
                else
                {
                    rotWeapon = statWeapon.rotationStart;
                }
            }
        }
    }

    public void Init(StatWeaponLoaded statWeapon, GameObject bulletBaseObject, float shootDistance, int playerId, SpriteRenderer toAnimate, Transform transformToFollow, Stat stat)
    {
        initialized = true;
        this.statWeapon = statWeapon;
        bulletBase = bulletBaseObject;
        this.shootDistance = shootDistance;
        this.playerId = playerId;
        animShoot = new AnimationMod(statWeapon.shootAnim);
        animShoot.Init(toAnimate, null, "P" + playerId);
        transformParent = transformToFollow;
        this.stat = stat;

        Reinit();
    }

    public void Reinit()
    {
        ammoId = 0;
        //clock = 0;
        if (clockLastShoot < clock) { clockLastShoot = clock; };
        rotWeapon = statWeapon.rotationStart;
        rotWeaponCoef = 1;
        active = true;
    }

    public void UpdateClock(float timeFlow)
    {
        if (active || statWeapon.alwaysReloading)
        {
            clock += timeFlow;
        }
    }

    public void UpdateAnimation(float timeFlow)
    {
        animShoot.UpdateAnimation();
    }

    public void Shoot(bool input)
    {
        StatBulletLoaded statBullet;
        if (input)
        {
            while (clockLastShoot <= clock)
            {
                if (statWeapon.randomAmmoOrder)
                {
                    ammoId = Random.Range(0, statWeapon.bullets.Count);
                }
                else
                {
                    ammoId++;
                    ammoId -= ammoId == statWeapon.bullets.Count ? statWeapon.bullets.Count : 0;
                }

                if (!statWeapon.shootAnimRepeat) { animShoot.StartAnimation(); }
                statBullet = stat.bullets[statWeapon.bullets[ammoId]];


                for (int i = 0; i < statWeapon.bulletNbr; i++)
                {
                    float rotRad = (transformParent.rotation.eulerAngles.z) * Mathf.Deg2Rad;
                    float rotBullet = statWeapon.orderedInaccuracy && statWeapon.bulletNbr > 1 ?
                        statWeapon.inaccuracy * (i / (statWeapon.bulletNbr - 1f) - 0.5f) + rotWeapon :
                        statWeapon.inaccuracy * (Random.value - 0.5f) + rotWeapon;

                    GameObject newBullet = Object.Instantiate(bulletBase,
                        transformParent.position + new Vector3(Mathf.Cos(rotRad) * shootDistance / 24, Mathf.Sin(rotRad) * shootDistance / 24),
                        Quaternion.Euler(transformParent.rotation.eulerAngles + new Vector3(0, 0, rotBullet)),
                        GameObject.Find("Bullets").transform);

                    newBullet.GetComponent<Bullet>().Init(statBullet, playerId, "Player"+(playerId+1), bulletBase, stat);
                }
                clockLastShoot += 1 / statWeapon.rateOfFire;

                ChangeWeaponRotation();
            }

            if (statWeapon.shootAnimRepeat) { animShoot.StartRepeatingAnimation(); }
        }
        else
        {
            if (clockLastShoot < clock) { clockLastShoot = clock; }
            if (statWeapon.shootAnimRepeat) { animShoot.StopAnimation(); }
        }
    }

    public void ShootForced()
    {
        clockLastShoot = clock;
        Shoot(true);
    }

    public void StopAnimation()
    {
        animShoot.StopAnimation();
    }
}
