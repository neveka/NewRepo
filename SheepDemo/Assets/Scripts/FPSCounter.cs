using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour 
{
	private string fpsText;
	private Color color;

	public  float updateInterval = 0.5F;

	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	void Start()
	{
		timeleft = updateInterval; 
	}
	
	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			fpsText = System.String.Format("{0:F2} FPS",fps);

			if(fps < 30)
				color = Color.yellow;
			else 
				if(fps < 10)
					color = Color.red;
			else
				color = Color.green;
			//	DebugConsole.Log(format,level);
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width*0.8f, 0, Screen.width*0.2f, Screen.height));
		GUI.color = color;
		GUILayout.Label(fpsText);
		GUI.color = Color.white;
		GUILayout.EndArea();
	}
}
