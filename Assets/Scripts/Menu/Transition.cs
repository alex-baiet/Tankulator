using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

    [SerializeField] private float fadeTime = 1;
    private float timer;
    private SpriteRenderer sprite;

	void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        timer += Time.deltaTime;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (fadeTime - timer) / fadeTime);
        Destroy(gameObject, fadeTime);
	}
}
