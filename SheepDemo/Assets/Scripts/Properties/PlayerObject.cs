using UnityEngine;
using System.Collections;

public class PlayerObject : MovingObject 
{
	public bool keyboard = true;
	public bool dragAndDrop;
	public float dragSpeed = 500;
	public float dragRadius = 2;
	protected bool _dragging;
	protected Vector3 _dragStartMouseGridPos;
	protected Vector3 _dragStartPlayerGridPos;
	protected float _normalSpeed;

	protected override void Awake()
	{
		_normalSpeed = speed;
		base.Awake();
	}

	protected override void Update () 
	{
		Vector3 input = keyboard?new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")):Vector3.zero;
		if (dragAndDrop) 
		{
			SetDragInputAndSpeed(ref speed, ref input);
		}

		if(Mathf.Abs(input.x)>Mathf.Abs(input.z))
		{
			input.z = 0;
		}
		else
		{
			input.x = 0;
		}
		input.Normalize ();
		StartMovingTo(input);

		base.Update();
	}

	void SetDragInputAndSpeed(ref float speed, ref Vector3 input)
	{
		speed = _dragging?dragSpeed:_normalSpeed;
		if(Input.GetMouseButtonDown (0))
		{
			_dragStartMouseGridPos = GridUtils.MousePosToGridPos(Input.mousePosition, _gridObject.GridPos.y, false);
			_dragStartPlayerGridPos = _gridObject.GridPos;
			if((_dragStartMouseGridPos-_dragStartPlayerGridPos).sqrMagnitude<dragRadius*dragRadius)
			{
				_dragging = true;
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			_dragging = false;
		}
		if(_dragging && !IsMoving())
		{
			Vector3 currentMouseGridPos =  GridUtils.MousePosToGridPos(Input.mousePosition, _gridObject.GridPos.y, false);
			Vector3 currentPlayerGridPos = _gridObject.GridPos;
			input = (currentMouseGridPos - currentPlayerGridPos) - (_dragStartMouseGridPos - _dragStartPlayerGridPos);
			/*if(input != Vector3.zero)
			{
				Debug.Log(this+" move "+input);
			}*/
		}
	}

	/*void OnDrawGizmos() 
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (GridUtils.GridPosToWorldPos(oldGridPos), Vector3.one);
	}*/
}
