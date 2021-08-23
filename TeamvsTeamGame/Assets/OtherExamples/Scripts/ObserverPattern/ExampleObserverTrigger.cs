using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleObserverTrigger : MonoBehaviour
{
    public static event Action<string> OnNewMessageEvent;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Firing Observer Trigger");
            OnNewMessageEvent?.Invoke("Hello");
        }
    }
}
