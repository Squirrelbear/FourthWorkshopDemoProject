using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCommandPlayer : MonoBehaviour
{
    public Queue<SampleAbstractCommand> commandQueue;

    // Start is called before the first frame update
    void Start()
    {
        commandQueue = new Queue<SampleAbstractCommand>();
        commandQueue.Enqueue(new SampleDoSomethingCommand(54));
        commandQueue.Enqueue(new SampleSaySomethingCommand(new Vector3(1, 2, 3)));
        commandQueue.Enqueue(new SampleDoSomethingCommand(87));
        commandQueue.Enqueue(new SampleSaySomethingCommand(new Vector3(4, 5, 3)));
        commandQueue.Enqueue(new SampleDoSomethingCommand(90));
        commandQueue.Enqueue(new SampleSaySomethingCommand(new Vector3(2, 5, 9)));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (commandQueue.Count > 0)
            {
                SampleAbstractCommand command = commandQueue.Dequeue();
                command.executeCommand();
            }
            else
            {
                Debug.Log("No more commands...");
            }
        }
    }
}
