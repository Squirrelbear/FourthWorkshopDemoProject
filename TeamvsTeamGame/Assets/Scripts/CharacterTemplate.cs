using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterTemplate", menuName = "Team Objects/New CharacterTemplate", order = 1)]
public class CharacterTemplate : ScriptableObject
{
    public enum CharacterTag { LightSide, Support, Leader, Rebel, Phoenix, Droid, Tank, Jedi, Attacker }

    [SerializeField] public string characterName;
    [SerializeField] public CharacterTag[] characterTags;

    [Header("Base Attributes")]
    [SerializeField] [Tooltip("Strength: Increases Health and Armour.")]
    public int strength;

    [SerializeField] [Tooltip("Agility: Increases Physical Critical Rating and Armour.")]
    public int agility;

    [SerializeField] [Tooltip("Tactics: Increases Physical Damage, Special Damage and Resistance.")]
    public int tactics;

    [SerializeField]
    [Tooltip("Mastery: Increases Accuracy, Critcal Avoidance, and Damage.")]
    public int mastery = 0;

    [Header("General")]
    [SerializeField]
    [Tooltip("Health: The core survivability of this unit. If this reaches 0 then the unit is defeated.")]
    public int health = 1229;
    [SerializeField]
    [Tooltip("Shield: Additional Survivability. If this reaches 0, then the unit will begin losing Health.")]
    public int shield = 2212;
    [SerializeField]
    [Tooltip("Speed: Determines the rate in which this unit gets to take a turn.")]
    public int speed = 13;
    [SerializeField]
    [Tooltip("Critical Damage: Increases the amount of damage dealt by a Critical Hit.")]
    public float critDamageMultiplier = 1.5f;
    [SerializeField]
    [Tooltip("Potency increases the chance to apply detrimental effects to opponents.")]
    public float potency = 0.0331f;
    [SerializeField]
    [Tooltip("Tenacity: Increases the chance to ward off detrimental effects.")]
    public float tenacity = 0.03f;
    [SerializeField]
    [Tooltip("Health Steal: Determines the amount of Health restored based upon damage dealt.")]
    public float healthSteal = 0;
    [SerializeField]
    [Tooltip("Defence Penetration: Ignores a percentage of enemy's Defence.")]
    public float defencePenetration = 0;

    [Header("Physical Offense")]
    [SerializeField]
    [Tooltip("Used to calculate damage dealt by all Physical abilities. Reduced by the opponent(s) Armour.")]
    public int damagePhysical = 120;
    [SerializeField]
    [Tooltip("Critical Chance: Determines the chance to deliver a Critical Hit for Physical abilties.")]
    public float criticalChancePhysical = 0.029f;
    [SerializeField]
    [Tooltip("Armour Penetration: Ignores an equal amount of the oppoenmt(s) Armour.")]
    public int armourPenetration = 10;
    [SerializeField]
    [Tooltip("Accuracy: Ignores an equal amount of an opponent(s) Dodge.")]
    public float accuracyPhysical;

    [Header("Physical Survivability")]
    [SerializeField]
    [Tooltip("Armour: Reduces the damage taken from abilities using Physical Damage.")]
    public float armour = 0.015f;
    [SerializeField]
    [Tooltip("Dodge Chance: Determines the chance to evade Physical abilities.")]
    public float dodgeChance = 0.02f;
    [SerializeField]
    [Tooltip("Critical Avoidance: Reduces the chance of receiving a Critical Hit from Physical damaging sources.")]
    public float criticalAvoidancePhysical = 0;

    [Header("Special Offense")]
    [SerializeField]
    [Tooltip("Damage: Used to calculate damage dealt by abilities that deal Special Damage. Reduced by opponent(s) Resistance.")]
    public int damageSpecial = 163;
    [SerializeField]
    [Tooltip("Critical Chance: Determines the chance to deliver a Critical Hit for abilities using Special Damage.")]
    public float criticalChance = 0.0294f;
    [SerializeField]
    [Tooltip("Resistance Penetration: Ignores an equal amount of the opponent(s) Resistance.")]
    public int resistancePenetration = 0;
    [SerializeField]
    [Tooltip("Accuracy: Ignores an equal amount of an opponent(s) Deflection.")]
    public float acurracySpecial = 0;

    [Header("Special Survivability")]
    [SerializeField]
    [Tooltip("Resistance: Reduces damage taken from abilities using Special Damage.")]
    public float resistance = 0.016f;
    [SerializeField]
    [Tooltip("Deflection Chance: Determines the chance to evade abilities that deal Special Damage.")]
    public float deflectionChance = 0.02f;
    [SerializeField]
    [Tooltip("Critical Avoidance: Reduces the chance of receiving a Critical Hit from Special damaging sources.")]
    public float criticalAvoidanceSpecial = 0;

}
