using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour, IGridObject
{
	[SerializeField]
	private IsoGrid _grid;
	private bool _active = true;
	private Transform _transform;

	public IIsoGrid Grid 
	{
		get{ return _grid; }
		set{ _grid = value as IsoGrid; }
	}

	public virtual bool IsVisible()//move to view
	{
		return gameObject && gameObject.activeSelf && _active;
	}

	public void SetActive(bool active)
	{
		if (_active == active)
			return;

		_active = active;
		UpdateObjectVisibility ();
	}

	public void SetVisible(bool visible)
	{
		if (visible == gameObject.activeSelf)
			return;

		gameObject.SetActive (visible);
		UpdateObjectVisibility ();
	}

	void UpdateObjectVisibility()
	{
		if (!IsVisible ()) 
		{
			_grid.RemoveObject (GridPos, this);
		} 
		else 
		{
			SnapToGrid();
		}
	}

	public string GetName()
	{
		return name;
	}

	public Vector3 Pos 
	{
		get{ 
			if(!_transform)
			{
				_transform = transform;
			}
			return GridUtils.WorldPosToGridPos(_transform.position); 
		}
		set{ 
			Vector3 oldViewPos = GridPos;
			_transform.position = GridUtils.GridPosToWorldPos(value);
			if (_grid) 
			{
				_grid.MoveObjectFromPosToPos(this, oldViewPos, GridPos, false);
			}; 
		}
	}

	public Vector3 GridPos 
	{
		get{ 		
			Vector3 pos = Pos;
			GridUtils.SnapToGrid(ref pos);
			return pos; 
		}
	}

	public void SetViewRotation(Vector3? center, Vector3 axis, float angle)//move to view
	{
		Vector3 oldViewPos = GridPos;
		if (center.HasValue) 
		{
			_transform.RotateAround (center ?? Vector3.zero, axis, angle);
		} 
		else 
		{
			_transform.Rotate(axis, angle);
		}
		if (_grid) 
		{
			_grid.MoveObjectFromPosToPos(this, oldViewPos, GridPos, false);
		}
	}

	public void SnapToGrid()
	{
		Pos = GridPos;
		if (_grid) 
		{
			_grid.MoveObjectFromPosToPos(this, GridPos, GridPos, true);
		}
	}

	public void SnapToAxis()
	{
		Vector3 euler = _transform.localRotation.eulerAngles;
		_transform.localRotation = 
			Quaternion.Euler(Mathf.RoundToInt(euler.x/90)*90, Mathf.RoundToInt(euler.y/90)*90, Mathf.RoundToInt(euler.z/90)*90);
	}

	public T GetProperty<T> ()
	{
		return GetComponent<T>();
	}
}
