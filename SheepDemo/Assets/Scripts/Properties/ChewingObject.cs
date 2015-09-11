using UnityEngine;
using System.Collections;

public class ChewingObject : SometimesDeathObject 
{
	public GameObject cap;
	public float height = 3;
	public float speed = 1;
	public float offset;
	protected GridObject _gridObject;
	Vector3 _startPosition;
	IGridObject _lastLoad;
	float _oldShift;

	// Use this for initialization
	void Start () 
	{
		_gridObject = GetComponent<GridObject> ();
		_startPosition = cap.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		IGridObject load = _gridObject.Grid.GetFromCell (_gridObject.GridPos + Vector3.up) as GridObject;
		if (load!=null && _lastLoad==null && _oldShift>0.5f) 
		{
			load = null;
		}
		if (_lastLoad!=null && load==null) 
		{
			offset = -Time.time;
		}
		_lastLoad = load;
		float shift = (load!=null ? 0 : (height * (1 - Mathf.Cos ((Time.time + offset) * speed)) / 2f));
		_oldShift = shift;
		cap.transform.position = _startPosition + Vector3.up * shift;
		UpdateDeathObject (shift<0.5f);
	}
}
