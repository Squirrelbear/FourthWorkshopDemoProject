using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineExample : MonoBehaviour
{
    public enum ExampleState { WaitForPressA, WaitForPressB, WaitForPressC, Done }

    public ExampleState currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = ExampleState.WaitForPressA;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.C))
        {
            moveToNextStateCheck();
        }
    }

    private void moveToNextStateCheck()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(currentState == ExampleState.WaitForPressA)
            {
                currentState = ExampleState.WaitForPressB;
                Debug.Log("State is now waiting for B.");
            }
            else
            {
                Debug.Log("You pressed A, but this is not currently the key to press.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (currentState == ExampleState.WaitForPressB)
            {
                currentState = ExampleState.WaitForPressC;
                Debug.Log("State is now waiting for C.");
            }
            else
            {
                Debug.Log("You pressed B, but this is not currently the key to press.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentState == ExampleState.WaitForPressC)
            {
                currentState = ExampleState.Done;
                Debug.Log("State machine has finished");
            }
            else
            {
                Debug.Log("You pressed C, but this is not currently the key to press.");
            }
        }
    }
}
