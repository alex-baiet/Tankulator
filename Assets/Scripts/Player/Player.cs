using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bulletBase = null;
    [HideInInspector] public int playerId;
    [SerializeField] private Slider slider = null;
    [HideInInspector] public int weaponId = 0;
    private bool weaponChanged = false;
    [SerializeField] private GameObject shootAnimObject = null;
    private float diag;
    private Weapon[] weapons;
    private float timeFlow;
    [SerializeField] private Transform specialSlider = null;
    [HideInInspector] public AnimationMod anim = new AnimationMod();

    [HideInInspector] public Stat stat;
    [HideInInspector] public StatBulletLoaded statBullet;
    [HideInInspector] public Sprite spriteBullet;
    [HideInInspector] public float life;
    [HideInInspector] public Special special;

    [HideInInspector] public bool inJump = false;
    public bool useKeyboard = true;
    public bool createMode = false; //GARBAGE
    [HideInInspector] public LoadStats load; //GARBAGE


    //Divers
    public float mouvementSpeed = 1; //GARBAGE
    public int firstWeapon = 1; //GARBAGE
    public int secondWeapon = 2; //GARBAGE



    void Awake()
    {
        LoadPlayer();
        
    }

    private void Update()
    {
        timeFlow = Time.deltaTime * Clock.timeFlowPlayer[playerId];
        UseWeapon();
        if (slider != null) { slider.value = life; }
        ChangeWeapon();
        special.SpecialUse(useKeyboard);
        anim.UpdateAnimation();
        special.anim.UpdateAnimation();
        special.animHover.UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (inJump == false) { Mouvement(); }
    }

    /// <summary>
    /// Change l'arme choisie si le joueur le veut (change la valeur weaponId)
    /// </summary>
    private void ChangeWeapon()
    {
        if (InputPlayer.GetAxisChangeWeapon(useKeyboard) != 0)
        {
            if (!weaponChanged)
            {
                weapons[weaponId].StopAnimation();
                weapons[weaponId].active = false;

                weaponId += InputPlayer.GetAxisChangeWeapon(useKeyboard) > 0 ? 1 : -1;
                if (weaponId >= weapons.Length) { weaponId = 0; }
                if (weaponId < 0) { weaponId = weapons.Length - 1; }
                weaponChanged = true;

                weapons[weaponId].Reinit();
            }
        }
        else { weaponChanged = false; }
    }

    /// <summary>
    /// Deplacement basique du joueur
    /// </summary>
    /// <param name="inputx">L'axe de déplacement x</param>
    /// <param name="inputy">L'axe de déplacement y</param>
    private void Mouvement()
    {
        float axisx = InputPlayer.GetAxisHorizontal(useKeyboard);
        float axisy = InputPlayer.GetAxisVertical(useKeyboard);

        if (Mathf.Abs(axisx) == 1 && Mathf.Abs(axisy) == 1) { diag = 0.7071f; } else { diag = 1f; }
        float finalSpeed = stat.player.speed * diag * Time.fixedDeltaTime * Clock.timeFlowPlayer[playerId] * special.speed;
        transform.position += new Vector3(
            axisx * finalSpeed,
            axisy * finalSpeed);
    }

    /// <summary>
    /// Initialise le joueur.
    /// </summary>
    public void LoadPlayer()
    {
        

        playerId = int.Parse(gameObject.name.Replace("Player", ""))-1;

        string namep = PlayerPrefs.GetString("P" + gameObject.name.Replace("Player", ""));
        //Debug.Log(name + " : " + namep);
        stat = StatAll.stats[namep];

        if (stat.player.sprite != null) { transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = stat.player.sprite; }
        else {
            anim = new AnimationMod(stat.player.anim);
            anim.Init(transform.Find("Sprite").GetComponent<SpriteRenderer>(), null, "P" + playerId);
            anim.StartRepeatingAnimation();
            transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = anim.GetFirstSprite();
        }

        gameObject.GetComponent<CircleCollider2D>().radius = stat.player.colliderObjectRadius / 24;

        life = stat.player.life;

        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = stat.player.life;
            slider.value = life;
        } else { Debug.LogWarning("Le " + name + " n'a pas de barre de vie."); }

        special = new Special(this, transform.Find("SpriteSpecial").GetComponent<SpriteRenderer>(), specialSlider);

        shootAnimObject = Instantiate(shootAnimObject, 
            new Vector3(transform.position.x + stat.player.shootDistance / 24, transform.position.y), Quaternion.Euler(0, 0, 0), transform);
        //stat.weapons[weaponId].shootAnim.SetRendererTarget(shootAnimObject.GetComponent<SpriteRenderer>());

        weapons = new Weapon[stat.player.weaponsName.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = new Weapon();
            weapons[i].Init(stat.weapons[stat.player.weaponsName[i]], bulletBase, stat.player.shootDistance, playerId, shootAnimObject.GetComponent<SpriteRenderer>(), transform, stat);
        }
    }
    
    private void UseWeapon()
    {
        for (int i = 0; i < weapons.Length; i++) { weapons[i].UpdateClock(timeFlow); }
        weapons[weaponId].Shoot(InputPlayer.GetButtonShoot(useKeyboard));
        weapons[weaponId].UpdateAnimation(timeFlow);
    }

}
