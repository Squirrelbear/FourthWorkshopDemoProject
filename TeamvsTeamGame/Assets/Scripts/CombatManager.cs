using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField]
    private GameObject testAttackTarget;
    [SerializeField]
    private GameObject testAttacker;
    [SerializeField]
    private HealthBarBehaviour testHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        testHealthBar.setAssociatedUnit(testAttackTarget.GetComponent<UnitHealthBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beginAttack(testAttacker, testAttackTarget);
        }
    }

    public void beginAttack(GameObject attacker, GameObject defender)
    {
        UnitAttackBehaviour unitAttackBehaviour = attacker.GetComponent<UnitAttackBehaviour>();
        unitAttackBehaviour.OnAttackTargetReached += applyAttackEffect;
        unitAttackBehaviour.executeAttackMove(defender, 1, 0.5f);
    }

    private void applyAttackEffect(UnitAttackBehaviour unitAttackBehaviour)
    {
        unitAttackBehaviour.OnAttackTargetReached -= applyAttackEffect;
        unitAttackBehaviour.currentTarget.GetComponent<UnitHealthBehaviour>().damageByValue(30, false);
    }
}
