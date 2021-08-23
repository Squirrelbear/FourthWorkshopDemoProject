using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackTargetStatusAction", menuName = "Team Objects/StatusAction/New AttackTarget StatusAction", order = 3)]
public class AttackTargetStatusAction : StatusAction
{
    public enum TargetType { AllEnemies, OnlyCurrentTarget }
    public enum AttackVisualType { Stationary, Projectile, MoveToTarget }
    public TargetType targetType;
    public AttackVisualType attackVisualType;
    public float multiStrikeChance;
    public bool canBeEvaded = true;
    public bool canBeDodged = true;
    public bool isSpecial = false;
    public float timeToReachTarget = 1;
    public float timeToReturn = 0.5f;

    public StatusEffectTemplate[] statusEffectsToApply;
    public float[] chanceToApplyStatusEffects;

    public override void performAction(GameObject attacker, GameObject defender)
    {
        CombatManager.instance.queueAttack(new QueuedAttack(this, attacker, defender));
    }

    public override void undoAction(GameObject attacker, GameObject defender)
    {
        
    }
}
