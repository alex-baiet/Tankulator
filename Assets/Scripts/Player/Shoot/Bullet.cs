using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class Bullet : MonoBehaviour
{
    public StatBulletLoaded statBullet;
    public int ownerId;
    public string ownerName;
    private float rotation;
    private float speed = 1f;
    private float clock = 0;
    private GameObject[] players = new GameObject[2];
    private Player[] playersClass = new Player[2];
    private SpriteRenderer spriteRenderer;
    private bool alreadyDestroyed = false;
    private float colliderScale;
    private AnimationMod destroyedAnim;
    private AnimationMod anim;
    private Weapon weapon = null;
    private Weapon weaponDestroy;
    private GameObject bulletBase;
    private Stat stat;

    private void Update()
    {
        clock += Time.deltaTime * Clock.timeFlowBullet[ownerId];

        if (!alreadyDestroyed)
        {
            CheckCollision();
            ChangeAlpha();
            Shoot();
            anim.UpdateAnimation();
        }
        else
        {
            destroyedAnim.UpdateAnimation();
            if (destroyedAnim.CheckAnimationEnded()) { Destroy(gameObject); }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!alreadyDestroyed)
        {
            speed += statBullet.speedUp * Time.fixedDeltaTime * Clock.timeFlowBullet[ownerId];
            speed = speed < statBullet.speedMin ? statBullet.speedMin :
                speed > statBullet.speedMax ? statBullet.speedMax :
                speed;

            transform.position += new Vector3(Mathf.Cos(Mathf.Deg2Rad * rotation), 
                Mathf.Sin(Mathf.Deg2Rad * rotation)) * speed * Time.fixedDeltaTime * Clock.timeFlowBullet[ownerId];
        }
    }

    private void ChangeAlpha()
    {
        float dp = StatAll.MathDistance(players[0].transform.position, players[1].transform.position); //distance joueur/joueur
        float db = StatAll.MathDistance(players[ownerId].transform.position, transform.position); //distance proprio/balle
        float dc = db / dp; //Coefficient de distance
        float[] d = new float[2];

        if (dc <= 0.4f) { spriteRenderer.color = new Color(1f, 1f, 1f, 0.1f); }
        else if (0.4f < dc && dc < 0.6f) { spriteRenderer.color = new Color(1f, 1f, 1f, 0.1f + (dc - 0.4f) * 4.5f); }
        else { spriteRenderer.color = new Color(1f, 1f, 1f, 1f); }

    }

    private void CheckCollision()
    {
        bool isDestroyed = false;

        if (clock > statBullet.lifeTime) { isDestroyed = true; }

        int x = (int)(transform.position.x);
        int y = (int)(-transform.position.y);
        if (x < 0 || y < 0 || y >= MapLoader.actualMap.collision.Length || x >= MapLoader.actualMap.collision[y].Length ||
            MapLoader.actualMap.collision[y][x] == '1')
        {
            isDestroyed = true;
        }

        for (int i = 0; i < 2; i++)
        {
            if (i != ownerId)
            {
                float d = Mathf.Sqrt(
                    Mathf.Pow(transform.position.x - players[i].transform.position.x, 2) +
                    Mathf.Pow(transform.position.y - players[i].transform.position.y, 2));
                if (d < (colliderScale + playersClass[i].stat.player.colliderRadius * playersClass[i].special.bombScale) / 24) {
                    DamagePlayer(players[i].GetComponent<Player>());
                    isDestroyed = true;
                }
            }
        }

        if (isDestroyed && !alreadyDestroyed)
        {
            DestroyBullet();
        }
    }

    private void DamagePlayer(Player player)
    {
        if (!alreadyDestroyed) { player.life -= statBullet.damage * player.special.bombDamage; }
        else { player.life -= statBullet.explosionDamage; }
    }

    private void DestroyBullet()
    {
        alreadyDestroyed = true;
        if (statBullet.explosionDamage != 0)
        {
            colliderScale = statBullet.explosionRadius;
            CheckCollision();
        }
        ShootDestroy();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        anim.StopAnimation();
        destroyedAnim.StartAnimation();
        destroyedAnim.UpdateAnimation();
        if (destroyedAnim.CheckAnimationEnded()) { Destroy(gameObject); }

    }

    public void Init(StatBulletLoaded statBullet, int ownerId, string ownerName, GameObject bulletBase, Stat stat)
    {
        this.statBullet = statBullet;
        destroyedAnim = new AnimationMod(this.statBullet.destroyedAnim);
        anim = new AnimationMod(this.statBullet.anim);
        this.ownerId = ownerId;
        this.ownerName = ownerName;
        this.stat = stat;

        rotation = transform.rotation.eulerAngles.z;
        if (!statBullet.isRotating) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (statBullet.sprite != null)
        {
            spriteRenderer.sprite = statBullet.sprite;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f);
        }

        speed = statBullet.speed + Random.Range(-statBullet.speedVar, statBullet.speedVar);
        colliderScale = statBullet.colliderRadius;
        destroyedAnim.Init(spriteRenderer, null, "B" + ownerId);
        anim.Init(spriteRenderer, null, "B" + ownerId);
        anim.StartRepeatingAnimation();

        for (int i = 0; i < 2; i++)
        {
            players[i] = GameObject.Find("Player" + (i + 1));
            playersClass[i] = players[i].GetComponent<Player>();

        }

        this.bulletBase = bulletBase;

        weapon = new Weapon();
        weaponDestroy = new Weapon();

        if (stat.weapons.ContainsKey(statBullet.weaponName)) 
        { weapon.Init(stat.weapons[statBullet.weaponName], bulletBase, 0, ownerId, null, transform, stat); }
        if (stat.weapons.ContainsKey(statBullet.weaponDestroyName)) 
        { weaponDestroy.Init(stat.weapons[statBullet.weaponDestroyName], bulletBase, 0, ownerId, null, transform, stat); }
    }

    private void Shoot()
    {
        if (weapon.initialized)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            weapon.UpdateClock(Time.deltaTime * Clock.timeFlowBullet[ownerId]);
            weapon.Shoot(true);
            if (!statBullet.isRotating)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void ShootDestroy()
    {
        if (weaponDestroy.initialized)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            weaponDestroy.ShootForced();
        }

    }
}
