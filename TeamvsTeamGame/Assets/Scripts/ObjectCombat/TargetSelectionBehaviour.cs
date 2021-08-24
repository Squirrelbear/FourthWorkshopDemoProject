using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectionBehaviour : MonoBehaviour
{
    public enum SelectionStatus { None, Ally, Enemy }

    private CharacterSheetBehaviour cachedCharSheet = null;
    private Image _image;
    //private MaterialPropertyBlock _propBlock;
    private SelectionStatus selectionStatus = SelectionStatus.None;

    private void Start()
    {
        _image = GetComponent<Image>();
        CombatManager.OnPlayerTargetChanged += handlePlayerTargetChanged;
        CombatManager.OnAllyTargetSelectionChanged += handleAllyTargetChanged;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerTargetChanged -= handlePlayerTargetChanged;
        CombatManager.OnAllyTargetSelectionChanged -= handleAllyTargetChanged;
    }

    public void handleAttemptSelection()
    {
        CharacterSheetBehaviour associatedBehaviour = getCharSheet();
        CombatManager.instance.attemptChangeAllyTarget(associatedBehaviour);
        CombatManager.instance.attemptChangePlayerTarget(associatedBehaviour.gameObject);
    }

    private void handlePlayerTargetChanged(CharacterSheetBehaviour characterSheetBehaviour)
    {
        if(getCharSheet() == characterSheetBehaviour)
        {
            if(selectionStatus != SelectionStatus.Ally)
            {
                changeColour(Color.red);
                selectionStatus = SelectionStatus.Enemy;
            }
        }
        else
        {
            if (selectionStatus != SelectionStatus.Ally)
            {
                changeColour(Color.white);
                selectionStatus = SelectionStatus.None;
            }
        }
    }

    private void handleAllyTargetChanged(CharacterSheetBehaviour characterSheetBehaviour)
    {
        if (getCharSheet() == characterSheetBehaviour)
        {
            if (selectionStatus != SelectionStatus.Enemy)
            {
                changeColour(Color.green);
                selectionStatus = SelectionStatus.Ally;
            }
        }
        else
        {
            if (selectionStatus != SelectionStatus.Enemy)
            {
                changeColour(Color.white);
                selectionStatus = SelectionStatus.None;
            }
        }
    }

    private CharacterSheetBehaviour getCharSheet()
    {
        if(cachedCharSheet == null)
        {
            cachedCharSheet = GetComponent<HealthBarBehaviour>()
                            .getAssociatedUnit().GetComponent<CharacterSheetBehaviour>();
        }
        return cachedCharSheet;
    }

    private void changeColour(Color color)
    {
        _image.color = color;
    }
}
