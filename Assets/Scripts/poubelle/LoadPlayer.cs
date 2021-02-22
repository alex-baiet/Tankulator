using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadPlayer : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Sprite image;
    private Texture2D texture;
    private byte[] data;
    public string SpriteName;
    [HideInInspector] public int player = 0;
    [HideInInspector] public int[] weapon = new int[2];
    [HideInInspector] public List<int> bullet;

    void Start()
    {
        
        if (File.Exists(Directory.GetCurrentDirectory() + @"\Mods\" + SpriteName + ".png"))
        {
            //print("directoryTarget");
            data = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\Mods\" + SpriteName + ".png");
            texture = new Texture2D(1, 1);
            texture.LoadImage(data);
            //texture = Resources.Load<Texture2D>("ppng");
            texture.filterMode = FilterMode.Point;
            image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 12);

            sprite = GetComponent<SpriteRenderer>();
            sprite.sprite = image;

        }

    }
}
