using UnityEngine;
using System.Collections;

public class RunnerRules : Rules 
{
	public GridObject hero;
	public float aimDistance = 100;
	private float _startDistance;
	protected MovingObject _movingObject;
	
	void Start()
	{
		_startDistance = hero.GridPos.x + hero.GridPos.z;
		_movingObject = hero.GetProperty<MovingObject> ();
	}

	protected override bool WinCondition()
	{
		return (hero.GridPos.x + hero.GridPos.z - _startDistance > aimDistance);
	}

	protected override bool LoseCondition()
	{
		return hero.GetProperty<MortalObject>() && hero.GetProperty<MortalObject>().IsDying();
	}

	protected override bool LoseAnimationFinished()
	{
		return hero.GetProperty<MortalObject>() && !hero.GetProperty<MortalObject>().IsDying ();
	}

	void OnGUI()
	{
		if(hero == null)
			return;

		GUI.Label (new Rect (0, 0, Screen.width * 0.2f, Screen.height / 10), (hero.GridPos.x + hero.GridPos.z - _startDistance).ToString () + "/" + aimDistance);
		if (_movingObject)
		{
			if (GUI.RepeatButton(new Rect (Screen.width * 0, Screen.height * 0.8f, Screen.width * 0.2f, Screen.height / 10), "\\")) {
				_movingObject.StartMovingTo (Vector3.forward);
			}
			if (GUI.RepeatButton (new Rect (Screen.width * 0.8f, Screen.height * 0.8f, Screen.width * 0.2f, Screen.height / 10), "/")) {
				_movingObject.StartMovingTo (Vector3.right);
			}
			if (GUI.RepeatButton (new Rect (Screen.width * 0, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height / 10), "/")) {
				_movingObject.StartMovingTo (Vector3.left);
			}
			if (GUI.RepeatButton (new Rect (Screen.width * 0.8f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height / 10), "\\")) {
				_movingObject.StartMovingTo (Vector3.back);
			}
			/*if (GUI.RepeatButton (new Rect (Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height / 10), "Jump")) {
				hero.StartMovingTo (Vector3.back);
			}*/
		}
	}
}
