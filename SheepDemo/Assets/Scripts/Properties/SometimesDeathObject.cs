using UnityEngine;
using System.Collections;

public class SometimesDeathObject : MonoBehaviour 
{
	protected void UpdateDeathObject(bool ok)
	{
		if (ok && name.Contains("Death")) 
		{
			name = name.Replace("Death", "");
		}
		if (!ok && !name.Contains("Death")) 
		{
			name += "Death";
		}
	}
}
