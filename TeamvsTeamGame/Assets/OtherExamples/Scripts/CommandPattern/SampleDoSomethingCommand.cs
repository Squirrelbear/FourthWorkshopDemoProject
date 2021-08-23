using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDoSomethingCommand : SampleAbstractCommand
{
    private int variable;

    public SampleDoSomethingCommand(int variable)
    {
        this.variable = variable;
    }

    public override void executeCommand()
    {
        Debug.Log("DoSomethingCommand: " + variable);
    }

    public override void undoCommand()
    {
        // Could reverse based on stored variables
    }
}
