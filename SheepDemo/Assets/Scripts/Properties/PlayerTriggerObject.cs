using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTriggerObject : PlayerObject 
{
	public List<Trigger> triggers = new List<Trigger>();
	public bool moveTogether;
	public bool reorderOnAlt;
	protected Vector3 _downMousePos;

	protected override void Update () 
	{
		Vector3 oldPos = _gridObject.Pos;
		base.Update ();
		if (Input.GetKey (KeyCode.Space)||(dragAndDrop && Input.GetMouseButtonUp(0) && (Input.mousePosition-_downMousePos).sqrMagnitude<1 )) 
		{
			TriggerTriggers();
		}
		if(dragAndDrop && Input.GetMouseButtonDown(0))
		{
			_downMousePos = Input.mousePosition;
		}
		if (reorderOnAlt && (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) && !IsMoving()) 
		{
			ReorderTriggers();
			return;
		}
		if (moveTogether && _gridObject.Pos!=oldPos) 
		{
			UpdateTriggersPos(oldPos);
		}
	}

	protected virtual void TriggerTriggers()
	{
		bool canBeTriggered = true;
		triggers.ForEach (t => {
			if (!t.CanBeTriggered())
				canBeTriggered = false;});
		if(canBeTriggered)
		{
			triggers.ForEach(t=>t.OnTrigger());
		}
	}

	protected virtual void ReorderTriggers()
	{
		bool[] places = new bool[9];
		triggers.ForEach(t=>{
			IGridObject gridObject = t.GetComponent<IGridObject>();
			if(gridObject != null && gridObject != _gridObject)
			{
				int place = Random.Range(0, places.Length);
				places[4] = true;
				while(places[place])
				{
					place = Random.Range(0, places.Length);
				}
				places[place] = true;
				gridObject.Pos = _gridObject.Pos+Vector3.right*(-1+place/3)+Vector3.forward*(-1+(place%3));
			}
		});
	}

	protected virtual void UpdateTriggersPos(Vector3 oldPos)
	{
		triggers.ForEach(t=>{
			IGridObject gridObject = t.GetComponent<IGridObject>();
			if(gridObject != null && gridObject != _gridObject)
			{
				gridObject.Pos += _gridObject.Pos-oldPos;
			}
		});
	}
}
