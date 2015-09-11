using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour 
{
	void OnDrawGizmos() 
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube (transform.position, 0.2f*Vector3.one);
	}
}
