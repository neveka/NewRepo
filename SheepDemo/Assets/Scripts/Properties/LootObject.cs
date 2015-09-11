using UnityEngine;
using System.Collections;

public class LootObject : Trigger 
{
	public override void OnTrigger()
	{
		IGridObject gridObject = GetComponent<IGridObject> ();
		if (gridObject.GetProperty<FallingObject> ()) 
		{
			gridObject.GetProperty<FallingObject> ().Fall ();
		} 
		else 
		{
			gridObject.SetVisible (false);
		}
	}
}
