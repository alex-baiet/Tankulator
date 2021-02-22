using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Special
{
    private Player player;
    private Transform specialSlider;
    private Slider sliderReload;
    private Slider sliderLoad;
    [HideInInspector] public StatSpecialLoaded statSpecial;
    private float lengthUseRemain;
    private float lengthReloadRemain;
    private float lengthBomb;
    private int loadNbr = 0;
    private bool inUse = false;
    [HideInInspector] public Player enemy;
    [HideInInspector] public AnimationMod anim = null;
    [HideInInspector] public AnimationMod animHover = null;

    [HideInInspector] public float speed = 1f;

    [HideInInspector] public float timePlayer = 1f;
    [HideInInspector] public float timeEnnemi = 1f;
    [HideInInspector] public float timeBulletPlayer = 1f;
    [HideInInspector] public float timeBulletEnnemi = 1f;

    [HideInInspector] public float bombScale = 1f;
    [HideInInspector] public float bombDamage = 1f;

    public Special(Player p, SpriteRenderer spriteRenderer, Transform specialSlider)
    {
        player = p;
        statSpecial = p.stat.specials[p.stat.player.specialName];
        char l = player.name == "Player1" ? '2' : '1';
        enemy = GameObject.Find("Player" + l).GetComponent<Player>();
        loadNbr = statSpecial.loadNbrCharged;
        lengthReloadRemain = statSpecial.lengthReload;

        if (statSpecial.isInfinite) { loadNbr = statSpecial.loadNbr; }

        anim = new AnimationMod(statSpecial.anim);
        anim.Init(spriteRenderer, spriteRenderer.sprite, "P" + player.playerId);
        if (player.anim != null) { player.anim.StartRepeatingAnimation(); }
        animHover = new AnimationMod(statSpecial.animHover);
        SpriteRenderer hoverRenderer = p.transform.Find("Hover Animation").GetComponent<SpriteRenderer>();
        animHover.Init(hoverRenderer, null, "P" + player.playerId);

        this.specialSlider = specialSlider;
        LoadSlider();
        SetValues(true);
    }

    private void LoadSlider()
    {
        sliderReload = specialSlider.GetChild(0).GetComponent<Slider>();
        sliderReload.minValue = 0f;
        sliderReload.maxValue = statSpecial.loadNbr;

        sliderLoad = specialSlider.GetChild(1).GetComponent<Slider>();
        sliderLoad.minValue = 0f;
        sliderLoad.maxValue = statSpecial.loadNbr;
    }

    public void SetValues() { SetValues(false); }

    public void SetValues(bool initialize)
    {
        if (inUse)
        {
            speed = statSpecial.useFixedSpeed ? statSpecial.speed / player.stat.player.speed : statSpecial.speed;

            timePlayer = statSpecial.timePlayer;
            timeEnnemi = statSpecial.timeEnnemi;
            timeBulletPlayer = statSpecial.timeBulletPlayer;
            timeBulletEnnemi = statSpecial.timeBulletEnnemi;

            lengthBomb = statSpecial.lengthBomb * statSpecial.timePlayer;
            bombScale = statSpecial.useFixedBombScale ? statSpecial.bombScale / player.stat.player.colliderRadius : statSpecial.bombScale;
            bombDamage = lengthBomb > 0 ? 0 : 1;

            player.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
            player.anim.StopAnimation();
            anim.StartRepeatingAnimation();
            animHover.StartRepeatingAnimation();
        }
        else
        {
            speed = 1;

            timePlayer = 1;
            timeEnnemi = 1;
            timeBulletPlayer = 1;
            timeBulletEnnemi = 1;

            lengthBomb = 0;
            bombScale = 1;
            bombDamage = 1;

            player.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
            player.anim.StartRepeatingAnimation();
            anim.StopAnimation();
            animHover.StopAnimation();
        }

        if (!initialize)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == player.playerId)
                {
                    Clock.timeFlowPlayer[i] = timePlayer * enemy.special.timeEnnemi;
                    Clock.timeFlowBullet[i] = timeBulletPlayer * enemy.special.timeBulletEnnemi;
                }
                else
                {
                    Clock.timeFlowPlayer[i] = timeEnnemi * enemy.special.timePlayer;
                    Clock.timeFlowBullet[i] = timeBulletEnnemi * enemy.special.timeBulletPlayer;
                }
            }
        }
    }

    private void SpecialUI()
    {
        if (statSpecial.isInfinite)
        {
            sliderReload.value = loadNbr;
            sliderLoad.value = loadNbr;
        }
        else
        {
            float lengthCoefUse = statSpecial.useSingleFrame ? 0 :
                statSpecial.length > 0 ? lengthUseRemain / statSpecial.length : 1;
            float lengthCoefReload = statSpecial.lengthReload > 0 ? lengthReloadRemain / statSpecial.lengthReload : 1;

            sliderReload.value = loadNbr + lengthCoefUse + (1 - lengthCoefReload);
            sliderLoad.value = loadNbr + lengthCoefUse;
        }
    }

    public void SpecialUse(bool useKeyboard)
    {
        bool useThisFrame = false;

        if (((InputPlayer.GetButtonSpecialDown(useKeyboard) && !inUse) ||
            (InputPlayer.GetButtonSpecial(useKeyboard) && statSpecial.useContinuously && !inUse))
            && loadNbr > 0 && !Clock.isPaused)
        {
            inUse = true;
            loadNbr -= statSpecial.isInfinite ? 0 : 1;
            lengthUseRemain = statSpecial.length;
            useThisFrame = statSpecial.useSingleFrame ? true : false;
            SetValues();
        }

        SpecialUI();

        if (inUse)
        {
            if (statSpecial.useSingleFrame)
            {
                inUse = useThisFrame;
                SetValues();
                inUse = false;
            }
            else
            {
                lengthBomb -= Time.deltaTime * Clock.timeFlowPlayer[player.playerId];
                if (lengthBomb < 0)
                {
                    bombDamage = 1;
                    bombScale = 1;
                }

                lengthUseRemain -= Time.deltaTime * Clock.timeFlowPlayer[player.playerId];
                while (lengthUseRemain < 0)
                {
                    if(InputPlayer.GetButtonSpecial(useKeyboard) && statSpecial.useContinuously && loadNbr > 0)
                    {
                        lengthUseRemain += statSpecial.length;
                        loadNbr--;
                    }
                    else
                    {
                        lengthUseRemain = 0;
                        inUse = false;
                        SetValues();
                    }
                }
            }
        }
        else if (loadNbr < statSpecial.loadNbr)
        {
            lengthReloadRemain -= Time.deltaTime * Clock.timeFlowPlayer[player.playerId];
            if (statSpecial.lengthReload > 0)
            {
                while (lengthReloadRemain <= 0 && loadNbr < statSpecial.loadNbr)
                {
                    loadNbr++;
                    lengthReloadRemain += statSpecial.lengthReload;
                }
                if(loadNbr == statSpecial.loadNbr) { lengthReloadRemain = statSpecial.lengthReload; }
            }
            else
            {
                loadNbr = statSpecial.loadNbr;
            }
        }
    }

}
