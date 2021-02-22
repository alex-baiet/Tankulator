using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//NewScript

public class Stat
{
    /*public StatPlayer playerStat;
    public StatWeapon[] weaponsStat = new StatWeapon[2];
    public StatSpecial specialStat;
    public Dictionary<string, StatBullet> bulletsStat = new Dictionary<string, StatBullet>();*/

    public StatPlayerLoaded player;
    public Dictionary<string, StatWeaponLoaded> weapons = new Dictionary<string, StatWeaponLoaded>();
    public Dictionary<string, StatSpecialLoaded> specials = new Dictionary<string, StatSpecialLoaded>();
    public Dictionary<string, StatBulletLoaded> bullets = new Dictionary<string, StatBulletLoaded>();

    public string path;
    
    
}
