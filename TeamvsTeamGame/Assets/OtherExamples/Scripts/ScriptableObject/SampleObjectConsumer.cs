using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleObjectConsumer : MonoBehaviour
{
    [SerializeField] private SampleScriptableObject objectData;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("ScriptableObject Consumer Example");
            Debug.Log(objectData.someStringData + " " + objectData.someIntValue);
        }
    }
}
