using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Garbage
public class AfterimageScript : MonoBehaviour {
    
    private SpriteRenderer sprite;
    private float spawn;
    private float _a;
    private Jump parent;
    
  
    
	void Awake ()
    {
        sprite = GetComponent<SpriteRenderer>();
        parent = GetComponentInParent<Jump>();
        sprite.sprite = parent.spriteParent.sprite;
        sprite.color = new Color(parent.spriteParent.color.r, parent.spriteParent.color.g, parent.spriteParent.color.b, parent.spriteParent.color.a);
        spawn = Time.time + parent.afterimageLife;
        _a = sprite.color.a * parent.afterimageAlpha;
        transform.SetParent(null);
        if (parent.inJump == false)
        {
            if (transform.position != parent.lastPosition)
            {
                transform.position = (parent.newPosition - parent.lastPosition) * parent.n / (parent.afterimageNumber - 1) + parent.lastPosition;
            }
        }
    }
	
	void Update ()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (spawn-Time.time)*_a/ parent.afterimageLife);
        Destroy(gameObject, parent.afterimageLife);
	}
}
