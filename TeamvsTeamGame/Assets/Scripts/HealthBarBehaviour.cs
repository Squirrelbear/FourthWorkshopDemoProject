using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField]
    private UnitHealthBehaviour unitHealth;
    [SerializeField]
    private Slider currentHealthBar;
    [SerializeField]
    private Slider currentShieldBar;
    [SerializeField]
    private Slider currentProtectionBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthLabel;
    [SerializeField]
    private TMPro.TextMeshProUGUI shieldLabel;
    [SerializeField]
    private TMPro.TextMeshProUGUI protectionLabel;

    // Start is called before the first frame update
    void OnDisable()
    {
        if (unitHealth != null)
        {
            unitHealth.UnitHealthChanged -= updateHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAssociatedUnit(UnitHealthBehaviour unitHealthBehaviour)
    {
        if(unitHealth != null)
        {
            unitHealth.UnitHealthChanged -= updateHealth;
        }

        unitHealth = unitHealthBehaviour;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(unitHealth.gameObject.transform.position);
        transform.position = objectPos;
        unitHealth.UnitHealthChanged += updateHealth;
        updateHealth(unitHealth);
    }

    private void updateHealth(UnitHealthBehaviour unitHealth)
    {
        currentHealthBar.value = (float)unitHealth.healthCurrent / unitHealth.healthMax;
        currentShieldBar.value = (float)unitHealth.shieldCurrent / unitHealth.shieldMax;
        currentProtectionBar.value = (float)unitHealth.protectionCurrent / unitHealth.healthMax;

        protectionLabel.text = unitHealth.protectionCurrent.ToString();
        shieldLabel.text = unitHealth.shieldCurrent.ToString();
        healthLabel.text = unitHealth.healthCurrent.ToString();
    }
}
