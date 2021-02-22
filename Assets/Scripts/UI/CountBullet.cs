using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountBullet : MonoBehaviour
{
    public Text text;
    private int totalBul;


    void Update()
    {
        totalBul = transform.childCount;
        text.text = "bullet nbr : " + totalBul;
    }
}
