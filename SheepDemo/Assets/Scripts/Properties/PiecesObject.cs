using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PiecesObject : SometimesDeathObject
{
	public List<GameObject> pieces = new List<GameObject>();
	public float fallSpeed = 0.1f;
	public float fallDelay = 1f;
	public float minY = -20;
	IGridObject _gridObject;

	void Awake()
	{
		_gridObject = GetComponent<IGridObject> ();
	}

	// Use this for initialization
	void OnEnable () {
		StartCoroutine (PiecesFall());
	}

	void OnDisable()
	{
		StopAllCoroutines ();
	}

	IEnumerator PiecesFall()
	{
		for (int i=0; i<pieces.Count; i++) 
		{
			yield return new WaitForSeconds(fallDelay);
			StartCoroutine(Fall(pieces[i], i==pieces.Count-1));
			if(i==pieces.Count-1)
			{
				IGridObject go = _gridObject.Grid.GetFromCell (_gridObject.GridPos + Vector3.up);
				if(go!=null && go.GetProperty<MortalObject>())
				{
					go.GetProperty<MortalObject>().Die();
				}
				UpdateDeathObject(false);
			}
		}
	}

	IEnumerator Fall(GameObject go, bool last)
	{
		while (go.transform.position.y>minY) 
		{
			go.transform.position += Vector3.down * fallSpeed;
			yield return null;
		}
		if (last) 
		{
			_gridObject.SetVisible(false);
		}
	}
}
