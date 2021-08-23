using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SampleAbstractCommand
{
    public abstract void executeCommand();
    public abstract void undoCommand();
}
