using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealTargetStatusAction", menuName = "Team Objects/StatusAction/New HealTarget StatusAction", order = 7)]
public class HealTargetStatusAction : StatusAction
{
    public enum TargetType { Self, Assist, Defender }
    public TargetType targetType;
    public bool asPercentOfMaxHP = true;
    public float healthValue;
    public float shieldValue;
    public float protectionValue;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        UnitHealthBehaviour target;
        if(targetType == TargetType.Self)
        {
            target = attacker.GetComponent<UnitHealthBehaviour>();
        }
        else if(targetType == TargetType.Assist)
        {
            target = CombatManager.instance.getAssist().GetComponent<UnitHealthBehaviour>();
        }
        else
        {
            target = defender.GetComponent<UnitHealthBehaviour>();
        }

        if (asPercentOfMaxHP)
        {
            target.increaseHealthByPercentOfMax(healthValue, shieldValue, protectionValue);
        }
        else
        {
            target.increaseHealth((int)healthValue, (int)shieldValue, (int)protectionValue);
        }
    }

    public override void undoAction(GameObject attacker, GameObject defender)
    {
        
    }
}
