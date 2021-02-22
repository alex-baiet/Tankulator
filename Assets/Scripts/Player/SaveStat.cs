using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//GARBAGE

public class SaveStat : MonoBehaviour
{

    private int _y;
    private int _x;
    private float value = 0;
    private string category;
    private string stringcat;



    public float StrToFlt(string number)
    {
        float result = 0;
        float coef = 0.1f;
        bool coma = false;
        foreach (char item in number)
        {
            if (item == '.' || coma == true)
            {
                coma = true;
                goto deci;
            }
            switch (item)
            {
                case '0': result = result * 10; break;
                case '1': result = result * 10 + 1; break;
                case '2': result = result * 10 + 2; break;
                case '3': result = result * 10 + 3; break;
                case '4': result = result * 10 + 4; break;
                case '5': result = result * 10 + 5; break;
                case '6': result = result * 10 + 6; break;
                case '7': result = result * 10 + 7; break;
                case '8': result = result * 10 + 8; break;
                case '9': result = result * 10 + 9; break;
                default: break;
            }
            continue;

        deci: switch (item)
            {
                case '0': coef = coef / 10; break;
                case '1': result = result + 1 * coef; goto case '0';
                case '2': result = result + 2 * coef; goto case '0';
                case '3': result = result + 3 * coef; goto case '0';
                case '4': result = result + 4 * coef; goto case '0';
                case '5': result = result + 5 * coef; goto case '0';
                case '6': result = result + 6 * coef; goto case '0';
                case '7': result = result + 7 * coef; goto case '0';
                case '8': result = result + 8 * coef; goto case '0';
                case '9': result = result + 9 * coef; goto case '0';
                default: break;
            }
        }
        return result;
    }

    public void SetCategory(string word)
    {
        category = word;
    }

    public void SetFightStat(string number)
    {
        PlayerPrefs.SetInt("Player" + _x.ToString() + category, (int)StrToFlt(number));
        print("Player" + _x.ToString() + category + " : " + ((int)StrToFlt(number)).ToString());
    }

    public void SaveAState(string number)
    {
        PlayerPrefs.SetFloat(category + _y.ToString() + "Stat" + _x.ToString(), StrToFlt(number));
        print(category + _y.ToString() + "Stat" + _x.ToString() + " : " + StrToFlt(number).ToString());
    }

    public void SaveString(string number)
    {
        print(value);
        PlayerPrefs.SetString(category + _y.ToString() + "Name", number);
        print(category + _y.ToString() + stringcat + number);
    }

    public void SetNumber(string yy)
    {
        _y = (int)StrToFlt(yy);
        print(_y.ToString());
    }

    public void SetStringCategory(string word)
    {
        stringcat = word;
    }

    /// <summary>
    /// uiiii
    /// </summary>
    /// <param name="xx"></param>
    public void StatNumber(int xx)
    {
        _x = xx;
        print(_x);
    }
}

