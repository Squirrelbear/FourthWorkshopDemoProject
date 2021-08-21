using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheetBehaviour : MonoBehaviour
{
    public event Action<CharacterSheetBehaviour> OnStatsUpdated;

    [SerializeField] private CharacterTemplate baseStats;

    [SerializeField] private Dictionary<string, float> statValues;

    [SerializeField] private string characterName;
    [SerializeField] private HashSet<CharacterTemplate.CharacterTag> characterTags;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        name = baseStats.name;

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
    }
}