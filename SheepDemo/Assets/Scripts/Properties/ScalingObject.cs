using UnityEngine;
using System.Collections;

public class ScalingObject : SometimesDeathObject 
{
	//public float speed = 1;
	Renderer _renderer;
	Transform _transform;
	// Use this for initialization
	void Start () 
	{
		_transform = transform;
		_renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_transform.localScale = Vector3.one * (0.5f+Mathf.Cos (Time.time)/2);
		bool ok = _transform.localScale.x>0.8f;
		_renderer.material.color = ok? Color.green:Color.red;
		UpdateDeathObject(ok);
	}
}
