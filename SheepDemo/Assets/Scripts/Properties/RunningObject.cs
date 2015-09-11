using UnityEngine;
using System.Collections;

public class RunningObject : MovingObject 
{
	protected override void Awake()
	{
		base.Awake ();
		if (right) 
		{
			_allNearCells = new Vector3[][]{ 
				new Vector3[]{ Vector3.right, Vector3.back, Vector3.forward, Vector3.left}, 
				new Vector3[]{ Vector3.right, Vector3.forward, Vector3.back, Vector3.left}};
		} 
		else 
		{
			_allNearCells = new Vector3[][]{ 
				new Vector3[]{ Vector3.forward, Vector3.right, Vector3.left, Vector3.back}, 
				new Vector3[]{ Vector3.right, Vector3.forward, Vector3.left, Vector3.back}, 
				new Vector3[]{ Vector3.right, Vector3.forward, Vector3.back, Vector3.left}, 
				new Vector3[]{ Vector3.forward, Vector3.right, Vector3.back, Vector3.left}};
		}
	}
	
	protected override void Update () 
	{
		base.Update();
		if (!IsMoving ()) 
		{
			Vector3[] nearCells = _allNearCells[Random.Range(0, _allNearCells.Length)];
			for(int i=0; i<nearCells.Length; i++)
			{
				Vector3 cellPos = nearCells[random?Random.Range(0,nearCells.Length):i];
				if(StartMovingTo(cellPos))
					break;
			}
		}
	}
	public bool right;
	public bool random;
	Vector3[][] _allNearCells;
}
