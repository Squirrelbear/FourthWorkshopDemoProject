using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangedAttackBehaviour : MonoBehaviour
{
    public event Action<UnitRangedAttackBehaviour> OnAttackTargetReached;
    public event Action<UnitRangedAttackBehaviour> OnFinishedAllProjectiles;

    public GameObject projectilePrefab;
    public int projectileCount;

    public List<ProjectileBehaviour> projectiles;
    public List<ProjectileBehaviour> activeProjectiles;
    public int finishedProjectilesCount;

    public void Start()
    {
        projectiles = new List<ProjectileBehaviour>();
        activeProjectiles = new List<ProjectileBehaviour>();
    }

    public bool isExecutingAttack()
    {
        return activeProjectiles.Count > 0;
    }

    public void fireProjectiles(GameObject target, float timeToFireAll)
    {
        StartCoroutine(fireAllProjectiles(target, timeToFireAll));
    }

    private void spawnProjectiles()
    {
        while(projectiles.Count < projectileCount)
        {
            var projectile = Instantiate(projectilePrefab, gameObject.transform);
            projectiles.Add(projectile.GetComponent<ProjectileBehaviour>());
            projectile.SetActive(false);
        }
    }

    private IEnumerator fireAllProjectiles(GameObject target, float timeToFireAll)
    {
        finishedProjectilesCount = 0;
        float timeForEach = timeToFireAll / projectileCount;
        spawnProjectiles();
        activeProjectiles.AddRange(projectiles);
        while(activeProjectiles.Count > 0)
        {
            fireProjectile(activeProjectiles[0], target, timeForEach);
            yield return new WaitForSeconds(timeForEach);
        }
    }

    private void fireProjectile(ProjectileBehaviour projectileBehaviour, GameObject target, float timeToTarget)
    {
        activeProjectiles.Remove(projectileBehaviour);
        projectileBehaviour.setOrigin(transform.position);
        projectileBehaviour.gameObject.SetActive(true);
        projectileBehaviour.OnAttackTargetReached += handleProjectileReachedTarget;
        projectileBehaviour.beginAttack(target, timeToTarget);
    }

    private void handleProjectileReachedTarget(ProjectileBehaviour projectileBehaviour)
    {
        OnAttackTargetReached?.Invoke(this);
        projectileBehaviour.OnAttackTargetReached += handleProjectileReachedTarget;

        finishedProjectilesCount++;
        if(finishedProjectilesCount == projectileCount)
        {
            OnFinishedAllProjectiles?.Invoke(this);
        }
    }
}
