using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public int cooldown;
    public int stacks;
    public StatusEffectTemplate statusEffectTemplate;
    public GameObject attacker, defender;
    public bool isExpired;

    public StatusEffect(StatusEffectTemplate template, GameObject attacker, GameObject defender)
    {
        this.statusEffectTemplate = template;
        cooldown = template.duration;
        stacks = 1;
        this.attacker = attacker;
        this.defender = defender;
        isExpired = false;
    }

    public void refresh(StatusEffect effectToReApply)
    {
        if(stacks < statusEffectTemplate.maxStacks)
        {
            stacks++;
        }
        cooldown = effectToReApply.cooldown;
    }

    public void tick()
    {
        if (cooldown > 0)
        {
            cooldown--;
        }
        if(cooldown == 0 && statusEffectTemplate.duration != 0)
        {
            isExpired = true;
        }
    }
}
