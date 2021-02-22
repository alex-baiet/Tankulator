using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWeaponName : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject weaponText = null;
    
    private int lastWeaponId = 0;
    private List<GameObject> textsObject = new List<GameObject>();
    private List<Text> texts = new List<Text>();

    void Start()
    {
        int i = 0;
        foreach (string weaponName in player.stat.player.weaponsName)
        {
            GameObject newTextObject = Instantiate(weaponText, transform);
            Text newText = newTextObject.GetComponent<Text>();

            newTextObject.transform.localPosition = new Vector2(player.playerId == 0 ? 54f : -54f, (10 + i * 20));
            if (i != 0) { newText.color -= new Color(0, 0, 0, 0.5f); }
            newText.alignment = player.playerId == 0 ?
                TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
            newText.text = weaponName;
            
            textsObject.Add(newTextObject);
            texts.Add(newText);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (lastWeaponId != player.weaponId)
        {
            texts[lastWeaponId].color = new Color(1, 1, 1, 0.5f);
            texts[player.weaponId].color = new Color(1, 1, 1, 1);
            
            lastWeaponId = player.weaponId;
        }
    }

}
