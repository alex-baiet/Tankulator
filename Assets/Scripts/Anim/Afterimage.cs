using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    [HideInInspector] public AfterimageVar AIVar = null;
    new private SpriteRenderer renderer = null;
    private string timeName = null;
    private float clock = 0f;
    private float originalAlpha = 1f;
    
    void Update()
    {
        clock += Time.deltaTime * Clock.GetTimeFlow(timeName);

        float coef = 1 - clock / AIVar.length;
        if (coef > 0)
        {
            float coefAlpha = AIVar.alphaStart * originalAlpha * coef;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, coefAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init(string timeName, AfterimageVar afterimageVar, SpriteRenderer parentRenderer)
    {
        AIVar = afterimageVar;
        this.timeName = timeName;
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = parentRenderer.sprite;
        originalAlpha = parentRenderer.color.a;
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, AIVar.alphaStart * originalAlpha);
    }


}
