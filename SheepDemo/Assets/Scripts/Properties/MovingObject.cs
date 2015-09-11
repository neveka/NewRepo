using UnityEngine;
using System.Collections;
using System;

public class MovingObject : MonoBehaviour
{
	public float speed = 1;
	public IGridObject platform;//here?
	protected MovingPath _path;
	protected PathFinder _finder = new PathFinder();
	protected IGridObject _gridObject;

	protected Animator _animator;

	protected virtual void Awake()
	{
		_gridObject = GetComponent<IGridObject> ();
		_animator = GetComponentInChildren<Animator> ();
	}

	public bool StartMovingTo (Vector3 offset)
	{
		if(IsMoving() || !_gridObject.IsVisible() || offset==Vector3.zero) 
		{
			return false;
		}
		_path = _finder.FindPath (_gridObject, offset, speed);
		if(_path != null)
		{
			SetSpeed(_path.speed, offset);//?
			return true;
		}
		return false;
	}

	void SetSpeed(float speed, Vector3 offset)//move to gridobject?
	{
		if(_animator)
		{
			_animator.SetBool("moving", false);
			_animator.SetFloat("speed", speed);
			_animator.SetFloat("xDelta", offset.x);
			_animator.SetFloat("zDelta", offset.z);

			/*RuntimeAnimatorController animController = _animator.runtimeAnimatorController;
			for(int i=0; i<animController.animationClips.Length; i++)
			{
				AnimationClip clip = animController.animationClips[i];
				Debug.Log(i+") "+clip.name+" ");

				//clip.SetCurve("", typeof(SpriteRenderer), "m_Sprite", AnimationCurve.Linear(0, 3, 1, 3));
			}*/
		}
	}

	void LateUpdate()
	{
		if(IsMoving() && _animator)
		{
			_animator.SetBool("moving", true);
		}
	}
	
	public bool IsMoving()
	{
		return _path != null;
	}

	public void OnStop()
	{
		if (!IsMoving())
			return;
		_gridObject.Pos = platform != null?platform.Pos + Vector3.up:_path.GetTargetPosition();

		SetSpeed(0, Vector3.zero);
		_path = null;
	}
	
	protected virtual void Update()
	{
		if (_path != null) 
		{
			Vector3 delta;
			if(_path.UpdateMovement(Time.deltaTime, _gridObject.Pos, out delta))
			{
				_gridObject.Pos += delta;
			}
			else
			{
				Vector3 oldPos = _gridObject.Pos;
				OnStop();
				Vector3 offset = _gridObject.GridPos-oldPos;
				offset.y = 0;
				_path = _finder.FindExtraPath(_gridObject, offset.normalized, speed);
				if(_path != null)
				{
					SetSpeed(_path.speed, offset.normalized);//?
				}
			}
		}	



		if(platform != null && !IsMoving() && _gridObject.IsVisible())
		{
			_gridObject.Pos = platform.Pos + Vector3.up;//?
		}
	}

	void OnDrawGizmos() 
	{
		if(platform as GridObject) 
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine (platform.GridPos+Vector3.one*0.5f, transform.position);
		}
		Gizmos.color = Color.red;
		if(_gridObject != null)
			Gizmos.DrawLine (_gridObject.GridPos+Vector3.one*0.5f, transform.position);
	}
}
