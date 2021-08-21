using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusAction : ScriptableObject
{
    public abstract void performAction(GameObject attacker, GameObject defender);
    public abstract void undoAction(GameObject attacker, GameObject defender);
}
