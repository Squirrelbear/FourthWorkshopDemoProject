using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSheetBehaviour : MonoBehaviour
{
    public event Action<CharacterSheetBehaviour> OnStatsUpdated;
    public event Action<CharacterSheetBehaviour> OnStatusEffectsChanged;

    [SerializeField] private CharacterTemplate baseStats;
    [SerializeField] private UnitAbilityTemplate[] abilityTemplates;

    [SerializeField] private Dictionary<string, float> statValues;

    [SerializeField] public string characterName;
    [SerializeField] private HashSet<CharacterTemplate.CharacterTag> characterTags;

    public List<StatusEffect> statusEffects;
    public List<UnitAbility> abilityList;

    // Start is called before the first frame update
    void Awake()
    {
        statusEffects = new List<StatusEffect>();
        initialiseFromTemplate();
        CombatManager.OnCharacterTurnEnded += handleOwnTurnEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addStatusEffect(StatusEffect addEffect)
    {
        // increase by 1 for abilities cast on self to prevent tick out at end of turn
        if(addEffect.attacker == this)
        {
            addEffect.cooldown++;
        }
        bool found = false;
        foreach(StatusEffect effect in statusEffects)
        {
            if(effect.statusEffectTemplate.statusName.Equals(addEffect.statusEffectTemplate.statusName))
            {
                effect.refresh(addEffect);
                found = true;
                break;
            }
        }
        if(!found)
        {
            statusEffects.Add(addEffect);
        }

        OnStatusEffectsChanged?.Invoke(this);
    }

    public void dispellAll(bool buffs)
    {
        int beforeCount = statusEffects.Count;
        statusEffects = statusEffects
            .Where(effect => !effect.statusEffectTemplate.isBuff && effect.statusEffectTemplate.isDispellable)
            .ToList();
        if (beforeCount != statusEffects.Count)
        {
            OnStatusEffectsChanged?.Invoke(this);
        }
    }

    private void handleOwnTurnEnd(CharacterSheetBehaviour characterSheetBehaviour)
    {
        if(characterSheetBehaviour == this)
        {
            foreach(UnitAbility ability in abilityList)
            {
                if (ability.cooldown > 0) {
                    ability.cooldown--;
                }
            }
            foreach (StatusEffect effect in statusEffects)
            {
                effect.tick();
            }
            int effectCount = statusEffects.RemoveAll(effect => effect.isExpired);
            if(effectCount > 0)
            {
                OnStatusEffectsChanged?.Invoke(this);
            }
        }
    }

    public float getValue(string fieldName)
    {
        float result = 0;
        bool success = statValues.TryGetValue(fieldName, out result);
        if(!success)
        {
            Debug.LogWarning("Critical Error! Attempted to access " + fieldName + " on character: " + name);
        }
        return result;
    }

    public string getName()
    {
        return characterName;
    }

    public bool hasTag(CharacterTemplate.CharacterTag tag)
    {
        return characterTags.Contains(tag);
    }

    public void modifyStatByValue(string fieldName, float value)
    {
        if(!statValues.ContainsKey(fieldName))
        {
            Debug.LogWarning("Critical Error! Attempted to modify " + fieldName + " on character: " + name);
            return;
        }

        statValues[fieldName] = getValue(fieldName) + value;
        OnStatsUpdated?.Invoke(this);
    }

    private void initialiseFromTemplate()
    {
        statValues = new Dictionary<string, float>();
        characterName = baseStats.name;

        characterTags = new HashSet<CharacterTemplate.CharacterTag>();
        foreach(var charTag in baseStats.characterTags)
        {
            characterTags.Add(charTag);
        }

        statValues.Add("health", baseStats.health);
        statValues.Add("shield", baseStats.shield);
        statValues.Add("speed", baseStats.speed);
        statValues.Add("critDamageMultiplier", baseStats.critDamageMultiplier);
        statValues.Add("potency", baseStats.potency);
        statValues.Add("tenacity", baseStats.tenacity);
        statValues.Add("healthSteal", baseStats.healthSteal);
        statValues.Add("defencePenetration", baseStats.defencePenetration);
        statValues.Add("damagePhysical", baseStats.damagePhysical);
        statValues.Add("criticalChancePhysical", baseStats.criticalChancePhysical);
        statValues.Add("armourPenetration", baseStats.armourPenetration);
        statValues.Add("accuracyPhysical", baseStats.accuracyPhysical);
        statValues.Add("armour", baseStats.armour);
        statValues.Add("dodgeChance", baseStats.dodgeChance);
        statValues.Add("criticalAvoidancePhysical", baseStats.criticalAvoidancePhysical);
        statValues.Add("damageSpecial", baseStats.damageSpecial);
        statValues.Add("criticalChance", baseStats.criticalChance);
        statValues.Add("resistancePenetration", baseStats.resistancePenetration);
        statValues.Add("accuracySpecial", baseStats.acurracySpecial);
        statValues.Add("resistance", baseStats.resistance);
        statValues.Add("deflectionChance", baseStats.deflectionChance);
        statValues.Add("criticalAvoidanceSpecial", baseStats.criticalAvoidanceSpecial);

        statValues.Add("counterChance", 0);
        statValues.Add("turn", 0);

        abilityList = new List<UnitAbility>();
        foreach(UnitAbilityTemplate template in abilityTemplates)
        {
            abilityList.Add(new UnitAbility(template, gameObject));
        }
    }
}
