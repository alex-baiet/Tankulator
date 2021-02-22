using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {

    public VarScript Var;
    private float prec;

    private void Start()
    {
        LoadBullet();
    }
    void Update () {
        if (/*Input.GetKey("left ctrl") &&*/ Input.GetKeyDown("v"))
        {
            SaveBullet();
        }
        if (/*Input.GetKey("left ctrl") &&*/ Input.GetKeyDown("b"))
        {
            LoadBullet();
        }
        if (Input.GetKey("a") && Input.GetKey("z") && Input.GetKey("e") && Input.GetKey("r"))
        {
            DELETE();
        }

    }
    /// <summary>
    /// c test konar
    /// </summary>
    void SaveSomething()
    {
        print(Application.persistentDataPath);
    }
    
    void DELETE()
    {
        PlayerPrefs.DeleteAll();
    }
    void SaveBullet()
    {
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < (int)VarScript.Stat.Size; x++)
            {
                PlayerPrefs.SetFloat("weapon" + y.ToString() + "Stat" + x.ToString(), Var.Bullet[y, x]);
            }
        }
    }
    void LoadBullet()
    {
        for (int y = 0; y < (int)VarScript.Weapon.Size; y++)
        {
            for (int x = 0; x < (int)VarScript.Stat.Size; x++)
            {
                Var.Bullet[y,x] = PlayerPrefs.GetFloat("weapon" + y.ToString() + "Stat" + x.ToString());
            }
        }
    }
}
