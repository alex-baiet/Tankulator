using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveTest : MonoBehaviour
{
    public class PlayerData
    {
        public string name;
        public float power;
        public int level;
    }
    void Start()
    {
        SavePlayerData();
        //LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayerData()
    {
        if (!Directory.Exists("Mods"))
        {
            Debug.Log("Creation d'un nouveau dossier");
            Directory.CreateDirectory("Mods");

        }
        PlayerData pd = new PlayerData
        {
            name = "Jammy ULTRA INSTINCT",
            power = 9000f,
            level = 999
        };
        string json = JsonUtility.ToJson(pd);
        print(json);
        print(Application.persistentDataPath);
        File.WriteAllText("Mods/PLAYERPLAYER.json", json);
        PlayerData pd2 = new PlayerData();
        pd2.name = "HAJLKHJKHKJLHZEK";
        pd2 = JsonUtility.FromJson<PlayerData>(json);
        print(pd2.name);
    }

    public void LoadPlayerData()
    {

        string save = File.ReadAllText("Mods/PLAYERPLAYER.json");
        Debug.Log("save : " + save);
    }
}
