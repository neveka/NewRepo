using UnityEngine;
using System.Collections;
using System;

public class LevelBuilder
{
	public static void SnapAllGridChildren(IIsoGrid grid)
	{
		grid.Clear ();

		IsoGrid gridBeh = grid as IsoGrid;
		IGridObject[] objects = gridBeh.transform.GetComponentsInChildren<IGridObject> ();
		Array.ForEach (objects, c => { 
			if(c != null)
			{
				c.Grid = grid;
				c.SnapToGrid ();
			}
		});
	}
}
