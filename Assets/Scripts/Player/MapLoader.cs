using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
//NewScript

public class MapLoader : MonoBehaviour
{
    public class Map
    {
        public string[] collision;
        public List<Vector2> spawnPoint;

        public GameObject[][] map;
        public string[] spritesPos;
        public Dictionary<char, Sprite> sprites;

        public GameObject[][] mapShadow;
        public string[] shadowsPos;
        public Dictionary<char, Sprite> shadows;

    }

    [SerializeField] private GameObject prefabWall = null;
    [SerializeField] private GameObject prefabGround = null;
    [SerializeField] private GameObject prefabShadow = null;
    [HideInInspector] public Dictionary<string, Map> maps = new Dictionary<string, Map>();
    [HideInInspector] public static Map actualMap;

    void Start()
    {
        string mapName = PlayerPrefs.GetString("map actual");
        if (!maps.ContainsKey(mapName)) { LoadMap(mapName); }
        DisplayMap(mapName);
    }


    private string[] LoadCollision(string path)
    {

        return File.ReadAllLines(path);
    }

    private void LoadMap(string name)
    {
        Map newMap = new Map();
        newMap.spawnPoint = LoadSpawn("Mods/Map/" + name + "/spawn.txt");
        newMap.collision = LoadCollision("Mods/Map/" + name + "/collision.txt");
        newMap.sprites = LoadSprites("Mods/Map/" + name + "/look_meaning.txt");
        newMap.spritesPos = LoadSpritesPosition("Mods/Map/" + name + "/look.txt");
        newMap.shadows = LoadShadows("Mods/Map/" + name + "/shadow_meaning.txt");
        newMap.shadowsPos = LoadShadowsPosition("Mods/Map/" + name + "/shadow.txt");

        maps.Add(name, newMap);
        actualMap = newMap;
    }

    private Dictionary<char, Sprite> LoadShadows(string path)
    {
        string[] file = File.ReadAllLines(path);
        Dictionary<char, Sprite> sprites = new Dictionary<char, Sprite>();

        foreach (string meaning in file)
        {
            string[] pair = meaning.Split('=');

            sprites.Add(pair[0][0], StatAll.LoadSpriteFromFile(path.Replace("/shadow_meaning.txt", "") + "/sprites/" + pair[1]));
        }
        return sprites;
    }

    private string[] LoadShadowsPosition(string path)
    {
        return File.ReadAllLines(path);
    }

    private List<Vector2> LoadSpawn(string path)
    {
        List<Vector2> newSpawnPoint = new List<Vector2>();
        string[] spawnText = File.ReadAllLines(path);


        for (int y = 0; y < spawnText.Length; y++)
        {
            for (int x = 0; x < spawnText[y].Length; x++)
            {
                if (spawnText[y][x] == '1')
                {
                    newSpawnPoint.Add(new Vector2(x + 0.5f, -(y + 0.5f)));
                }
            }
        }

        return newSpawnPoint;
    }

    /// <summary>
    /// Retourne les images pour le terrain.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Dictionary<char, Sprite> LoadSprites(string path)
    {
        string[] file = File.ReadAllLines(path);
        Dictionary<char, Sprite> sprites = new Dictionary<char, Sprite>();
        string folderPath = path.Replace("/look_meaning.txt", "");

        foreach (string meaning in file)
        {
            string[] pair = meaning.Split('=');
            string spritePath = folderPath + "/sprites/" + pair[1];
            if (File.Exists(spritePath))
            {
                sprites.Add(pair[0][0], StatAll.LoadSpriteFromFile(folderPath + "/sprites/" + pair[1]));
            }
            else { Debug.LogError("L'image " + spritePath + " n'existe pas."); }

        }
        return sprites;
    }

    private string[] LoadSpritesPosition(string path)
    {
        return File.ReadAllLines(path);
    }

    private GameObject[][] CreateMap()
    {
        GameObject[][] map = new GameObject[actualMap.collision.Length][];
        for (int i = 0; i < actualMap.collision.Length; i++)
        {
            map[i] = new GameObject[actualMap.collision[i].Length];
        }
        Vector3 pos;

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                pos = new Vector3(x + 0.5f, -(y + 0.5f));
                if (actualMap.collision[y][x] == '1') { map[y][x] = Instantiate(prefabWall, pos, transform.rotation, transform); }
                else { map[y][x] = Instantiate(prefabGround, pos, transform.rotation, transform); }

                char chara = actualMap.spritesPos[y][x];
                if (actualMap.sprites.ContainsKey(chara))
                {
                    map[y][x].GetComponent<SpriteRenderer>().sprite = actualMap.sprites[chara];
                }
                else if (chara != ' ' && chara != '_') { Debug.LogError("Le caractère " + chara + " n'a pas d'image assignée dans look_meaning.txt."); }
            }
        }
        return map;
    }

    private GameObject[][] CreateMapShadows()
    {
        GameObject[][] mapsh = new GameObject[actualMap.shadowsPos.Length][];
        for (int i = 0; i < mapsh.Length; i++)
        {
            mapsh[i] = new GameObject[actualMap.shadowsPos[i].Length];
        }
        Vector3 pos;

        for (int y = 0; y < mapsh.Length; y++)
        {
            for (int x = 0; x < mapsh[y].Length; x++)
            {
                pos = new Vector3(x + 0.5f, -(y + 0.5f));
                mapsh[y][x] = Instantiate(prefabShadow, pos, Quaternion.Euler(Vector3.zero), transform);

                char chara = actualMap.shadowsPos[y][x];
                if (actualMap.shadows.ContainsKey(chara))
                {
                    mapsh[y][x].GetComponent<SpriteRenderer>().sprite = actualMap.shadows[chara];
                }
                else if (chara != ' ' && chara != '_') { Debug.LogError("Le caractère " + chara + " n'a pas d'image assignée dans shadow_meaning.txt."); }
            }
        }
        return mapsh;
    }

    private void DisplayMap(string mapName)
    {
        actualMap = maps[mapName];
        actualMap.map = CreateMap();
        actualMap.mapShadow = CreateMapShadows();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = actualMap.spawnPoint[i];
        }
    }
}
