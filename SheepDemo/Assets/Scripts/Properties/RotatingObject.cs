using UnityEngine;
using System.Collections;

public class RotatingObject : SometimesDeathObject 
{
	public Vector3 axis;
	public float speed = 1;

	public Vector3 rotationCenterOffset;
	public float greenAngle = 120;
	public float delay;
	public float okDegrees = 20;
	float _totalAngle;

	Renderer _renderer;
	IGridObject _gridObject;
	Vector3 _startPosition;

	// Use this for initialization
	void Awake () 
	{
		_startPosition = transform.position;
		_renderer = GetComponent<Renderer> ();
		_gridObject = GetComponent<IGridObject> ();
		_totalAngle = delay;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (rotationCenterOffset != Vector3.zero) 
		{
			_gridObject.SetViewRotation (_startPosition + rotationCenterOffset, axis, speed * Time.deltaTime);
		} 
		else 
		{
			_gridObject.SetViewRotation (null, axis, speed * Time.deltaTime);
		}
		_totalAngle += speed * Time.deltaTime;
		_totalAngle %= 360;
		if (_renderer) 
		{
			bool ok = Mathf.Abs (_totalAngle) < okDegrees || Mathf.Abs (_totalAngle - greenAngle) < okDegrees || Mathf.Abs (_totalAngle - greenAngle*2) < okDegrees || Mathf.Abs (_totalAngle - greenAngle*3) < okDegrees;
			_renderer.material.color = ok ? Color.green : Color.red;
			UpdateDeathObject(ok);
		}
	}

	void OnStop()
	{
		if (_totalAngle != delay) 
		{
			_gridObject.SetViewRotation (_startPosition + rotationCenterOffset, axis, -_totalAngle+delay);
			_totalAngle = delay;
		}
	}

	void OnAppear()
	{
		_startPosition = transform.position;
	}

	void OnDisable()//?
	{
		_gridObject.SnapToAxis ();
		_gridObject.SnapToGrid ();
	}
}
