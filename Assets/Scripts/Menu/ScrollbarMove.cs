using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarMove : MonoBehaviour
{
    private float valueLast = 1f;
    private float valueNew = 1f;
    [SerializeField] private float moveLength = 0.5f;
    [SerializeField] private float MoveSlowDownCoef = 2f;
    private float coefTime = 0f;
    private Scrollbar scroll;

    void Start()
    {
        scroll = GetComponent<Scrollbar>();
    }

    void Update()
    {
        MoveScrollbar();
    }

    private void MoveScrollbar()
    {
        coefTime += Time.deltaTime;

        if (coefTime < moveLength)
        {
            scroll.value = valueLast + (valueNew - valueLast) * Mathf.Pow(coefTime / moveLength, 1f / MoveSlowDownCoef);

        }
        else
        {
            scroll.value = valueNew;
        }
    }

    public void SetScrollbarValue(float value)
    {
        valueLast = scroll.value;
        valueNew = value;
        coefTime = 0f;
    }
}
