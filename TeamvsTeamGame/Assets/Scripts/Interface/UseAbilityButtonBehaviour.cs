using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAbilityButtonBehaviour : MonoBehaviour
{
    public UnitAbility linkedAbility;
    public Image imageRef;
    public GameObject cooldownOverlay;
    public TMPro.TextMeshProUGUI cooldownText;

    public void handleClick()
    {
        if (linkedAbility.cooldown == 0 && !linkedAbility.abilitySequence.isPassive)
        {
            CombatManager.instance.useAbility(linkedAbility);
        }
    }

    public void linkAbility(UnitAbility unitAbility)
    {
        linkedAbility = unitAbility;
        imageRef.sprite = unitAbility.abilitySequence.abilityIcon;
        cooldownOverlay.SetActive(linkedAbility.cooldown != 0);
        cooldownText.text = linkedAbility.cooldown.ToString();
        cooldownText.enabled = linkedAbility.cooldown != 0;
    }
}
