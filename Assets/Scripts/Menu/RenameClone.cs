using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameClone : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        string name = gameObject.name;
        int newLenght = name.Length-7;
        string newName = "";

        int i = 1;
        foreach(char chr in name)
        {
            if (i <= newLenght)
            {
                newName += chr;
            }
            i++;
        }
        gameObject.name = newName;
    }

}
