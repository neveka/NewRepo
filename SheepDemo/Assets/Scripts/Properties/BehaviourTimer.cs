using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BehaviourTimer : Trigger 
{
	public string behType;
	public List<GridObject> gridObjects = new List<GridObject>();

	public List<string> floatFields;
	public List<float> floatValues;
	public bool changeFloatsToOpposite;

	public float duration = 1;
	public bool startState = true;
	public bool endState;

	int dir = 1;
	bool busy;

	public override void OnTrigger()
	{
		if (busy)
			return;
		StartCoroutine (WaitAndSetBehState());
	}

	// Update is called once per frame
	protected IEnumerator WaitAndSetBehState () 
	{
		busy = true;
		if (gridObjects.Count == 0) 
		{
			gridObjects.Add(_gridObject as GridObject);
		}
		List<MonoBehaviour> behs = gridObjects.ConvertAll(gridObject=>gridObject.GetComponent (Type.GetType (behType)) as MonoBehaviour);
		foreach (MonoBehaviour beh in behs) 
		{
			for (int i=0; i<floatFields.Count; i++) {
				beh.GetType ().GetField (floatFields [i]).SetValue (beh, dir*floatValues [i]);
			}
			beh.enabled = startState;
		}
		yield return new WaitForSeconds (duration);
		foreach (MonoBehaviour beh in behs) 
		{
			beh.enabled = endState;
		}
		if (changeFloatsToOpposite) 
		{
			dir *= -1;
		}
		busy = false;
	}
}
