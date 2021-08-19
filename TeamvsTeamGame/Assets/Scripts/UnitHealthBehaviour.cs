using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            protection += protection;
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
