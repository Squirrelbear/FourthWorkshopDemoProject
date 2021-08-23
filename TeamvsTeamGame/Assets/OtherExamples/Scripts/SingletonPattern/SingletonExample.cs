using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonExample : MonoBehaviour
{
	public static SingletonExample instance { get; private set; }

	void Awake()
	{
		instance = this;
	}

	public void doSomethingMethod()
    {
		Debug.Log("I did something.");
    }

	/*
	For non-Monobehavior objects you could do:
	public SingletonExample() 
	{
		instance = this;
	}
	
	Another option is to have a method to get the instance and create it if it doesn't exist.
	
	public static SingletonExample getSingleton()
	{
		if(instance == null) {
			instance = new SingletonExample();
		}
		return instance;
	}
	*/
}
