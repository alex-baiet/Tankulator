using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {
    private Transform cursorPos;

	// Use this for initialization
	void Start () {
        cursorPos = transform.Find("Cursor");
    }
	
	// Update is called once per frame
	void Update () {
        float rotationZ = Mathf.Atan2(cursorPos.position.x - transform.position.x,cursorPos.position.y - transform.position.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, 0 - rotationZ + 90);
    }
}
