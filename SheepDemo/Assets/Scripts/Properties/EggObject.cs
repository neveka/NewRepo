using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EggObject : Trigger
{
	public GameObject prefabToSpawn;
	public Vector3 offset = Vector3.down;
	Renderer _renderer;
	bool _canBeTriggered;

	void Awake () 
	{
		_renderer = GetComponentInChildren<Renderer> ();
	}

	protected override void Update () 
	{
		base.Update ();
		_canBeTriggered = _gridObject.GridPos == _gridObject.Pos && _gridObject.Grid.GetFromCell(_gridObject.GridPos + offset) == null;
		if (_renderer) 
		{
			_renderer.material.color = _canBeTriggered?Color.white:Color.red;
		}
	}

	// Update is called once per frame
	public override void OnTrigger() 
	{
		if (_canBeTriggered) 
		{
			GameObject obj = GameObject.Instantiate (prefabToSpawn) as GameObject;
			obj.transform.parent = transform.parent;
			IGridObject gridObject = obj.GetComponent<IGridObject>();
			gridObject.Grid = _gridObject.Grid;
			gridObject.Pos = _gridObject.Pos + offset;
		}
	}

	public override bool CanBeTriggered()
	{
		return _canBeTriggered;
	}
}
