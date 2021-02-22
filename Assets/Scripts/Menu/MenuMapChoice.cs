using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//NewScript

public class MenuMapChoice : MonoBehaviour
{

    private class MapMenu
    {
        public string name;

        public Dictionary<char, Texture2D> textures = new Dictionary<char, Texture2D>();
        public Dictionary<char, Color> colors = new Dictionary<char, Color>();
        public Color[][] mapColors;

        public Dictionary<char, Texture2D> texturesShadows = new Dictionary<char, Texture2D>();
        public Dictionary<char, Color> colorsShadows = new Dictionary<char, Color>();
        public Color[][] mapColorsShadows;

        public Texture2D mapPreview;
    }

    [SerializeField] private GameObject buttonToAdd = null;
    [SerializeField] private GameObject mapButtons = null;  
    [SerializeField] private GameObject mapDisplay = null;
    [SerializeField] private Button buttonStart = null;

    private RawImage mapUI;
    private List<string> mapsName = new List<string>();
    private Dictionary<string, MapMenu> maps = new Dictionary<string, MapMenu>();

    void Start()
    {
        LoadMapsName();
        CreateAllButtons();
        //InitMapUI();
        mapUI = mapDisplay.GetComponent<RawImage>();
    }

    private Color GetAverageColor(Color colorBase, Color colorToBlit)
    {
        float abl = colorToBlit.a;
        //Debug.Log(abl);
        return new Color(
            colorBase.r * (1f - abl) + colorToBlit.r * abl,
            colorBase.g * (1f - abl) + colorToBlit.g * abl,
            colorBase.b * (1f - abl) + colorToBlit.b * abl,
            colorBase.a + (1f - colorBase.a) * abl);
    }

    private Color GetAverageColor(Texture2D texture)
    {
        float r = 0;
        float g = 0;
        float b = 0;
        float a = 0;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color px = texture.GetPixel(x, y);
                r += px.r * px.a;
                g += px.g * px.a;
                b += px.b * px.a;
                a += px.a;
            }
        }

        r /= a;
        g /= a;
        b /= a;
        a /= texture.width * texture.height;

        return new Color(r, g, b, a);
    }
    
    /// <summary>
    /// Créer tout les boutons pour le choix du terrain.
    /// </summary>
    private void CreateAllButtons()
    {
        Vector3 pos = mapButtons.transform.position;
        RectTransform rectTr = mapButtons.GetComponent<RectTransform>();
        rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, 30f + mapsName.Count * 35f);

        int i = 0;
        foreach (string mapName in mapsName)
        {
            LoadMap(mapName);
            NewButton("But" + mapName, new Vector3(pos.x - 55f / 12f, pos.y + (-30f - i * 35f) / 12f, 0));
            i++;
        }
    }

    private Texture2D CreateMapPreview(MapMenu map)
    {
        int maxWidth = 0;
        foreach (Color[] line in map.mapColors)
        {
            maxWidth = line.Length > maxWidth ? line.Length : maxWidth;
        }

        Texture2D preview = new Texture2D(256, 256);
        for (int i = 0; i < 9; i++)
        {
            int iPow = (int)Mathf.Pow(2, i);
            if (iPow >= maxWidth && iPow >= map.mapColors.Length)
            {
                preview = new Texture2D(iPow, iPow);
                break;
            }
            else if (i == 8)
            {
                preview = new Texture2D(iPow, iPow);
                Debug.LogWarning("La map semble être trop grande pour créer une preview dans le menu.");
            }
        }
        preview.filterMode = FilterMode.Point;

        int xgap = (preview.width - maxWidth) / 2;
        int ygap = (preview.width - map.mapColors.Length) / 2;

        for (int y = 0; y < map.mapColors.Length; y++)
        {
            for (int x = 0; x < map.mapColors[y].Length; x++)
            {
                if (y < map.mapColorsShadows.Length && x < map.mapColorsShadows[y].Length)
                { preview.SetPixel(x + xgap, -1 - (y + ygap), GetAverageColor(map.mapColors[y][x], map.mapColorsShadows[y][x]));}
                else { preview.SetPixel(x + xgap, - 1 - (y + ygap), map.mapColors[y][x]); }
            }
        }
        preview.name = map.name;
        preview.Apply();
        return preview;
    }

    public void DisplayMap(string mapName)
    {
        for (int x = 0; x < 25; x++)
        {
            for (int y = 0; y < 23; y++)
            {
                mapUI.texture = maps[mapName].mapPreview;
            }
        }
    }

    //Garbage
    private void InitMapUI()
    {
        for (int x = 0; x < 25; x++)
        {
            for (int y = 0; y < 23; y++)
            {
                //mapUI[x, y] = mapDisplay.transform.GetChild(x).GetChild(y).gameObject.GetComponent<RawImage>();
            }
        }
    }

    private void LoadMap(string mapName)
    {
        MapMenu map = new MapMenu();
        map.name = mapName;

        Dictionary<char, Texture2D> newTextures = new Dictionary<char, Texture2D>();
        Dictionary<char, Color> newColors = new Dictionary<char, Color>();

        map.textures = LoadMapMenuTextures(mapName, false);
        map.colors = LoadMapMenuColors(map.textures);
        map.mapColors = LoadMapMenuMapColors("Mods/Map/" + mapName + "/look.txt", map.colors);
        map.texturesShadows = LoadMapMenuTextures(mapName, true);
        map.colorsShadows = LoadMapMenuColors(map.texturesShadows);
        map.mapColorsShadows = LoadMapMenuMapColors("Mods/Map/" + mapName + "/shadow.txt", map.colorsShadows);

        map.mapPreview = CreateMapPreview(map);

        maps.Add(mapName, map);
    }

    private Dictionary<char, Texture2D> LoadMapMenuTextures(string mapName, bool loadingShadows)
    {
        string[] file = File.ReadAllLines("Mods/Map/" + mapName + (loadingShadows ? "/shadow_meaning.txt" : "/look_meaning.txt"));
        Dictionary<char, Texture2D> newTextures = new Dictionary<char, Texture2D>();

        foreach (string line in file)
        {
            string[] lineSplit = line.Split('=');
            newTextures.Add(lineSplit[0][0], StatAll.LoadTextureFromFile("Mods/Map/" + mapName + "/sprites/" + lineSplit[1]));
        }

        return newTextures;
    }

    private Dictionary<char, Color> LoadMapMenuColors(Dictionary<char, Texture2D> textures)
    {
        Dictionary<char, Color> newColors = new Dictionary<char, Color>();
        foreach (KeyValuePair<char, Texture2D> pairTexture in textures)
        {
            newColors.Add(pairTexture.Key, GetAverageColor(pairTexture.Value));
        }

        return newColors;
    }

    private Color[][] LoadMapMenuMapColors(string fileLookPath, Dictionary<char, Color> colors)
    {

        string[] look = File.ReadAllLines(fileLookPath);
        Color[][] mapColors = new Color[look.Length][];
        for (int y = 0; y < look.Length; y++)
        {
            mapColors[y] = new Color[look[y].Length];
            for (int x = 0; x < look[y].Length; x++)
            {
                if (colors.ContainsKey(look[y][x])) { mapColors[y][x] = colors[look[y][x]]; }
                else if (look[y][x] != ' ' && look[y][x] != '_') { Debug.LogWarning("Le symbole " + look[y][x] + " n'a pas de sprite assigné dans look_meaning.txt."); }
            }
        }

        return mapColors;
    }

    private void LoadMapsName()
    {
        foreach (string file in Directory.GetDirectories("Mods/Map"))
        {
            mapsName.Add(file.Replace(@"Mods/Map\", ""));
            //Debug.Log(file.Replace(@"Mods/Map\", ""));
        }
    }

    private void NewButton(string name, Vector3 pos)
    {
        buttonToAdd.name = name;
        GameObject newBut = Instantiate(buttonToAdd, pos, Quaternion.Euler(0f, 0f, 0f), mapButtons.transform);
        LoadMapButtonChoice lmbc = newBut.GetComponent<LoadMapButtonChoice>();

        newBut.name = name;
        lmbc.butStart = buttonStart;
        lmbc.menuMapChoice = this;
        
        //Debug.Log("position : " + pos.x + ", " + pos.y + ", " + pos.z);

    }
}
