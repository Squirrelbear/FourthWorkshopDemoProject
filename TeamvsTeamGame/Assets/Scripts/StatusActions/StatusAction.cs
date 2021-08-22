using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusAction : ScriptableObject
{
    public enum EnemyOrAssistTarget { Enemy, Assist }
    public EnemyOrAssistTarget defenderAs;

    public abstract void performAction(GameObject attacker, GameObject defender);
    public abstract void undoAction(GameObject attacker, GameObject defender);
}
