using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAbilityTemplate", menuName = "Team Objects/New UnityAbilityTemplate", order = 3)]
public class UnitAbilityTemplate : ScriptableObject
{
    public enum TargetType { NoTarget, Friend, EnemyAndFriend, Enemy }

    public string abilityName;
    public TargetType requireChooseTarget;
    public bool isPassive;
    public bool hasCooldown;
    public int cooldown;

    public List<StatusAction> actionSequence;
}
