using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public event Action<ProjectileBehaviour> OnAttackTargetReached;

    [SerializeField]
    private Vector3 origin;

    [SerializeField]
    public GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void beginAttack(GameObject target, float timeToAttack)
    {
        StartCoroutine(attackTarget(target, timeToAttack));
    }

    private IEnumerator attackTarget(GameObject target, float timeToAttack)
    {
        transform.position = origin;
        currentTarget = target;
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
        StartCoroutine(moveToPosition(transform, target.transform.position, timeToAttack));
        yield return new WaitForSeconds(timeToAttack);
        OnAttackTargetReached?.Invoke(this);
        gameObject.SetActive(false);
    }

    private IEnumerator moveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
