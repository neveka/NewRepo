using UnityEngine;
using System.Collections;

public class SaggingObject : MonoBehaviour 
{
	public float speed = 1;
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
		bool ok = load == null;
		Vector3 targetPos = _gridObject.GridPos + (ok ? Vector3.zero : (Vector3.down / 4));
		if (targetPos == _gridObject.Pos)
			return;

		Vector3 curPos = _gridObject.Pos;
		if ((targetPos - curPos).sqrMagnitude > Time.deltaTime * Time.deltaTime*speed*speed) 
		{
			targetPos = curPos+(targetPos-curPos).normalized*Time.deltaTime*speed;
		} 
		_gridObject.Pos = targetPos;
	}
}
