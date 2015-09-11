using UnityEngine;
using System.Collections;

public static class GridUtils
{
	public static void SnapToGrid(ref Vector3 pos)
	{
		pos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
	}
	
	public static Vector3 WorldPosToGridPos(Vector3 worldPos)
	{
		return worldPos - Vector3.one*0.5f;
	}

	public static Vector3 GridPosToWorldPos(Vector3 gridPos)
	{
		return gridPos + Vector3.one*0.5f;
	}
	
	public static Vector3 MousePosToGridPos(Vector3 mousePos, float level, bool forEditor)
	{
		if (forEditor) 
		{
			mousePos.y = Screen.height - (mousePos.y + 50);
		}
		Camera camera = forEditor ? Camera.current : Camera.main;
		Ray ray = camera.ScreenPointToRay(mousePos);
		float k = (level-ray.origin.y)/ray.direction.normalized.y;
		Vector3 newPos = ray.origin+k*ray.direction.normalized;
		//if (forEditor) 
		{
			newPos = new Vector3 (Mathf.Round (newPos.x), Mathf.Round (newPos.y), Mathf.Round (newPos.z));
		}
		return newPos;
	}
}
