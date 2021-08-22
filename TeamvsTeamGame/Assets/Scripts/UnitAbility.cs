using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbility
{
    public int cooldown;
    public UnitAbilityTemplate abilitySequence;
    public GameObject gameObject;

    public UnitAbility(UnitAbilityTemplate template, GameObject gameObject)
    {
        abilitySequence = template;
        this.gameObject = gameObject;
    }
}
