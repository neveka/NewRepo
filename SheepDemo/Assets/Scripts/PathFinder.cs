using UnityEngine;
using System.Collections;

public class PathFinder
{
	IGridObject goForward;
	IGridObject goForwardUnder;
	Vector3[] points;

	public MovingPath FindPath(IGridObject goThis, Vector3 offset, float speed)
	{
		if (Find (goThis, offset, ref speed, false)) 
		{
			return new MovingPath (points, speed);
		}
		return null;
	}

	public MovingPath FindExtraPath(IGridObject goThis, Vector3 offset, float speed)
	{
		if (Find(goThis, offset, ref speed, true)) 
		{
			return new MovingPath (points, speed);
		}
		return null;
	}


	public enum PathType
	{
		LINE,
		LADDER,
		JUMP_ON,
		JUMP_OVER,
		JUMP_OVER_EXTRA
	}
	static Vector3[] GetPointsForType(Vector3 startPoint, Vector3 endPoint, PathType type)
	{
		Vector3[] points = null;
		if (type == PathType.LADDER) 
		{
			points = new Vector3[]{startPoint, 0.75f * startPoint + 0.25f * endPoint, 0.25f * startPoint + 0.75f * endPoint, endPoint};
			points [1].y = startPoint.y;
			points [2].y = endPoint.y;
		} 
		else if (type == PathType.JUMP_OVER||type == PathType.JUMP_OVER_EXTRA) 
		{
			points = new Vector3[]{startPoint, startPoint*5f/6f + endPoint/6f, 0.5f * startPoint + 0.5f * endPoint, startPoint/6f + endPoint*5f/6f, endPoint};
			points [2].y += 1;
			points [3].y += 1;
		}
		else
		{
			points = new Vector3[]{startPoint, endPoint};
		}
		return points;
	}
	
	bool Find(IGridObject goThis, Vector3 offset, ref float speed, bool extra)
	{
		points = null;
		IIsoGrid grid = goThis.Grid;
		Vector3 endPoint = goThis.GridPos + (extra?0:1)*offset;
		PathType type = PathType.LINE;

		GetForwardObjects (grid, endPoint);

		if (extra && goThis.GetProperty<MortalObject>()) //?
		{
			goThis.GetProperty<MortalObject>().CheckShouldDie();
		}

		if (goForwardUnder!=null && goForwardUnder.GetProperty<ArrowObject>()) 
		{
			offset = goForwardUnder.GetProperty<ArrowObject>().GetOffset();
		}

		if (goForwardUnder != null && goForwardUnder.GetProperty<SpeedChanger> ()) 
		{
			speed *= goForwardUnder.GetProperty<SpeedChanger> ().coef;
		}

		//ladders make path longer 
		if((goForward != null && goForward.GetName ().Contains("Ladder"))||(goForward == null && goForwardUnder != null && goForwardUnder.GetName ().Contains ("Ladder")))//move to ladder?
		{
			endPoint += (goForward!=null?Vector3.up:Vector3.down) + offset;
			GetForwardObjects(grid, endPoint);
			type = PathType.LADDER;
		}

		//jelly
		if (goForwardUnder!=null && goForwardUnder.GetName ().Contains ("Jelly") && //move to jelly?
		    grid.GetFromCell (endPoint + offset) == null) 
		{
			endPoint += 2 * offset;
			GetForwardObjects(grid, endPoint);
			type = PathType.JUMP_OVER;
		}

		if (goForwardUnder != null && goForwardUnder.GetProperty<PathObject> ()) 
		{
			points = goForwardUnder.GetProperty<PathObject> ().GetPoints(goThis.GridPos);
		}

		if (extra && endPoint == goThis.GridPos) 
		{
			return false;
		}

		if (points == null) 
		{
			points = GetPointsForType(goThis.GridPos, endPoint, type);
		}

		if (goForward != null && goForward.GetProperty<Trigger> () && goForward.GetProperty<Trigger> ().CanPassThrough()) 
		{
			goForward = null;
		}

		if (goThis.GetName().Contains("Flying") && goForward == null) 
		{
			return true;
		}

		if (goThis.GetProperty<PlayerTriggerObject> () && 
			(goForward == null || (goForward.GetProperty<Trigger> () && goThis.GetProperty<PlayerTriggerObject> ().triggers.Contains (goForward.GetProperty<Trigger> ())))) 
		{
			return true;
		}

		if ( goThis.GetProperty<TrajectoryObject>() && 
		    (goForward == null || (goForward.GetProperty<MovingObject>() && goForward.GetProperty<MovingObject>().platform == goThis)))//platform can fly
		{
			return true;
		}

		if (goForward == null && goForwardUnder != null) 
		{
			if (goThis.GetProperty<MovingObject>())//step to/from platform 
			{			
				goThis.GetProperty<MovingObject>().platform = (goForwardUnder .GetName().Contains("Platform"))?goForwardUnder:null;
			}
			return true;
		}
		return false;
	}

	void GetForwardObjects(IIsoGrid grid, Vector3 endPoint)
	{
		goForward = grid.GetFromCell(endPoint);
		goForwardUnder = grid.GetFromCell(endPoint+Vector3.down);
	}
}
