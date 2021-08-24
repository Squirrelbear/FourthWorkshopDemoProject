using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IfHasTagStatusAction", menuName = "Team Objects/StatusAction/New IfHasTag StatusAction", order = 2)]
public class IfHasTagStatusAction : StatusAction
{
    public enum AnyOrAll { Any, All }

    public AnyOrAll anyOrAll;
    public CharacterTemplate.CharacterTag[] tags;
    public StatusAction executeIfTrue;
    public StatusAction executeIfFalse;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        if(defenderAs == EnemyOrAssistTarget.Assist)
        {
            defender = CombatManager.instance.getAssist().gameObject;
        }

        if(isConditionTrue(defender))
        {
            executeIfTrue?.performAction(attacker, defender);
        }
        else
        {
            executeIfFalse?.performAction(attacker, defender);
        }
    }

    public override void undoAction(GameObject attacker, GameObject defender)
    {
        if (isConditionTrue(defender))
        {
            executeIfTrue?.undoAction(attacker, defender);
        }
        else
        {
            executeIfFalse?.undoAction(attacker, defender);
        }
    }

    private bool isConditionTrue(GameObject defender)
    {
        CharacterSheetBehaviour charSheet = defender.GetComponent<CharacterSheetBehaviour>();
        foreach(var tag in tags)
        {
            if(charSheet.hasTag(tag))
            {
                if(anyOrAll == AnyOrAll.Any)
                {
                    return true;
                }
            }
            else if(anyOrAll == AnyOrAll.All)
            {
                return false;
            }
        }
        return anyOrAll == AnyOrAll.All;
    }
}
