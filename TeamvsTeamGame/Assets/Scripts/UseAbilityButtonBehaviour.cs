using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbilityButtonBehaviour : MonoBehaviour
{
    public UnitAbility linkedAbility;

    public void handleClick()
    {
        if(linkedAbility.cooldown == 0)
        {
            CombatManager.instance.useAbility(linkedAbility);
        }
    }
}
