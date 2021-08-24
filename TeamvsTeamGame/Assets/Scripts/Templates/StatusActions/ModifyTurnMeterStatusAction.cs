using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifyTurnMeterStatusAction", menuName = "Team Objects/StatusAction/New ModifyTurnMeter StatusAction", order = 8)]
public class ModifyTurnMeterStatusAction : StatusAction
{
    public enum TargetType { Self, Assist, Defender }
    public TargetType targetType;
    public float percentChange;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        CharacterSheetBehaviour target;
        if (targetType == TargetType.Self)
        {
            target = attacker.GetComponent<CharacterSheetBehaviour>();
        }
        else if (targetType == TargetType.Assist)
        {
            target = CombatManager.instance.getAssist();
        }
        else
        {
            target = defender.GetComponent<CharacterSheetBehaviour>();
        }

        float current = target.getValue("turn");
        if(percentChange > 0)
        {
            // TODO not used to cap the gain
            float changeBy = percentChange;
            if(percentChange + current > 1)
            {
                changeBy = 1 - current;
            }
            target.modifyStatByValue("turn",percentChange);
        }
        else if (percentChange < 0)
        {
            float postChange = current + percentChange;
            target.modifyStatByValue("turn", postChange < 0 ? current : percentChange);
        }
    }

    public override void undoAction(GameObject attacker, GameObject defender)
    {
    }
}
