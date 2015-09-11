using UnityEngine;
using System.Collections;

public class Rules : MonoBehaviour 
{
	enum GameState
	{
		GAMEPLAY,
		FINISH_ANIMATION,
		//COMPLETE
	}

	private GameState _gameState = GameState.GAMEPLAY;

	protected virtual void Update()
	{

		if (_gameState == GameState.GAMEPLAY) 
		{
			if (WinCondition ()) 
			{
				Debug.Log ("Level win!");
				CompleteLevel ();
			}

			if (LoseCondition ()) 
			{
				Debug.Log ("Level loose!");
				_gameState = GameState.FINISH_ANIMATION;
			}
		}

		if (_gameState == GameState.FINISH_ANIMATION && LoseAnimationFinished ()) 
		{
			CompleteLevel();
		}
	}

	void CompleteLevel()
	{
		if (RoomDialog.wasLoaded) 
		{
			Application.LoadLevel ("room1");
		} 
		else 
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}

	protected virtual bool LoseAnimationFinished()
	{
		return true;
	}

	protected virtual bool WinCondition()
	{
		return false;
	}

	protected virtual bool LoseCondition()
	{
		return false;
	}
}
