using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevSaveStat : MonoBehaviour
{
    //Script inutilisé
    public int target;
    private Stat stat = new Stat();

    public class PlayerStat
    {
        public string name;
        public float life;
        public float speed;
        public string[] weaponsName = new string[2];
        public string specialName;

    }

    public class WeaponStat
    {
        public string name;
    }

    public class SpecialStat
    {
        public string name;
    }


    void Start()
    {

    }

}
