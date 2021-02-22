using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//NewScript
//LoadSpriteFromFile() existant dans StatAll.
//GARBAGE
public class LoadSprite : MonoBehaviour
{

    void Start()
    {
        LoadSpriteFromFile();
    }

    public void LoadSpriteFromFile()
    {
        byte[] bytes = File.ReadAllBytes("Mods/ImageTest.png");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        texture.filterMode = FilterMode.Point;
        Debug.Log(texture.width);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 12f);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
