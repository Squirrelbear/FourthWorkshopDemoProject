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
    [SerializeField]
    private TMPro.TextMeshProUGUI nameLabel;

    [SerializeField]
    private TMPro.TextMeshProUGUI turnMeterLabel;
    [SerializeField]
    private Slider turnMeterBar;

    public bool firstUpdate = true;

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
        if(firstUpdate)
        {
            firstUpdate = false;
            updateHealth(unitHealth);
        }
    }

    public void setAssociatedUnit(UnitHealthBehaviour unitHealthBehaviour)
    {
        if(unitHealth != null)
        {
            unitHealth.UnitHealthChanged -= updateHealth;
            unitHealth.gameObject.GetComponent<CharacterSheetBehaviour>().OnStatsUpdated -= updateTurnMeter;
        }

        unitHealth = unitHealthBehaviour;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(unitHealth.gameObject.transform.position);
        transform.position = objectPos;
        unitHealth.UnitHealthChanged += updateHealth;
        //updateHealth(unitHealth);
        CharacterSheetBehaviour charSheet = unitHealth.gameObject.GetComponent<CharacterSheetBehaviour>();
        nameLabel.text = charSheet.characterName;
        charSheet.OnStatsUpdated += updateTurnMeter;
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

    private void updateTurnMeter(CharacterSheetBehaviour characterSheetBehaviour)
    {
        float turnMeterPercent = Mathf.Min(1, characterSheetBehaviour.getValue("turn"));
        turnMeterBar.value = turnMeterPercent;
        turnMeterLabel.text = (turnMeterPercent*100).ToString("0") + "%";
    }
}
