using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifyStatsStatusAction", menuName = "Team Objects/StatusAction/New ModifyStats StatusAction", order = 1)]
public class ModifyStatsStatusAction : StatusAction
{
    public enum PercentOrValue { ExactValue, PercentOfMaxHP, PercentOfStat}
    public enum AttackerOrDefender { Attacker, Defender }

    [SerializeField] public string statName;
    [SerializeField] public PercentOrValue applyValueAs = PercentOrValue.ExactValue;
    [SerializeField] public float value = 0;
    [SerializeField] public AttackerOrDefender ofAttackerOrDefender = AttackerOrDefender.Defender;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        float actualValue = getActualValue(attacker, defender);

        defender.GetComponent<CharacterSheetBehaviour>().modifyStatByValue(statName, actualValue);
    }

    public override void undoAction(GameObject attacker, GameObject defender)
    {
        float actualValue = getActualValue(attacker, defender);

        defender.GetComponent<CharacterSheetBehaviour>().modifyStatByValue(statName, -actualValue);
    }

    private float getActualValue(GameObject attacker, GameObject defender)
    {
        float actualValue = value;
        GameObject statsFromObj = (ofAttackerOrDefender == AttackerOrDefender.Defender) ? defender : attacker;
        if (applyValueAs == PercentOrValue.PercentOfMaxHP)
        {
            actualValue = statsFromObj.GetComponent<UnitHealthBehaviour>().healthMax * value;
        }
        else if (applyValueAs == PercentOrValue.PercentOfStat)
        {
            actualValue = statsFromObj.GetComponent<CharacterSheetBehaviour>().getValue(statName) * value;
        }
        return actualValue;
    }
}
