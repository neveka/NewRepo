using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrajectoryObject : MovingObject 
{
	public List<Vector3> points = new List<Vector3>();
	public float rotatingSpeed;
	private int dir = 1;
	private int idx;
	public Vector3 rotatingAxis;

	// Update is called once per frame
	protected override void Update() 
	{
		base.Update ();
		if (!IsMoving () && points.Count>1) {
			Vector3 oldPoint = points [idx];
			int oldDir = dir;
			int oldIdx = idx;
			idx += dir;
			if (idx < 0) 
			{
				idx = 1;
				dir = 1;
			}
			if (idx >= points.Count) 
			{
				idx = points.Count - 2;
				dir = -1;
			}
			Vector3 offset = points [idx] - oldPoint;
			if(base.StartMovingTo (offset))
			{
				rotatingAxis = new Vector3(offset.z, 0, offset.x);
			}
			else
			{
				dir = oldDir;
				idx = oldIdx;
			}
		} 
		if (IsMoving () && rotatingSpeed>0) 
		{
			transform.Rotate (rotatingAxis, rotatingSpeed * Time.deltaTime);
		}
	}
	
}
