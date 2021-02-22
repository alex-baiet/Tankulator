using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class StatPlayer
{
    public string name;
    public string spritePath;
    public string[] weaponsName = new string[2];
    public string specialName;

    public float life = 100f;
    public float speed = 3f;
    public float colliderRadius = 0f;
    public float colliderObjectRadius = 0f;
    public float shootDistance = 6f;
}
