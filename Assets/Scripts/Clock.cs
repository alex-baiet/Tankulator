using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NewScript

public class Clock : MonoBehaviour
{

    public static float[] timeFlowPlayer = new float[2] { 1, 1 };
    public static float[] timeFlowBullet = new float[2] { 1, 1 };
    public static bool isPaused = false;

    private static float[] timeFlowPlayerBefore;
    private static float[] timeFlowBulletBefore;

    public static float GetTimeFlow(string timeName)
    {
        switch (timeName)
        {
            case "P0":return timeFlowPlayer[0];
            case "P1":return timeFlowPlayer[1];
            case "B0":return timeFlowBullet[0];
            case "B1":return timeFlowBullet[1];
            default:return 1f;
        }
    }

    public static void PauseTime()
    {
        if (!isPaused)
        {
            isPaused = true;
            timeFlowPlayerBefore = timeFlowPlayer;
            timeFlowBulletBefore = timeFlowBullet;

            timeFlowPlayer = new float[2] { 0, 0 };
            timeFlowBullet = new float[2] { 0, 0 };
        }
    }

    public static void UnpauseTime()
    {
        if (isPaused)
        {
            isPaused = false;
            timeFlowPlayer = timeFlowPlayerBefore;
            timeFlowBullet = timeFlowBulletBefore;
        }
    }

    public void PauseTimeLoaded() { PauseTime(); }
    public void UnpauseTimeLoaded() { UnpauseTime(); }
}
