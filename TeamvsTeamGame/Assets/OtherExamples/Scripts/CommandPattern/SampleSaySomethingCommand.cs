using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSaySomethingCommand : SampleAbstractCommand
{
    private Vector3 variable;

    public SampleSaySomethingCommand(Vector3 variable)
    {
        this.variable = variable;
    }

    public override void executeCommand()
    {
        Debug.Log("SaySomethingCommand: " + variable);
    }

    public override void undoCommand()
    {
        // Could reverse based on stored variables
    }
}

