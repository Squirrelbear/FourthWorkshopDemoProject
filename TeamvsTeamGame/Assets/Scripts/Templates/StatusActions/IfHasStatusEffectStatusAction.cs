using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IfHasStatusEffectStatusAction", menuName = "Team Objects/StatusAction/New IfHasStatusEffect StatusAction", order = 5)]
public class IfHasStatusEffectStatusAction : StatusAction
{
    public enum AttackerOrDefender { Attacker, Defender }
    public enum BuffDebuffOrEither { AnyBuff, AnyDebuff, Any }
    public AttackerOrDefender thisUnit;
    public BuffDebuffOrEither hasEffectType;
    public bool onlyIfHasSpecificName;
    public string specificEffectName;
    public StatusAction executeIfTrue;
    public StatusAction executeIfFalse;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        if (isConditionTrue(attacker, defender))
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
        if (isConditionTrue(attacker, defender))
        {
            executeIfTrue?.undoAction(attacker, defender);
        }
        else
        {
            executeIfFalse?.undoAction(attacker, defender);
        }
    }


    private bool isConditionTrue(GameObject attacker, GameObject defender)
    {
        GameObject checkUnit = thisUnit == AttackerOrDefender.Attacker ? attacker : defender;
        CharacterSheetBehaviour charSheet = checkUnit.GetComponent<CharacterSheetBehaviour>();
        if (onlyIfHasSpecificName)
        {
            foreach (StatusEffect effect in charSheet.statusEffects)
            {
                if (effect.statusEffectTemplate.statusName.Equals(specificEffectName))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            foreach (StatusEffect effect in charSheet.statusEffects)
            {
                if (hasEffectType != BuffDebuffOrEither.AnyDebuff && effect.statusEffectTemplate.isBuff)
                {
                    return true;
                }
                else if(hasEffectType != BuffDebuffOrEither.AnyBuff && !effect.statusEffectTemplate.isBuff)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
