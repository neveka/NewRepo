using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Tile
{
	GameObject _gameObject;
	public static Tile CreateFromExisting(GameObject gameObject)
	{
		return new Tile (){_gameObject = gameObject};
	}

	public static Tile CreateNew(GameObject gameObject)
	{
		GameObject newGameObject = ((GameObject)GameObject.Instantiate (gameObject));
		newGameObject.transform.parent = gameObject.transform.parent;
		return new Tile (){_gameObject = newGameObject};
	}

	protected virtual IGridObject[] GetGridObjects()
	{
		return _gameObject.transform.GetComponentsInChildren<GridObject> (true);
	}

	public void AppearGridObjectsWithOffset(Vector3 offset)
	{
		Array.ForEach (GetGridObjects(), c => { 
			if(c != null)
			{		
				if((c as MonoBehaviour))
				{
					(c as MonoBehaviour).SendMessage("OnStop", SendMessageOptions.DontRequireReceiver);
				}
				c.SetActive(true);
				c.SetVisible(true);//move to OnStop
				c.Pos += offset;
				c.SnapToGrid();
			}
		});
		Array.ForEach (GetGridObjects(), c => { 
			if(c != null)
			{		
				if((c as MonoBehaviour))
				{
					(c as MonoBehaviour).SendMessage("OnAppear", SendMessageOptions.DontRequireReceiver);
				}
			}
		});
	}
}

public class TilingGround : MonoBehaviour 
{
	public GameObject tilingObject;//wrap to layer
	public GridObject hero;
	public Vector3 offset;
	public float forwardAdd = 16;
	public int tilesCount = 3;
	public int tilesCountBack = 1;
	public Vector3 shiftedCoords = new Vector3 (1, 0, 1);
	List<Tile> _tiles = new List<Tile>();
	Vector3 _passedOffset;

	public void Start()
	{
		Tile tile;
		for (int i=0; i<tilesCount; i++) 
		{
			if(i==0)
			{
				tile = Tile.CreateFromExisting(tilingObject);
			}
			else
			{
				tile = Tile.CreateNew(tilingObject);
			}
			_tiles.Add(tile);
		}
		for (int i=0; i<tilesCount; i++) 
		{
			_tiles[i].AppearGridObjectsWithOffset((i-tilesCountBack)*offset);
		}
	}

	void Update()
	{
		if (hero && Vector3.Dot(hero.GridPos, shiftedCoords) + forwardAdd > Vector3.Dot(offset+_passedOffset, shiftedCoords)) 
		{
			Debug.LogWarning("Add tile");
			_passedOffset += offset;
			_tiles[0].AppearGridObjectsWithOffset(offset*tilesCount);
			Tile tile = _tiles[0];
			_tiles.RemoveAt(0);
			_tiles.Add(tile);
		}
	}
}
