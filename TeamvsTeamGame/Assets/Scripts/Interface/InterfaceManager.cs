using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public List<GameObject> abilityIconSet;

    public static InterfaceManager instance;

    public GameObject unitStatusInterfacePrefab;
    public GameObject interfaceParentReference;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CombatManager.OnCharacterTurnStart += handleCharacterChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableAllButtons()
    {
        foreach(GameObject button in abilityIconSet)
        {
            button.SetActive(false);
        }
    }

    private void handleCharacterChange(CharacterSheetBehaviour characterSheetBehaviour)
    {
        UnitAbility[] abilities = characterSheetBehaviour.abilityList.ToArray();
        for(int i = 0; i < abilityIconSet.Count; i++)
        {
            if(i >= abilities.Length)
            {
                abilityIconSet[i].SetActive(false);
            }
            else
            {
                abilityIconSet[i].GetComponent<UseAbilityButtonBehaviour>().linkAbility(abilities[i]);
                abilityIconSet[i].SetActive(true);
            }
        }
    }

    public void setupInterfaceForUnits(List<CharacterSheetBehaviour> unitList)
    {
        foreach (CharacterSheetBehaviour unit in unitList)
        {
            var newObj = Instantiate(unitStatusInterfacePrefab, interfaceParentReference.transform);
            newObj.GetComponent<HealthBarBehaviour>().setAssociatedUnit(unit.GetComponent<UnitHealthBehaviour>());
        }
    }
}
