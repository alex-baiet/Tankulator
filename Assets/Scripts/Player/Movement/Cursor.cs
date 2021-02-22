using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0, 0, 0);
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.position = new Vector3 (MousePosition.x , MousePosition.y,0);
        transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
    }
}
