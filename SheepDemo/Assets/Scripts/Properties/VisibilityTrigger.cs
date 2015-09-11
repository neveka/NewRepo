using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisibilityTrigger : Trigger 
{
	public List<GridObject> onObjects;
	public List<GridObject> offObjects;
	bool _on;

	public override void OnTrigger()
	{
		_on = !_on;
		foreach (GridObject o in onObjects) 
		{
			o.SetVisible(_on);
		}
		foreach (GridObject o in offObjects) 
		{
			o.SetVisible(!_on);
		}
	}
}
