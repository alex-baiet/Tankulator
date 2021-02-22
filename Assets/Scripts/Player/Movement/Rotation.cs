using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Clock.isPaused)
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //modifie la rotation en fonction de la camera et de la souris
            float rotationZ = Mathf.Atan2(MousePosition.y, MousePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
    }
}
