using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMinMax : MonoBehaviour {

    [SerializeField] private float coef = 1;
    [SerializeField] private Text text = null;
    [SerializeField] private Text min = null;
    [SerializeField] private Text max = null;
    [SerializeField] private Slider slider = null;
    private string word;

    // Use this for initialization
    void Start () {
        word = text.text;
        min.text = (slider.minValue * coef).ToString();
        max.text = (slider.maxValue * coef).ToString();
    }
	
	// Update is called once per frame
	void Update () {
        text.text = word + " : " + slider.value.ToString();
    }
}
