using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IsoGrid : MonoBehaviour, IIsoGrid//make not beh
{
	//static int SIDE = 100;
	//private GridObject[] _cells = new GridObject[SIDE*SIDE*SIDE];

	Dictionary<Vector3, List<GridObject>> _objects = new Dictionary<Vector3, List<GridObject>>();

	/*int Vector3ToIndex(Vector3 pos)
	{
		return (int)(Mathf.Clamp((int)pos.x,-SIDE/2,SIDE/2-1)+SIDE/2)*SIDE*SIDE+
			(Mathf.Clamp((int)pos.y,-SIDE/2,SIDE/2-1)+SIDE/2)*SIDE+
				(Mathf.Clamp((int)pos.z,-SIDE/2,SIDE/2-1)+SIDE/2);
	}*/

	void Awake()
	{
		LevelBuilder.SnapAllGridChildren (this);
	}

	public void RemoveObject(Vector3 oldPos, IGridObject obj)
	{
		List<GridObject> list;
		if (_objects.TryGetValue (oldPos, out list)) 
		{
			list.Remove(obj as GridObject);
			if(list.Count == 0)
			{
				_objects.Remove(oldPos);
			}
		}
	}

	public void SetToCell(Vector3 pos, IGridObject value)
	{
		if (value == null) 
		{
			Debug.LogError("SetToCell "+pos+" null object");
		}
		List<GridObject> list;
		if (_objects.TryGetValue (pos, out list)) 
		{
			list.Add(value as GridObject);
		} 
		else
		{
			_objects.Add(pos, new List<GridObject>(){value as GridObject});
		}
	}

	public IGridObject GetFromCell(Vector3 pos)
	{
		IGridObject go = null;
		List<GridObject> list;
		if (_objects.TryGetValue (pos, out list) && list.Count>0) 
		{
			go = _objects[pos][0];
		}
		return go;
	}

	public List<IGridObject> GetAllFromCell(Vector3 pos)
	{
		List<GridObject> list;
		if (_objects.TryGetValue (pos, out list)) 
		{
			return list.ConvertAll(o=>(IGridObject)o);
		}
		return null;
	}

	public bool NeedUpdate()
	{
		return _objects.Count == 0;
	}

	public void Clear()
	{
		_objects.Clear ();
	}

	public void MoveObjectFromPosToPos(IGridObject obj, Vector3 oldPos, Vector3 newPos, bool force)
	{
		if(oldPos != newPos||force)//optimize
		{
			RemoveObject(oldPos, obj);
			SetToCell(newPos, obj);
		}
	}

	void OnDrawGizmos() 
	{
		foreach(KeyValuePair<Vector3, List<GridObject>> pair in _objects)
		{
			GridObject obj = pair.Value.Count>0?pair.Value[0]:null;
			Gizmos.color = obj?Color.yellow:Color.red;
			Gizmos.DrawWireCube (GridUtils.GridPosToWorldPos(pair.Key), Vector3.one);
		}
	}
}
