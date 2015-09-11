using UnityEngine;
using System.Collections;

public class MortalObject : FallingObject 
{
	public bool IsDying()
	{
		return IsFalling();
	}
	
	public void Die()
	{
		Fall ();
	}

	public void CheckShouldDie()
	{
		IGridObject go = _gridObject.Grid.GetFromCell (_gridObject.GridPos + Vector3.down);
		if (go == null || go.GetName ().Contains ("Death")) 
		{
			Die();
		}
	}
}
