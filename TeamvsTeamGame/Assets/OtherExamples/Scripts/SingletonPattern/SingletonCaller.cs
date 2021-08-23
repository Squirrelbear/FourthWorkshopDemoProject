using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCaller : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Singleton Example");
            SingletonExample.instance.doSomethingMethod();
        }
    }
}
