using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorJoystick : MonoBehaviour {

    private VarScript var;
    [SerializeField] private float distanceCursor = 3;
    [SerializeField] private Transform owner = null;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        float _x = Input.GetAxisRaw("HorizontalJoystickAim");
        float _y = Input.GetAxisRaw("VerticalJoystickAim");
        float dCursor = Mathf.Sqrt(Mathf.Pow(_x, 2) + Mathf.Pow(_y, 2));
        if (dCursor < 1) { dCursor = 1; }
        transform.position = new Vector2(owner.position.x + _x * distanceCursor/dCursor, owner.position.y + _y * distanceCursor/dCursor);
        transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);

        if (Clock.isPaused) { GetComponent<SpriteRenderer>().enabled = false; }
        else { GetComponent<SpriteRenderer>().enabled = true; }
    }
    
}
