using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public event Action<GameObject> OnCharacterTurnEnded;
    public event Action<GameObject> OnInitialSetup;
    public event Action<GameObject> OnPlayerTargetChanged;

    public Queue<QueuedAttack> attackQueue;
    public static CombatManager instance;

    public List<GameObject> playerUnits;
    public List<GameObject> enemyUnits;

    public GameObject currentPlayerTarget; 

    void Awake()
    {
        instance = this;
        attackQueue = new Queue<QueuedAttack>();
    }

    private void Update()
    {
        if(attackQueue.Count == 0)
        {
            return;
        }

        if(attackQueue.Peek().isDone)
        {
            attackQueue.Dequeue();
        }
        else if(!attackQueue.Peek().isStarted)
        {
            attackQueue.Peek().beginAttack();
        }
    }

    public void queueAttack(QueuedAttack queuedAttack)
    {
        attackQueue.Enqueue(queuedAttack);
    }

    public void selectFirstValidPlayerTarget()
    {
        List<GameObject> validTargets = getValidTargets(enemyUnits);
        if(validTargets.Count > 0)
        {
            currentPlayerTarget = validTargets[0];
            OnPlayerTargetChanged?.Invoke(currentPlayerTarget);
        }
    }

    public void attemptChangePlayerTarget(GameObject preferredTarget)
    {
        if(getValidTargets(enemyUnits).Contains(preferredTarget))
        {
            currentPlayerTarget = preferredTarget;
            OnPlayerTargetChanged?.Invoke(currentPlayerTarget);
        }
    }

    public List<GameObject> getValidTargets(List<GameObject> fromSet)
    {
        List<GameObject> activeObjs = fromSet.Where(p => p.activeSelf).ToList();
        List<GameObject> tauntObjs = new List<GameObject>();
        List<GameObject> stealthObjs = new List<GameObject>();

        foreach(GameObject obj in activeObjs)
        {
            CharacterSheetBehaviour charSheet = obj.GetComponent<CharacterSheetBehaviour>();
            if(charSheet.statusEffects.Where(effect => effect.statusEffectTemplate.statusName.Equals("Taunt")).Any())
            {
                tauntObjs.Add(obj);
            }
            if (charSheet.statusEffects.Where(effect => effect.statusEffectTemplate.statusName.Equals("Stealth")).Any())
            {
                stealthObjs.Add(obj);
            }
        }

        if (tauntObjs.Count > 0)
        {
            return tauntObjs;
        }
        else if (stealthObjs.Count == activeObjs.Count)
        {
            return stealthObjs;
        }
        else
        {
            foreach(GameObject stealthObj in stealthObjs)
            {
                activeObjs.Remove(stealthObj);
            }
            return activeObjs;
        }
    }

    public void useAbility(UnitAbility ability)
    {
        Debug.Log("Using ability: " + ability.name);
        foreach(StatusAction action in ability.abilitySequence.actionSequence)
        {
            action.performAction(ability.gameObject, currentPlayerTarget);
        }
    }

    /*[SerializeField]
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

    public void useAbility(UnitAbility unitAbility)
    {

    }*/
}
