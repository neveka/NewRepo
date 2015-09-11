using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathObject : MonoBehaviour 
{
	public GameObject railPrefab;
	public Vector3 constrains;
	public float step = 1;
	List<PathPoint> _points;
	List<Vector3> _interpolatedPoints;

	void Awake()
	{
		_points = new List<PathPoint> (transform.GetComponentsInChildren<PathPoint> ());
		if (_points.Count < 2)
			return;
		_interpolatedPoints = GetInterpolatedPoints ();

		SyncRails ();
	}
		
	void SyncRails()
	{
		if (!railPrefab || _interpolatedPoints == null)
			return;

		List<Transform> rails = new List<Transform>(transform.GetComponentsInChildren<Transform> ());
		rails.RemoveAll (r=>!r||!r.name.Contains("Rail"));//?
		if (railPrefab && rails.Count != _interpolatedPoints.Count-1) //sync creation
		{
			for (int i = 0; i<rails.Count; i++) 
			{
				DestroyImmediate (rails [i].gameObject);
			}
			rails.Clear ();
			for (int i=0; i<_interpolatedPoints.Count-1; i++) 
			{
				GameObject r = GameObject.Instantiate (railPrefab) as GameObject;
				r.transform.parent = transform;
				rails.Add (r.transform);
			}
		}
		if (rails!=null && rails.Count == _interpolatedPoints.Count - 1) //sync transforms
		{
			for (int i=0; i<_interpolatedPoints.Count-1; i++) 
			{
				rails [i].transform.position = (_interpolatedPoints [i] + _interpolatedPoints [i + 1]) / 2 + Vector3.down * 0.5f;
				Vector3 target = _interpolatedPoints [i + 1] + Vector3.down * 0.5f;
				if (constrains.x != 0) 
				{
					target.x = rails [i].transform.position.x;
				}
				if (constrains.y != 0) 
				{
					target.y = rails [i].transform.position.y;
				}
				if (constrains.z != 0) 
				{
					target.z = rails [i].transform.position.z;
				}
				rails [i].transform.LookAt (target);
			}
		}
	}

	List<Vector3> GetInterpolatedPoints()
	{
		Dictionary<double, double>[] known = {new Dictionary<double, double>(), new Dictionary<double, double>(), new Dictionary<double, double>()};
		float t = 0;
		for (int i=0; i<_points.Count; i++)
		{
			if(!known[0].ContainsKey(t))
			{
				known[0].Add(t, _points[i].transform.position.x);
				known[1].Add(t, _points[i].transform.position.y);
				known[2].Add(t, _points[i].transform.position.z);
			}
			if(i<_points.Count-1)
				t += (_points[i+1].transform.position-_points[i].transform.position).magnitude;
		}
		if (known [0].Count < 2)
			return null;

		SplineInterpolator[] spline = {new SplineInterpolator(known[0]), new SplineInterpolator(known[1]), new SplineInterpolator(known[2])};
		var start = 0;
		var end = t;

		List<Vector3> list = new List<Vector3> ();
		for (float k = start; k <= end; k += step)
		{
			list.Add(new Vector3((float)spline[0].GetValue(k), (float)spline[1].GetValue(k),  (float)spline[2].GetValue(k)));
		}
		return list;
	}

	void OnDrawGizmos() 
	{
#if UNITY_EDITOR
		Awake();
#endif
		Gizmos.color = Color.red;
		for (int i=0; i<_points.Count-1; i++) 
		{
			Gizmos.DrawLine(_points[i].transform.position, _points[i+1].transform.position);
		}
		if (_interpolatedPoints == null)
			return;
		Gizmos.color = Color.white;
		for (int i=0; i<_interpolatedPoints.Count-1; i++) 
		{
			Gizmos.DrawLine(_interpolatedPoints[i], _interpolatedPoints[i+1]);
		}
	}

	public Vector3[] GetPoints(Vector3 startPoint)
	{
		if (_interpolatedPoints == null)
			return new Vector3[0];
		List<Vector3> points = _interpolatedPoints.ConvertAll(p=>GridUtils.WorldPosToGridPos(p));
		points.Insert (0, startPoint);
		return points.ToArray ();
	}
}
