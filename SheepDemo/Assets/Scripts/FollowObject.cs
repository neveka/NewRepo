using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour 
{
	public Transform target;
	public Vector3 offset;

	public float corridorWidth = 3;
	Transform _transform;

	void Start()
	{
		_transform = transform;
	}

	void Update()
	{
		_transform.position = target.position + offset;

		if (corridorWidth >= 0) 
		{
			float width = corridorWidth;
			float oneDevidedSqrt2 = 1.41421356237f;;
			float deviation = (_transform.position.x - _transform.position.z)/ oneDevidedSqrt2;
			if (Mathf.Abs (deviation) > width) 
			{
				float delta = (Mathf.Abs (deviation)  - width) / oneDevidedSqrt2;
				if (deviation > 0) 
				{
					_transform.position += new Vector3 (-delta, 0, delta);
				} 
				else 
				{
					_transform.position += new Vector3 (delta, 0, -delta);
				}
				//Debug.LogWarning ("out " + deviation + " -> " + (_transform.position.x - _transform.position.z)/ oneDevidedSqrt2);
			}
		}
	}
}
