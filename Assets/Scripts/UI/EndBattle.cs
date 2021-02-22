using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBattle : MonoBehaviour
{
    private Player[] players = new Player[2];
    private bool battleIsFinished = false;
    [SerializeField] private Transform maskTransform = null;
    private Transform menuTransform;
    [SerializeField] private Camera cam = null;
    private float timer = 0;
    [SerializeField] private float lengthTransition = 1;
    private bool transitionIsFinihed = false;
    [SerializeField] private Button buttonMenu = null;
    [SerializeField] private Text textWinner = null;




    void Start()
    {
        menuTransform = maskTransform.GetChild(0);
        maskTransform.gameObject.SetActive(false);

        for (int i = 0; i < 2; i++)
        {
            players[i] = GameObject.Find("Player" + (i + 1)).GetComponent<Player>();
        }
    }


    void Update()
    {
        if (!battleIsFinished)
        {
            foreach (Player player in players)
            {
                if (player.life <= 0)
                {
                    EndGame();
                }
            }
        }

        Transition();
    }

    private void EndGame()
    {
        textWinner.text = "Victoire pour\n";
        textWinner.text += players[0].life <= 0 ? players[1].stat.player.name + "\n(Joueur 2)" : players[0].stat.player.name + "\n(Joueur 1)";
        battleIsFinished = true;
        maskTransform.gameObject.SetActive(true);
        Clock.PauseTime();
        cam.gameObject.GetComponent<Pause>().enabled = false;
        buttonMenu.interactable = false;
    }

    private void Transition()
    {
        if (battleIsFinished && !transitionIsFinihed)
        {
            timer += Time.deltaTime;
            if (timer < lengthTransition)
            {
                float coefPos = cam.orthographicSize / 15 / 12;
                maskTransform.position = new Vector3(0f, (360f * Mathf.Pow((lengthTransition - timer) / lengthTransition, 2)) * coefPos) + transform.position;
                menuTransform.position = Vector3.zero + transform.position;
                foreach (Image img in GetComponentsInChildren<Image>())
                {
                    float coefBackground = img.name == "Background" ? 0.5f : 1f;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Sqrt(timer / lengthTransition) * coefBackground);
                }
            }
            else
            {
                maskTransform.position = Vector3.zero + transform.position;
                menuTransform.position = Vector3.zero + transform.position;
                foreach (Image img in GetComponentsInChildren<Image>())
                {
                    float coefBackground = img.name == "Background" ? 0.5f : 1f;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, coefBackground);
                }
                transitionIsFinihed = true;
            }

        }
    }
}
