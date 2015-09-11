using UnityEngine;
using System.Collections;

public class TeleportFromObject : MonoBehaviour 
{
	public GridObject teleportToObject;
	IGridObject _gridObject;
	// Use this for initialization
	void Start () 
	{
		_gridObject = GetComponent<IGridObject> ();
	}


	// Update is called once per frame
	void Update () 
	{
		IGridObject load = _gridObject.Grid.GetFromCell (_gridObject.GridPos + Vector3.up);
		if (load!=null && teleportToObject!=null) 
		{
			if(load.GetProperty<MovingObject>())
			{
				load.GetProperty<MovingObject>().OnStop();
			}
			load.Pos = teleportToObject.GridPos + Vector3.up;
		}
	}
}
