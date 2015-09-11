using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomDialog : MonoBehaviour 
{
	public List<string> levels = new List<string>();
	public static bool wasLoaded;

	void Awake()
	{
		wasLoaded = true;
	}

	void OnGUI()
	{
		GUI.Button (new Rect (Screen.width*0.2f, Screen.height*0.2f, Screen.width*0.6f, Screen.height*0.5f), "King: Dolly! \nRun 100 meters to win level!");

		if (GUI.Button (new Rect (Screen.width * 0.3f, Screen.height * 0.8f, Screen.width * 0.4f, Screen.height * 0.1f), "Tap to continue")) 
		{
			Application.LoadLevel(levels[Random.Range(0, levels.Count)]);
		}
	}
}
