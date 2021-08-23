using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SampleScriptableObject", menuName = "SampleObjects/SampleScriptableObject", order = 1)]
public class SampleScriptableObject : ScriptableObject
{
	public string someStringData;
	public int someIntValue;
	public Vector3[] someListOfPoints;
}

