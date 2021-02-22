using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//NewScript

/// <summary>
/// Class utlisée pour initialiser les booutons de choix de personnage.
/// </summary>
public class LoadPlayerButtonChoice : MonoBehaviour
{

    private string namep;
    private StatPlayer statPlayer;
    [HideInInspector] public Stat stat;
    [HideInInspector] public MenuPlayerChoice menuPChoice = null;
    [HideInInspector] public int playerId = 0;
    [HideInInspector] public Text infoText = null;
    [HideInInspector] public RawImage infoImage = null;
    [HideInInspector] public string Pname = null;

    void Start()
    {
        LoadPlayerButton();
        ChangeUIText();
    }

    /// <summary>
    /// Récupère le StatPlayer pour le bouton.
    /// </summary>
    private void LoadPlayerButton()
    {
        namep = name.Replace("But", "");
        statPlayer = stat.player;

        gameObject.GetComponent<Button>().onClick.AddListener(ChangeUIInfoText);

    }

    /// <summary>
    /// Remplace le texte du bouton par le nom du personnage.
    /// </summary>
    private void ChangeUIText()
    {
        //string playerName = transform.parent.parent.gameObject.GetComponent<StatAll>().players[nbr].name;
        transform.GetChild(0).gameObject.GetComponent<Text>().text = statPlayer.name;
    }

    public void ChangeUIInfoText()
    {
        infoText.alignment = TextAnchor.UpperLeft;
        infoText.fontSize = 12;
        infoText.text = "Nom : " + statPlayer.name + 
            "\n\nVie: " + statPlayer.life.ToString() + 
            "\nVitesse : " + statPlayer.speed.ToString() +
            "\n\nArmes :\n -" + statPlayer.weaponsName[0] +
            "\n -" + statPlayer.weaponsName[1] +
            "\n\nSpecial:\n -" + statPlayer.specialName;

        Texture playerTexture = stat.player.texture;
        RectTransform rectTransform = infoImage.rectTransform;
        rectTransform.sizeDelta = new Vector2(playerTexture.width * 2, playerTexture.height * 2);

        //infoImage.gameObject.rectTransform;
        infoImage.texture = playerTexture;

        
        PlayerPrefs.SetString(Pname, statPlayer.name);
        Debug.Log("Personnage " + statPlayer.name + " choisi pour le joueur " + Pname);
        menuPChoice.PConfirmed[playerId] = true;
    }
}
