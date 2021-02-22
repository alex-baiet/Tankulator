using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class StatSpecial
{
    public string name;

    public string animPath = "";
    public string animHoverPath = "";

    public float length = 1f;
    public float lengthReload = 1f;
    public int loadNbr = 1;
    public int loadNbrCharged = 0;

    public bool useContinuously = false;
    public bool isInfinite = false;
    public bool useSingleFrame = false;

    public bool useFixedSpeed = false;
    public float speed = 1f;

    public float timePlayer = 1f;
    public float timeEnnemi = 1f;
    public float timeBulletPlayer = 1f;
    public float timeBulletEnnemi = 1f;

    public bool useFixedBombScale = false;
    public float bombScale = 1f;
    public float lengthBomb = 0f;
}
