using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public event Action<CharacterSheetBehaviour> OnCharacterTurnStart;
    public event Action<CharacterSheetBehaviour> OnCharacterTurnEnded;
    public event Action<CharacterSheetBehaviour> OnInitialSetup;
    public event Action<CharacterSheetBehaviour> OnPlayerTargetChanged;

    public Queue<QueuedAttack> attackQueue;
    public static CombatManager instance;

    public List<CharacterSheetBehaviour> playerUnits;
    public List<CharacterSheetBehaviour> enemyUnits;

    public CharacterSheetBehaviour activeTurnUnit;
    public CharacterSheetBehaviour currentPlayerTarget;

    public bool firstUpdate = true;

    void Awake()
    {
        instance = this;
        attackQueue = new Queue<QueuedAttack>();
        OnCharacterTurnEnded += handleEndOfTurn;
    }

    [SerializeField]
    private GameObject testAttackTarget;
    [SerializeField]
    private GameObject testAttacker;
    [SerializeField]
    private HealthBarBehaviour testHealthBar;

    private void Start()
    {
        testHealthBar.setAssociatedUnit(testAttackTarget.GetComponent<UnitHealthBehaviour>());
    }

    private void Update()
    {
        if(firstUpdate)
        {
            handleEndOfTurn(null);
            firstUpdate = false;
        }

        if(attackQueue.Count == 0)
        {
            return;
        }

        if(attackQueue.Peek().isDone)
        {
            attackQueue.Dequeue();
            if(attackQueue.Count == 0)
            {
                OnCharacterTurnEnded?.Invoke(activeTurnUnit);
            }
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
        List<CharacterSheetBehaviour> validTargets = getValidTargets(enemyUnits);
        if(validTargets.Count > 0)
        {
            currentPlayerTarget = validTargets[0];
            OnPlayerTargetChanged?.Invoke(currentPlayerTarget);
        }
    }

    public void attemptChangePlayerTarget(GameObject preferredTarget)
    {
        if(getValidTargets(enemyUnits).Contains(preferredTarget.GetComponent<CharacterSheetBehaviour>()))
        {
            currentPlayerTarget = preferredTarget.GetComponent<CharacterSheetBehaviour>();
            OnPlayerTargetChanged?.Invoke(currentPlayerTarget);
        }
    }

    public List<CharacterSheetBehaviour> getValidTargets(List<CharacterSheetBehaviour> fromSet)
    {
        List<CharacterSheetBehaviour> activeObjs = fromSet.Where(p => p.gameObject.activeSelf).ToList();
        List<CharacterSheetBehaviour> tauntObjs = new List<CharacterSheetBehaviour>();
        List<CharacterSheetBehaviour> stealthObjs = new List<CharacterSheetBehaviour>();

        foreach(CharacterSheetBehaviour obj in activeObjs)
        {
            if(obj.statusEffects.Where(effect => effect.statusEffectTemplate.statusName.Equals("Taunt")).Any())
            {
                tauntObjs.Add(obj);
            }
            if (obj.statusEffects.Where(effect => effect.statusEffectTemplate.statusName.Equals("Stealth")).Any())
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
            foreach(CharacterSheetBehaviour stealthObj in stealthObjs)
            {
                activeObjs.Remove(stealthObj);
            }
            return activeObjs;
        }
    }

    public void useAbility(UnitAbility ability)
    {
        InterfaceManager.instance.disableAllButtons();
        Debug.Log("Using ability: " + ability.abilitySequence.abilityName);
        foreach(StatusAction action in ability.abilitySequence.actionSequence)
        {
            action.performAction(ability.gameObject, currentPlayerTarget.gameObject);
        }
        ability.cooldown = ability.abilitySequence.cooldown+1;
        if(attackQueue.Count == 0)
        {
            OnCharacterTurnEnded?.Invoke(activeTurnUnit);
        }
    }

    public void handleEndOfTurn(CharacterSheetBehaviour characterWhoEndedTurn)
    {
        StartCoroutine(findNextReadyCharacter());
    }

    private IEnumerator findNextReadyCharacter()
    {
        CharacterSheetBehaviour readyChar;
        while((readyChar = getHighestReadyCharacter()) == null)
        {
            tickAllSpeed();
            yield return new WaitForSeconds(0.2f);
        }
        activeTurnUnit = readyChar;
        activeTurnUnit.modifyStatByValue("turn",-1);
        OnCharacterTurnStart?.Invoke(readyChar);
        yield return null;
    }

    private CharacterSheetBehaviour getHighestReadyCharacter()
    {
        CharacterSheetBehaviour maxReady = null;
        float maxCounter = 0;

        foreach (CharacterSheetBehaviour obj in playerUnits)
        {
            float objTurn = obj.getValue("turn");
            if (objTurn > 1 && objTurn > maxCounter)
            {
                maxReady = obj;
                maxCounter = objTurn;
            }
        }
        /*
        foreach (CharacterSheetBehaviour obj in enemyUnits)
        {
            float objTurn = obj.getValue("turn");
            if (objTurn > 1 && objTurn > maxCounter)
            {
                maxReady = obj;
                maxCounter = objTurn;
            }
        }*/

        return maxReady;
    }

    private void tickAllSpeed()
    {
        float maxSpeed = getMaximumSpeed();
        Debug.Log("Ticking with max speed: " + maxSpeed);
        foreach (CharacterSheetBehaviour obj in playerUnits)
        {
            obj.modifyStatByValue("turn", obj.getValue("speed") / (maxSpeed * 10));
        }
        foreach (CharacterSheetBehaviour obj in enemyUnits)
        {
            obj.modifyStatByValue("turn", obj.getValue("speed") / (maxSpeed * 10));
        }
    }

    private float getMaximumSpeed()
    {
        float max = 1;
        foreach(CharacterSheetBehaviour obj in playerUnits)
        {
            max = Math.Max(max, obj.getValue("speed"));
        }
        foreach (CharacterSheetBehaviour obj in enemyUnits)
        {
            max = Math.Max(max, obj.getValue("speed"));
        }
        return max;
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
