using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{
	public bool triggerOnStart;
	public bool triggerOnEnter;
	public bool triggerOnAppear;
	public bool triggerOnTap;
	protected IGridObject _gridObject;
	IGridObject _lastLoad;
	// Use this for initialization
	protected virtual void Start () 
	{
		_gridObject = GetComponent<IGridObject> ();
		if (triggerOnStart) 
		{
			OnTrigger();
		}
	}

	protected void OnTap () 
	{
		if(triggerOnTap)
		{
			OnTrigger();
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if (triggerOnEnter) 
		{
			IGridObject load = _gridObject.Grid.GetFromCell (_gridObject.GridPos + Vector3.up);
			if (load!=null && load != _lastLoad) 
			{
				_lastLoad = load;
				OnTrigger ();
			}
			if (load == null) 
			{
				_lastLoad = null;
			}
		}
	}

	public void OnAppear()
	{
		if (triggerOnAppear) 
		{
			OnTrigger();
		}
	}

	public bool CanPassThrough()
	{
		if (triggerOnEnter) 
		{
			OnTrigger();
			return true;
		}
		return false;
	}

	public virtual bool CanBeTriggered()
	{
		return true;
	}

	public virtual void OnTrigger()
	{
	}
}
