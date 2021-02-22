using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float cs = 3f; //constant taille de la camera
    private float ps = 0.5f; //produit taille de la camera
    private Camera cam;
    [SerializeField] private Transform player1 = null;
    [SerializeField] private Transform player2 = null;
    [SerializeField] private bool resizeCamera = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        transform.position = (player1.position + player2.position) / 2 + transform.forward * transform.position.z;
        if (resizeCamera)
        {
            if (Mathf.Abs(player1.position.y - player2.position.y) > Mathf.Abs(player1.position.x - player2.position.x))
            {
                cam.orthographicSize = Mathf.Abs((player1.position.y - player2.position.y)) * ps + cs;
            }
            else
            {
                cam.orthographicSize = Mathf.Abs((player1.position.x - player2.position.x)) * ps + cs;
            }
        }
    }
}
