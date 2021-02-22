using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//NewScript

public class LoadMapButtonChoice : MonoBehaviour
{

    private string mapName;
    [HideInInspector] public Button butStart = null;
    [HideInInspector] public MenuMapChoice menuMapChoice = null;

    void Start()
    {
        LoadMapButton();
        ChangeUIText();
    }

    private void LoadMapButton()
    {
        mapName = name.Replace("But", "");
        gameObject.GetComponent<Button>().onClick.AddListener(UIDisplayInfoMap);

    }

    /// <summary>
    /// Remplace le texte du bouton par le nom de la map.
    /// </summary>
    private void ChangeUIText()
    {
        transform.GetChild(0).gameObject.GetComponent<Text>().text = mapName;
    }

    public void UIDisplayInfoMap()
    {
        PlayerPrefs.SetString("map actual", mapName);
        Debug.Log("Map " + mapName + " choisi.");
        butStart.interactable = true;

        menuMapChoice.DisplayMap(mapName);
    }
}
