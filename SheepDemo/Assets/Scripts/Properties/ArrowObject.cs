using UnityEngine;
using System.Collections;

public class ArrowObject : MonoBehaviour 
{
	public GameObject arrow;

	public Vector3 GetOffset()
	{
		return arrow.transform.TransformDirection(Vector3.right);
	}

	protected void OnTap () 
	{
		arrow.transform.Rotate (0, 90, 0);
	}
}
