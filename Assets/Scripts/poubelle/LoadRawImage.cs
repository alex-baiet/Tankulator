using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadRawImage : MonoBehaviour
{

    void Start()
    {
        LoadSpriteFromFile();
    }

    public void LoadSpriteFromFile()
    {
        byte[] bytes = File.ReadAllBytes("Mods/ImageTest.png");
        Texture2D texture = new Texture2D(100, 100);
        texture.LoadImage(bytes);
        gameObject.GetComponent<RawImage>().texture = texture;
    }
}
