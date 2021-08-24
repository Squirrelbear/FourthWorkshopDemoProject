using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterSheetBehaviour))]
public class UnitHealthBehaviour : MonoBehaviour
{
    public event Action<UnitHealthBehaviour> UnitHealthChanged;
    public event Action<UnitHealthBehaviour> UnitDied;

    [SerializeField]
    public int healthCurrent;
    [SerializeField]
    public int shieldCurrent;
    [SerializeField]
    public int protectionCurrent;
    [SerializeField]
    public int shieldMax;
    [SerializeField]
    public int healthMax;
    [SerializeField]
    private bool invulnerable = false;

    private void Start()
    {
        CharacterSheetBehaviour charSheet = GetComponent<CharacterSheetBehaviour>();
        setHealth((int)charSheet.getValue("health"), (int)charSheet.getValue("shield"));
        charSheet.OnStatsUpdated += handleCharSheetValueChange;
    }

    private void handleCharSheetValueChange(CharacterSheetBehaviour characterSheetBehaviour)
    {
        bool statChanged = false;
        float statSheetHealth = characterSheetBehaviour.getValue("health");
        if (statSheetHealth != healthMax)
        {
            float healthPercent = (float)healthCurrent / healthMax;
            healthCurrent = (int)(healthPercent * statSheetHealth);
            healthMax = (int)statSheetHealth;
            statChanged = true;
        }
        float statSheetShield = characterSheetBehaviour.getValue("shield");
        if (statSheetShield != shieldMax)
        {
            float shieldPercent = (float)shieldCurrent / shieldMax;
            shieldCurrent = (int)(shieldPercent * statSheetShield);
            shieldMax = (int)statSheetShield;
            statChanged = true;
        }
        if(statChanged)
        {
            UnitHealthChanged?.Invoke(this);
        }
    }

    public bool isDead()
    {
        return healthCurrent <= 0;
    }

    public void setHealth(int maxHealth, int maxShield)
    {
        healthMax = healthCurrent = maxHealth;
        shieldCurrent = shieldMax = maxShield;
        protectionCurrent = 0;
    }

    public void increaseHealth(int baseHealth, int shield, int protection)
    {
        if(baseHealth > 0)
        {
            healthCurrent = Math.Min(healthCurrent + baseHealth, healthMax);
        }
        if (shield > 0)
        {
            shieldCurrent = Math.Min(shieldCurrent + shield, shieldMax);
        }
        if(protection > 0)
        {
            protectionCurrent += protection;
        }
        UnitHealthChanged?.Invoke(this);
    }

    public void increaseHealthByPercentOfMax(float baseHealthPercent, float shieldPercent, float protectionPercent)
    {
        increaseHealth((int)(baseHealthPercent * healthMax), (int)(shieldPercent * healthMax), (int)(protectionPercent * healthMax));
    }

    public void damageByValue(int amount, bool isPiercing)
    {
        if(invulnerable || amount <= 0 || isDead())
        {
            return;
        }

        if(protectionCurrent > 0 && !isPiercing)
        {
            protectionCurrent -= amount;
            if(protectionCurrent < 0)
            {
                amount = -protectionCurrent;
                protectionCurrent = 0;
            }
            else
            {
                amount = 0;
            }
        }
        if(amount > 0 && shieldCurrent > 0 && !isPiercing)
        {
            shieldCurrent -= amount;
            if(shieldCurrent < 0)
            {
                amount = -shieldCurrent;
                shieldCurrent = 0;
            }
            else
            {
                amount = 0;
            }
        }
        if(amount > 0)
        {
            healthCurrent -= amount;
            if(healthCurrent < 0)
            {
                healthCurrent = 0;
            }
        }

        UnitHealthChanged?.Invoke(this);
        if(isDead())
        {
            UnitDied?.Invoke(this);
        }
    }

    public void damageByPercentOfMax(float percent, bool isPiercing)
    {
        damageByValue((int)(percent * healthMax), isPiercing);
    }
}
