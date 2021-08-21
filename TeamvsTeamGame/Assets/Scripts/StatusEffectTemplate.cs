using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectTemplate", menuName = "Team Objects/New StatusEffectTemplate", order = 2)]
public class StatusEffectTemplate : ScriptableObject
{
    [SerializeField] public bool isBuff;
    [SerializeField] public string statusName;
    [SerializeField] public bool isPreventable;
    [SerializeField] public bool isDispellable;
    [SerializeField] public bool isCopyable;
    [SerializeField] public int duration;
    [SerializeField] public StatusAction[] actionList;

    public void applyEffect(GameObject attacker, GameObject defender)
    {

    }

    public void removeEffect(GameObject defender)
    {

    }
}
