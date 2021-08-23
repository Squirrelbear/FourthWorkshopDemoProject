using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleButtonBehaviour : MonoBehaviour
{
    public TMPro.TextMeshProUGUI buttonLabel;
    private int currentTimeScale = 1;

    public void nextTimeScale()
    {
        currentTimeScale *= 2;
        if(currentTimeScale > 4)
        {
            currentTimeScale = 1;
        }
        Time.timeScale = currentTimeScale;
        buttonLabel.text = currentTimeScale + "x";
    }
}
