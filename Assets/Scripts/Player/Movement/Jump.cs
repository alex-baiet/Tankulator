using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{

    [SerializeField] private Transform raycastTransform = null;
    [SerializeField] private GameObject afterimage = null; //image rémanente
    public SpriteRenderer spriteParent = null;
    [HideInInspector] public Vector3 lastPosition; //position avant Jump
    [HideInInspector] public Vector3 newPosition; //position après Jump
    [HideInInspector] public float n; //ordre des images rémanentes
    [HideInInspector] public bool inJump = false;
    private Player player;
    private LoadStats load;

    public float timeBetweenJump = 0.8f; //délai entre chaque saut (seconde)
    public float afterimageLife = 0.5f; //temps de vie de l'image rémanente (seconde)
    public float afterimageAlpha = 0.5f; //transparence de départ de l'image rémanente (0 à 1)
    [HideInInspector] public bool jumpOnTime = false; //activer/désactiver le Jump sur un temps donné
    //Jump on time
    //public float jumpTime = 0.3f; //temps du saut (seconde)
    //public float jumpSpeed = 3; //vitesse durant un Jump (unité*300)
    //public float timeBetweenAfterimage = 0.05f; //Temps entre chaque images rémanentes (seconde)
    //Jump instantané
    public float jumpSize = 2; //longueur du saut (unité Unity)
    public float afterimageNumber = 7; //nombre d'image rémanente

    private float endJump;
    private Vector3 direction;
    private float lastTime;
    //private float timeNextAfterimage = 0;
    private Animator animator;
    private Rigidbody2D rb;
    private float diag;
    private float timeNextJump;

    private enum Stat
    {
        timeBetweenJump,
        jumpSize,
        afterimageLife,
        afterimageAlpha,
        afterimageNumber,
        //jumpOnTime,
        //jumpTime,
        //jumpSpeed,
        //timeBetweenAfterimage,
        size
    }

    void Start()
    {
        spriteParent = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        load = GetComponent<LoadStats>();
        LoadJump(load.jump);
    }

    void Update()
    {
        raycastTransform.rotation = Quaternion.Euler(0, 0, 0);
        if (player.createMode) { LoadJump(load.jump); }
        Jumping("Jump", "Horizontal", "Vertical");
    }

    private void Jumping(string input, string inputx, string inputy)
    {
        
        if (Input.GetButton(input) == true
        && timeNextJump < Time.time
        && (Input.GetAxisRaw(inputx) != 0
        || Input.GetAxisRaw(inputy) != 0))
        {

            lastPosition = transform.position;

            if (jumpOnTime)
            {
                /*timeNextJump = Time.time + timeBetweenJump + jumpTime;
                if (Mathf.Abs(Input.GetAxisRaw(inputx)) == 1 && Mathf.Abs(Input.GetAxisRaw(inputy)) == 1) { diag = 0.7071f; } else { diag = 1f; }
                //newPosition = new Vector2(transform.position.x + Input.GetAxisRaw(inputx) * diag * jumpSize, transform.position.y + Input.GetAxisRaw(inputy) * diag * jumpSize);
                //startJump = Time.time;
                endJump = Time.time + jumpTime;
                direction = new Vector3(Input.GetAxisRaw(inputx) * diag, Input.GetAxisRaw(inputy) * diag);
                inJump = true;
                n = 0;*/
            }

            else //teleport
            {
                Quaternion rot = transform.rotation;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                RaycastHit2D hit = Physics2D.Raycast(raycastTransform.position, transform.right * Input.GetAxisRaw(inputx) + transform.up * Input.GetAxisRaw(inputy), jumpSize, 1<<16 );
                transform.rotation = rot;

                timeNextJump = Time.time + timeBetweenJump;

                if (Mathf.Abs(Input.GetAxisRaw(inputx)) == 1 && Mathf.Abs(Input.GetAxisRaw(inputy)) == 1) { diag = 0.7071f; } else { diag = 1f; }
                if (hit.collider !=null)
                {
                    print(hit.collider.name);
                    transform.position = hit.point;
                }

                else
                {
                    transform.position = new Vector2(transform.position.x + Input.GetAxisRaw(inputx) * diag * jumpSize, transform.position.y + Input.GetAxisRaw(inputy) * diag * jumpSize);
                }
                newPosition = transform.position;
                for (n = afterimageNumber - 1; n >= 0; n -= 1)
                {
                    Instantiate(afterimage, transform);
                }
            }
        }
        /*if (inJump == true)
        {
            rb.velocity = direction * Time.deltaTime * jumpSpeed * 300;
            transform.position += direction * diag * Time.deltaTime * jumpSpeed;
            if (Time.time > timeNextAfterimage)
            {
                Instantiate(afterimage, transform);
                timeNextAfterimage = Time.time + timeBetweenAfterimage;
            }
            if (Time.time >= endJump) { inJump = false; }
        }*/
    }

    private void LoadJump(int actualJump)
    {
        timeBetweenJump = PlayerPrefs.GetFloat("Jump" + actualJump.ToString() + "Stat" + ((int)Stat.timeBetweenJump).ToString());
        jumpSize = PlayerPrefs.GetFloat("Jump" + actualJump.ToString() + "Stat" + ((int)Stat.jumpSize).ToString());
        afterimageLife = PlayerPrefs.GetFloat("Jump" + actualJump.ToString() + "Stat" + ((int)Stat.afterimageLife).ToString());
        afterimageAlpha = PlayerPrefs.GetFloat("Jump" + actualJump.ToString() + "Stat" + ((int)Stat.afterimageAlpha).ToString());
        afterimageNumber = (int)PlayerPrefs.GetFloat("Jump" + actualJump.ToString() + "Stat" + ((int)Stat.afterimageNumber).ToString());
    }
}
