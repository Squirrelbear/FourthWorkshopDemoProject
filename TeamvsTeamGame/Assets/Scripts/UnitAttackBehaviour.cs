using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackBehaviour : MonoBehaviour
{
    public event Action<UnitAttackBehaviour> OnAttackTargetReached;
    public event Action<UnitAttackBehaviour> OnFinishedReturn;

    public enum AttackState { WaitingCommand, MovingToTarget, ReturnToOrigin }
    [SerializeField]
    private Vector3 origin;
    [SerializeField]
    private AttackState attackState = AttackState.WaitingCommand;

    [SerializeField]
    private AnimationClip attackAnimation;
    [SerializeField]
    private AnimationClip walkAnimation;

    [SerializeField]
    public GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        attackState = AttackState.WaitingCommand;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isExecutingAttack()
    {
        return attackState != AttackState.WaitingCommand;
    }

    public void executeAttackMove(GameObject target, float timeToAttack, float timeToReturn)
    {
        if(attackState != AttackState.WaitingCommand)
        {
            Debug.LogWarning("Attempted to execute an attack while already attacking.");
            return;
        }
        StartCoroutine(executeAttackSequence(target, timeToAttack, timeToReturn));
    }

    private IEnumerator executeAttackSequence(GameObject target, float timeToAttack, float timeToReturn)
    {
        currentTarget = target;
        Animation anim = gameObject.GetComponentInChildren<Animation>();
        attackState = AttackState.MovingToTarget;
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
        if (anim != null && walkAnimation != null)
        {
            anim.Play(walkAnimation.name);
        }
        StartCoroutine(moveToPosition(transform, target.transform.position, timeToAttack, 0.75f));
        yield return new WaitForSeconds(timeToAttack);
        if (anim != null && attackAnimation != null)
        {
            anim.Play(attackAnimation.name);
        }
        OnAttackTargetReached?.Invoke(this);
        yield return new WaitForSeconds(0.4f);
        attackState = AttackState.ReturnToOrigin;
        StartCoroutine(moveToPosition(transform, origin, timeToReturn, 1));
        yield return new WaitForSeconds(timeToReturn);
        attackState = AttackState.WaitingCommand;
        OnFinishedReturn?.Invoke(this);
        yield return null;
    }

    // Based on code from: https://answers.unity.com/questions/296347/move-transform-to-target-in-x-seconds.html
    private IEnumerator moveToPosition(Transform transform, Vector3 position, float timeToMove, float percentToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < percentToMove)
        {
            t += Time.deltaTime / timeToMove * percentToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
