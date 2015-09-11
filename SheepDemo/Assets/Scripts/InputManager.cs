using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour 
{
	
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D [] hitInfos = Physics2D.RaycastAll(ray.origin, ray.direction);
			System.Array.ForEach(hitInfos, h=>h.transform.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver));

			RaycastHit [] hitInfos3D = Physics.RaycastAll(ray);
			System.Array.ForEach(hitInfos3D, h=>h.transform.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver));
		}
	}
}
