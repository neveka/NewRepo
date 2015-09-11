using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ColorChance
{
	public Color color;
	public float chance;
}

public class RandomObject : MonoBehaviour 
{
	public Color invisibleColor = new Color(0,0,0,0);
	public List<ColorChance> colorChances = new List<ColorChance>();
	public Renderer targetRenderer;

	void OnAppear () 
	{
		if(!targetRenderer)
			targetRenderer = GetComponentInChildren<Renderer> ();
		float sum = 0;
		colorChances.ForEach (c => sum += c.chance);
		float dice = Random.Range (0, sum);
		int i = -1;
		while (dice>=0) 
		{
			i++;
			dice -= colorChances[i].chance;
		}
		if (targetRenderer) 
		{
			targetRenderer.material.color = colorChances[i].color;
		}
		IGridObject gridObject = GetComponent<IGridObject>();
		if(colorChances[i].color == invisibleColor || gridObject.Grid.GetAllFromCell(gridObject.GridPos).Count>1)
		{
			gridObject.SetVisible(false);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
