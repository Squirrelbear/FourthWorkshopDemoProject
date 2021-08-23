using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleObserver : MonoBehaviour
{
    public bool toUpperCase;

    private void OnEnable() => ExampleObserverTrigger.OnNewMessageEvent += printMessage;
    private void OnDisable() => ExampleObserverTrigger.OnNewMessageEvent -= printMessage;

    private void printMessage(string value)
    {
        if(toUpperCase)
        {
            Debug.Log(value.ToUpper());
        }
        else
        {
            Debug.Log(value);
        }
    }
}
