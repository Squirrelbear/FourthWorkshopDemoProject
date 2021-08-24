using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyStatusEffectStatusAction", menuName = "Team Objects/StatusAction/New ApplyStatusEffect StatusAction", order = 6)]
public class ApplyStatusEffectStatusAction : StatusAction
{
    public enum StatusEffectTarget { Self, Defender, Assist }

    public StatusEffectTarget applyToTarget;
    public StatusEffectTemplate statusEffectToApply;
    public float chanceToApply;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        if(chanceToApply < Random.value)
        {
            return;
        }

        StatusEffect effectToApply = new StatusEffect(statusEffectToApply, attacker, defender);
        if(applyToTarget == StatusEffectTarget.Self)
        {
            attacker.GetComponent<CharacterSheetBehaviour>().addStatusEffect(effectToApply);
        }
        else if (applyToTarget == StatusEffectTarget.Defender)
        {
            defender.GetComponent<CharacterSheetBehaviour>().addStatusEffect(effectToApply);
        }
        else
        {
            CombatManager.instance.getAssist().addStatusEffect(effectToApply);
        }
    }

    public override void undoAction(GameObject attacker, GameObject defender)
    {
        
    }
}
