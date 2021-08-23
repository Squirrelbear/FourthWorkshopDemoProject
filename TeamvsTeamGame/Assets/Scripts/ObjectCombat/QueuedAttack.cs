using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedAttack
{
    public GameObject target;
    public GameObject attacker;
    public AttackTargetStatusAction statusAction;
    public bool isDone;
    public bool isStarted;
    public bool hasMultistriked;

    public QueuedAttack(AttackTargetStatusAction statusAction, GameObject attacker, GameObject target)
    {
        this.target = target;
        this.attacker = attacker;
        this.statusAction = statusAction;
        isDone = false;
        hasMultistriked = false;
        isStarted = false;
    }

    public void beginAttack()
    {
        isStarted = true;
        if(statusAction.attackVisualType == AttackTargetStatusAction.AttackVisualType.MoveToTarget)
        {
            beginMoveToAttack();
        }
        else if(statusAction.attackVisualType == AttackTargetStatusAction.AttackVisualType.Projectile)
        {
            beginRangedAttack();
        }
        else
        {
            // TODO
            isDone = true;
        }
    }

    private void processDamageStep()
    {
        CharacterSheetBehaviour attackerCharSheet = attacker.GetComponent<CharacterSheetBehaviour>();
        CharacterSheetBehaviour targetCharSheet = target.GetComponent<CharacterSheetBehaviour>();

        if (isDodging(attackerCharSheet, targetCharSheet))
        {
            Debug.Log("Attack on " + targetCharSheet.characterName + " was "
                + (statusAction.isSpecial ? "deflected" : "dodged") + " by " 
                + attackerCharSheet.characterName);
            return;
        }

        bool isCrit = isCriticalStrike(attackerCharSheet, targetCharSheet);
        float baseDamage = attackerCharSheet.getValue(statusAction.isSpecial ? "damageSpecial" : "damagePhysical");
        float armourModifier = getArmourModifier(attackerCharSheet, targetCharSheet);
        float critMultiplier = attackerCharSheet.getValue("critDamageMultiplier");

        float actualDamage = calculateActualDamage(isCrit, baseDamage, armourModifier, critMultiplier);

        float healthSteal = attackerCharSheet.getValue("healthSteal") * actualDamage;
        attacker.GetComponent<UnitHealthBehaviour>().increaseHealth((int)healthSteal,0,0);
        target.GetComponent<UnitHealthBehaviour>().damageByValue((int)actualDamage, false);
    }

    private float calculateActualDamage(bool isCrit, float baseDamage, float armourModifier, float critMultiplier)
    {
        float resultDamage = Random.Range(baseDamage * 0.9f, baseDamage * 1.1f);
        string logMessage = "Base: " + resultDamage;

        if (isCrit)
        {
            resultDamage *= critMultiplier;
            logMessage += " With Crit: " + resultDamage;
        }
        resultDamage *= (100 - armourModifier)/100f;
        logMessage += " With Armour: " + resultDamage;
        Debug.Log(logMessage);
        return resultDamage;
    }

    private bool isDodging(CharacterSheetBehaviour attacker, CharacterSheetBehaviour target)
    {
        if (!statusAction.canBeDodged)
        {
            return false;
        }

        float dodgeChance = target.getValue(statusAction.isSpecial ? "deflectionChance" : "dodgeChance");
        float accuracyChance = target.getValue(statusAction.isSpecial ? "accuracySpecial" : "accuracyPhysical");

        return dodgeChance - accuracyChance > Random.value;
    }

    private bool isCriticalStrike(CharacterSheetBehaviour attacker, CharacterSheetBehaviour target)
    {
        float critAvoidance = target.getValue(statusAction.isSpecial ? "criticalAvoidanceSpecial" : "criticalAvoidancePhysical");
        float critChance = attacker.getValue(statusAction.isSpecial ? "criticalChance" : "criticalChancePhysical");

        return critChance - critAvoidance > Random.value;
    }

    private float getArmourModifier(CharacterSheetBehaviour attacker, CharacterSheetBehaviour target)
    {
        float armour = target.getValue(statusAction.isSpecial ? "resistance" : "armourPenetration");
        float penetration = attacker.getValue(statusAction.isSpecial ? "resistancePenetration" : "armour");
        float defencePenetration = attacker.getValue("defencePenetration");

        return Mathf.Max(armour * (1 - penetration) * (1 - defencePenetration), 0);
    }

    private void beginMoveToAttack()
    {
        UnitAttackBehaviour unitAttackBehaviour = attacker.GetComponent<UnitAttackBehaviour>();
        unitAttackBehaviour.OnAttackTargetReached += handleAttackTargetReached;
        unitAttackBehaviour.executeAttackMove(target, statusAction.timeToReachTarget, statusAction.timeToReturn);
    }

    private void handleAttackTargetReached(UnitAttackBehaviour unitAttackBehaviour)
    {
        unitAttackBehaviour.OnAttackTargetReached -= handleAttackTargetReached;
        unitAttackBehaviour.OnFinishedReturn += handleFinishedAttack;
        processDamageStep();
    }

    private void handleFinishedAttack(UnitAttackBehaviour unitAttackBehaviour)
    {
        unitAttackBehaviour.OnFinishedReturn -= handleFinishedAttack;
        float randValue = Random.value;
        if (!hasMultistriked && statusAction.multiStrikeChance > randValue)
        {
            Debug.Log("Multistrike with " + statusAction.multiStrikeChance + " chance and random: " + randValue);
            hasMultistriked = true;
            beginAttack();
        }
        else
        {
            isDone = true;
        }
    }

    private void beginRangedAttack()
    {
        UnitRangedAttackBehaviour unitRangedAttackBehaviour = attacker.GetComponent<UnitRangedAttackBehaviour>();
        unitRangedAttackBehaviour.OnAttackTargetReached += handleAttackTargetReached;
        unitRangedAttackBehaviour.OnFinishedAllProjectiles += handleFinishedAttack;
        unitRangedAttackBehaviour.fireProjectiles(target, statusAction.timeToReachTarget);
    }

    private void handleAttackTargetReached(UnitRangedAttackBehaviour unitRangedAttackBehaviour)
    {
        unitRangedAttackBehaviour.OnAttackTargetReached -= handleAttackTargetReached;
        processDamageStep();
    }

    private void handleFinishedAttack(UnitRangedAttackBehaviour unitRangedAttackBehaviour)
    {
        unitRangedAttackBehaviour.OnFinishedAllProjectiles -= handleFinishedAttack;
        float randValue = Random.value;
        if (!hasMultistriked && statusAction.multiStrikeChance > randValue)
        {
            Debug.Log("Multistrike with " + statusAction.multiStrikeChance + " chance and random: " + randValue);
            hasMultistriked = true;
            beginRangedAttack();
        }
        else
        {
            isDone = true;
        }
    }
}
