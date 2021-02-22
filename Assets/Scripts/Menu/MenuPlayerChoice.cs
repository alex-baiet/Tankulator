using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//NewScript

public class MenuPlayerChoice : MonoBehaviour
{
    [SerializeField] private Button buttonConfirm = null;
    [SerializeField] private GameObject butToAdd = null;
    [Header("player info")]
    [Tooltip("BLBLABLABLABLABLABLABLABABABLALBABALALBLABABLLBLBLBLBLBBLLBLBBLLBLLB")]
    [SerializeField] private GameObject P1Buttons = null;
    [SerializeField] private Text P1Info = null;
    [SerializeField] private RawImage P1Image = null;
    [Space]
    [SerializeField] private GameObject P2Buttons = null;
    [SerializeField] private Text P2Info = null;
    [SerializeField] private RawImage P2Image = null;
    [SerializeField] private ScrollbarMove P2ScrollBar = null;


    [HideInInspector] public bool[] PConfirmed = new bool[] { false, false };
    private GameObject[] PButtons;
    private Text[] PInfo;
    private RawImage[] PImage;

    private List<GameObject> buttonsObject = new List<GameObject>();
    private List<Button> buttons = new List<Button>();
    private int chosenBut = 0;
    private bool moved = false;

    void Start()
    {
        PButtons = new GameObject[] { P1Buttons, P2Buttons };
        PInfo = new Text[] { P1Info, P2Info };
        PImage = new RawImage[] { P1Image, P2Image };

        CreateAllButtons(0);
        CreateAllButtons(1);
    }


    void Update()
    {
        ChooseButton();
        SubmitButton();
        if (PConfirmed[0] && PConfirmed[1]) { buttonConfirm.interactable = true; }
    }


    /// <summary>
    /// Créer tout les boutons pour le choix du personnage du menu principal.
    /// </summary>
    private void CreateAllButtons(int playerId)
    {
        Vector3 pos = PButtons[playerId].transform.position;
        RectTransform rectTr = PButtons[playerId].GetComponent<RectTransform>();
        rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, 30f + StatAll.stats.Count * 35f);

        int i = 0;
        foreach (KeyValuePair<string, Stat> statP in StatAll.stats)
        {
            NewButton("But" + statP.Value.player.name, new Vector3(pos.x + 55f / 12f, pos.y + (-30f - i * 35f) / 12f, 0), playerId);
            i++;
        }

        if (playerId == 1)
        {
            buttons[0].colors = NewButtonColor(buttons[0].colors, buttons[0].colors.highlightedColor);
        }

    }

    /// <summary>
    /// Créer un nouveau bouton venant d'une prefab.
    /// </summary>
    private void NewButton(string name, Vector3 pos, int playerId)
    {
        GameObject newBut = Instantiate(butToAdd, pos, Quaternion.Euler(0f, 0f, 0f), PButtons[playerId].transform);

        LoadPlayerButtonChoice lpbc = newBut.GetComponent<LoadPlayerButtonChoice>();
        Button but = newBut.GetComponent<Button>();

        newBut.name = name;
        lpbc.stat = StatAll.stats[name.Replace("But", "")];
        lpbc.infoText = PInfo[playerId];
        lpbc.infoImage = PImage[playerId];
        lpbc.Pname = "P" + (playerId + 1);
        lpbc.menuPChoice = this;
        lpbc.playerId = playerId;
        but.interactable = playerId == 0;

        if (playerId == 1)
        {
            buttonsObject.Add(newBut);
            buttons.Add(but);
            but.colors = NewButtonColor(but.colors, but.colors.normalColor);
        }

    }


    private ColorBlock NewButtonColor(ColorBlock colorBlock, Color newColor)
    {
        colorBlock.disabledColor = newColor;
        return colorBlock;
    }

    private void ChooseButton()
    {
        float inputAxis = -InputPlayer.GetAxisVertical(false);
        if (inputAxis < -0.1f || inputAxis > 0.1f)
        {
            if (!moved)
            {
                buttons[chosenBut].colors = NewButtonColor(buttons[chosenBut].colors, buttons[chosenBut].colors.normalColor);

                chosenBut += inputAxis > 0.1f ? 1 : -1;
                Debug.Log("Nombre de joueurs chargés : " + buttons.Count);
                if (chosenBut == buttons.Count) { chosenBut = 0; }
                else if (chosenBut < 0) { chosenBut = buttons.Count - 1; }

                buttons[chosenBut].colors = NewButtonColor(buttons[chosenBut].colors, buttons[chosenBut].colors.selectedColor);

                float scrollMax = P1Buttons.GetComponent<RectTransform>().sizeDelta.y - 230f;
                float butPosInScroll = 30f + chosenBut * 35f - 115f;
                float coefScroll = 1 - butPosInScroll / scrollMax;
                /*Debug.Log("scrollMax = " + scrollMax);
                Debug.Log("butPosInScroll = " + butPosInScroll);
                Debug.Log("coefScroll = " + coefScroll);*/
                P2ScrollBar.SetScrollbarValue( 
                    coefScroll < 0 ? 0 :
                    coefScroll > 1 ? 1 :
                    coefScroll);

                moved = true;

            }
        }
        else
        {
            moved = false;
        }
    }

    private void SubmitButton()
    {
        if (InputPlayer.GetButtonConfirmDown(false))
        {
            Debug.Log("J2 Confirmé");
            buttonsObject[chosenBut].GetComponent<LoadPlayerButtonChoice>().ChangeUIInfoText();
            buttons[chosenBut].colors = NewButtonColor(buttons[chosenBut].colors, buttons[chosenBut].colors.pressedColor);
        }

        if (InputPlayer.GetButtonConfirmUp(false))
        {
            buttons[chosenBut].colors = NewButtonColor(buttons[chosenBut].colors, buttons[chosenBut].colors.selectedColor);

        }

        if (InputPlayer.GetButtonPauseDown() && PConfirmed[0] && PConfirmed[1])
        {
            buttonConfirm.onClick.Invoke();
        }
    }
}
