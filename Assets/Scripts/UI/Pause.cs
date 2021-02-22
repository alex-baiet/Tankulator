using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    [SerializeField] private GameObject menu = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (InputPlayer.GetButtonPauseDown())
        {
            if (!Clock.isPaused) { PauseGame(); }
            else { UnpauseGame(); }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Clock.PauseTime();
        menu.SetActive(true);
    }

    private void UnpauseGame()
    {
        Clock.UnpauseTime();
        menu.SetActive(false);
    }
}
