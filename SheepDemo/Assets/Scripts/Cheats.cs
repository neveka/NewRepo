using UnityEngine;
using System.Collections;

public class Cheats : MonoBehaviour 
{
	bool _cheats;

	bool _shadows = true;
	bool _geometry = true;
	bool _pathfinding = true;
	bool _light = true;
	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, Screen.width * 0.2f, Screen.height * 0.1f), _cheats?"X":"Cheats")) 
		{
			_cheats = !_cheats;
		}
		if (!_cheats)
			return;

		GUILayout.BeginArea(new Rect(0, Screen.height * 0.1f, Screen.width, Screen.height));
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button("Shadows:"+(_shadows?"on":"off"))) 
		{
			_shadows = !_shadows;
			ShowShadows(_shadows);
		}
		if (GUILayout.Button("Light:"+(_light?"on":"off"))) 
		{
			_light = !_light;
			EnableLight(_light);
		}
		if (GUILayout.Button("Geometry:"+(_geometry?"on":"off"))) 
		{
			_geometry = !_geometry;
			ShowGeomery(_geometry);
		}
		if (GUILayout.Button("Pathfinding:"+(_pathfinding?"on":"off"))) 
		{
			_pathfinding = !_pathfinding;
			EnablePathfinding(_pathfinding);
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}

	void ShowShadows(bool show)
	{
		Light light = GameObject.FindObjectOfType<Light> ();
		light.shadows = show?LightShadows.Hard:LightShadows.None;
	}

	void EnableLight(bool enable)
	{
		Light light = GameObject.FindObjectOfType<Light> ();
		light.enabled = enable;
	}

	void ShowGeomery(bool show)
	{
		Renderer[] renderers = GameObject.FindObjectsOfType<Renderer> ();
		foreach (Renderer r in renderers) 
		{
			r.enabled = show;
		}
	}

	void EnablePathfinding(bool enable)
	{
		MovingObject[] movings = GameObject.FindObjectsOfType<MovingObject> ();
		foreach (MovingObject m in movings) 
		{
			m.enabled = enable;
		}
	}
}
