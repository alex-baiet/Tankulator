using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//GARBAGE

public class VarScript : MonoBehaviour {

    [SerializeField]private short weaponNum;
    [SerializeField]private short statNum;
    public float[][][]Player;
    public float[,] Bullet;
    private int _y = 0;
    private int _x = 0;
    private float value = 0;



    public enum Weapon
    {
        Gun,
        Shotgun,
        AK47,
        Size = 100
    }
    
    public enum Stat
    {
        precision,
        fireRate,
        bulletSpeed,
        bulletSpeedVariation,
        bulletLife,
        bulletScale,
        bulletNumber,
        Size
    }




    //Balle
    /*public float precision; //precision du public float fireRate; //nombre de tirs par public float bulletSpeed; //vitesse des balles (unité force*300public float bulletSpeedVariation; //écart maximal de vitesse entre les balles (unité force*300public float bulletLife; //temps de vie des balles (secondepublic float bulletScale; //taille des ballespublic float bulletNumber; //nombre de balles par tir*/
    public Color bulletColor; //couleur des balles

    //Jump général
    public float timeBetweenJump; //délai entre chaque saut (seconde)
    public float afterimageLife; //temps de vie de l'image rémanente (seconde)
    public float afterimageAlpha; //transparence de départ de l'image rémanente (0 à 1)
    public bool jumpOnTime; //activer/désactiver le Jump sur un temps donné
    //Jump on time
    public float jumpTime; //temps du saut (seconde)
    public float jumpSpeed; //vitesse durant un Jump (unité*300)
    public float timeBetweenAfterimage; //Temps entre chaque images rémanentes (seconde)
    //Jump instantané
    public float jumpSize; //longueur du saut (unité Unity)
    public float afterimageNumber; //nombre d'image rémanente

    //Divers
    public float mouvementSpeed; //vitesse de déplacement (unité*300)
    //public Color playerColor; //couleur du joueur

    //Balle
    public void ChooseWeapon(int __y) { _y = __y; }
    public void ChooseStat(int __x) { _x = __x; }
    public void BulletStat(float _a) { Bullet[_y, _x] = _a; print(Bullet[_y, _x]); }

    public void Start()
    {
        Bullet = new float[100, (int)Stat.Size];
        for (_x = 0; _x < (int)Stat.Size; _x++)
        {
            for (_y = 0; _y < 100; _y++)
            {
                Bullet[_y, _x] = 1;
            }
        }
    }
 
    public void WeaponStrToInt(string number)
    {
        _y = 0;
        StrToInt(number, "_y");
    }
    public void StatStrToInt(string number)
    {
        _x = 0;
        StrToInt(number, "_x");
    }
    public void ChangeStat(string number)
    {
        StrToInt(number, "value");
        print(_y);
        print(_x);
        print(value);
        Bullet[_y, _x] = value;
        print(Bullet[_y, _x]);
    }
    /// <summary>
    /// Transform a string number into a float.
    /// </summary>
    /// <param name="number">Input the string number to transform</param>
    /// <param name="cible">Input the variable wich will have the result</param>
    private void StrToInt(string number, string cible)
    {
        float result = 0;
        float coef = 0.1f;
        bool coma = false;
        foreach (char item in number)
        {
            if (item == '.' || coma == true)
            {
                coma = true;
                goto deci;
            }
            switch (item)
            {
                case '0': result = result * 10; break;
                case '1': result = result * 10 + 1; break;
                case '2': result = result * 10 + 2; break;
                case '3': result = result * 10 + 3; break;
                case '4': result = result * 10 + 4; break;
                case '5': result = result * 10 + 5; break;
                case '6': result = result * 10 + 6; break;
                case '7': result = result * 10 + 7; break;
                case '8': result = result * 10 + 8; break;
                case '9': result = result * 10 + 9; break;
                default:  break;
            }
            continue;

            deci: switch (item)
            {
                case '0': coef = coef / 10; break;
                case '1': result = result + 1 * coef; goto case '0';
                case '2': result = result + 2 * coef; goto case '0';
                case '3': result = result + 3 * coef; goto case '0';
                case '4': result = result + 4 * coef; goto case '0';
                case '5': result = result + 5 * coef; goto case '0';
                case '6': result = result + 6 * coef; goto case '0';
                case '7': result = result + 7 * coef; goto case '0';
                case '8': result = result + 8 * coef; goto case '0';
                case '9': result = result + 9 * coef; goto case '0';
                default: break;
            }
        }
        if (cible == "_y") { _y = (int)result; }
        if (cible == "_x") { _x = (int)result; }
        if (cible == "value") { value = result; }
    }
}