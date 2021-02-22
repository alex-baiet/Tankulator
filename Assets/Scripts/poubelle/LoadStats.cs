using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStats : MonoBehaviour
{

    private SaveStat sav = new SaveStat();

    public int playerType = 0;
    public int player = 0;
    public int jump = 0;
    public int weapon = 0;
    public int bullet = 0;

    private void Start()
    {
        LoadLoad();
    }

    private void LoadLoad()
    {
        if (playerType != 0)
        {
            player = PlayerPrefs.GetInt("Player" + playerType.ToString() + "player");
            jump = PlayerPrefs.GetInt("Player" + playerType.ToString() + "jump");
            weapon = PlayerPrefs.GetInt("Player" + playerType.ToString() + "weapon");
            bullet = PlayerPrefs.GetInt("Player" + playerType.ToString() + "bullet");
        }
    }

    public void LoadPlayer(string nbr) { player = (int)sav.StrToFlt(nbr); }
    public void LoadJump(string nbr) { jump = (int)sav.StrToFlt(nbr); }
    public void LoadWeapon(string nbr) { weapon = (int)sav.StrToFlt(nbr); }
    public void LoadBullet(string nbr) { bullet = (int)sav.StrToFlt(nbr); }

}
