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
        else
        {
            // TODO
            isDone = true;
        }
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
    }

    private void handleFinishedAttack(UnitAttackBehaviour unitAttackBehaviour)
    {
        unitAttackBehaviour.OnFinishedReturn -= handleFinishedAttack;
        if(!hasMultistriked && statusAction.multiStrikeChance > Random.value)
        {
            hasMultistriked = true;
            beginAttack();
        }
        else
        {
            isDone = true;
        }
    }
}
