using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour 
{
	public float fallDistance = 10;
	public float fallSpeed = 5;	
	public bool up;
	bool _falls;
	protected IGridObject _gridObject;
	protected MovingObject _movingObject;

	void Awake()
	{
		_gridObject = GetComponent<IGridObject> ();
		_movingObject = _gridObject.GetProperty<MovingObject> ();
	}

	public void Fall()
	{
		if (_movingObject) 
		{
			_movingObject.platform = null;
		}
		FollowObject fo = Camera.main.gameObject.GetComponent<FollowObject> ();
		if (fo && fo.target == transform) 
		{
			Destroy(fo);
		}
		_falls = true;
		_gridObject.SetActive (false);
	}

	public bool IsFalling()
	{
		return _falls;
	}

	void Update()
	{
		if (_falls && (!_movingObject||!_movingObject.IsMoving())) 
		{
			if (fallDistance > 0) 
			{
				fallDistance -= fallSpeed*Time.deltaTime;
				_gridObject.Pos += Vector3.down*fallSpeed*Time.deltaTime*(up?-1:1); 
			}
			if(fallDistance<=0)
			{
				_falls = false;
				_gridObject.SetVisible(false);
			}
		}
	}
}
