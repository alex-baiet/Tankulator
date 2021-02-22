using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public Vector3 position;
    public GameObject test;
    public Texture texture;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            
        }
    }


    void Update()
    {

        transform.position = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal"), 0, 0);
        for (int i = 0; i < 1600; i++)
        {
            
            Graphics.DrawMesh(mesh,new Vector3(Random.Range(5, 5), Random.Range(5, 5),0),transform.rotation,material, 0);
            //Graphics.DrawTexture(new Rect(Random.Range(-5, 5), Random.Range(-5, 5), 100,100), texture);
        }
        foreach (BoxCollider2D item in GetComponents<BoxCollider2D>())
        {
            Destroy(item,2);
            print("yeah");
        }
    }
    private void OnGUI()
    {
        Graphics.DrawTexture(new Rect(100, 100, 100, 100), texture);
        
    }
}
