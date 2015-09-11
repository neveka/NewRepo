using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class GridObjectsFactory
{
	public static GridObject Create(GameObject prefab)
	{
		GameObject go = (PrefabUtility.InstantiatePrefab(prefab) as GameObject);
		GridObject gridObject = go.GetComponent<GridObject>();
		if(!gridObject)
		{
			gridObject = go.AddComponent<GridObject>();
		}
		return gridObject;
	}
}

public class LevelEditorWindow : EditorWindow 
{
	private int _layer = 1;
	private Vector3 _cursorPos;
	private bool _edithinOn;
	private Vector2 _scrollPosition;
	private GridObject _currentCube;
	private GridObject _underMouseCube;
	private IsoGrid _grid;
	public List<GameObject> cubePrefabs = new List<GameObject>();

	[MenuItem ("Iso/Tool Bar")]
	public static void  ShowWindow () 
	{
		EditorWindow.GetWindow(typeof(LevelEditorWindow));
	}
	
	void OnGUI () 
	{
		_scrollPosition = GUILayout.BeginScrollView (_scrollPosition);
		bool edithinOn = GUILayout.Toggle (_edithinOn, "Editing");
		if (edithinOn != _edithinOn) 
		{
			_edithinOn = edithinOn;
			UpdateToolbar();
		}
		for(int i=0; i<cubePrefabs.Count; i++)
			if(GUILayout.Button(cubePrefabs[i].name) && _edithinOn)
		{
			if(_currentCube)
				DestroyImmediate(_currentCube.gameObject);
			_currentCube = GridObjectsFactory.Create(cubePrefabs[i]);
		}
		GUILayout.Space(10);
		if(GUILayout.Button("Empty"))
		{
			if(_currentCube)
				DestroyImmediate(_currentCube.gameObject);
		}
		if(GUILayout.Button("Update")||!_grid||(_grid.NeedUpdate() && _grid.GetComponentInChildren<IGridObject>()!=null))
		{
			UpdateToolbar();
		}
		GUILayout.Label("Cursor position: "+_cursorPos.ToString());
		if(GUILayout.Button("^"))
		{
			_cursorPos += Vector3.up;
		}		
		if(GUILayout.Button("v"))
		{
			_cursorPos += Vector3.down;
		}
		GUILayout.Label("Cursor layer: "+_layer.ToString());
		if(GUILayout.Button("+"))
		{
			_layer ++;
		}		
		if(GUILayout.Button("-"))
		{
			_layer--;
		}
		FindGrid();
		int side = 10;
		for (int k=0; k<=1; k++) 
		{
			for (int i=-side; i<side; i++) 
			{
				string str = "";
				for (int j=-side; j<side; j++) 
				{
					IGridObject go = _grid.GetFromCell (new Vector3 (i, k, j));
					str += (go == null ? "0" : "1");
				}
				GUILayout.Label (str);
			}
			GUILayout.Label ("");
		}
		GUILayout.EndScrollView();
	}	

	void UpdateToolbar()
	{
		if(_currentCube)
			DestroyImmediate(_currentCube.gameObject);
		SceneView.onSceneGUIDelegate += SceneGUI;
		FindGrid();
		_cursorPos = Vector3.zero;
		UpdatePrefabsButtons ();
		LevelBuilder.SnapAllGridChildren (_grid);
	}

	void UpdatePrefabsButtons()
	{
		cubePrefabs.Clear();
		DirectoryInfo dir = new DirectoryInfo("Assets/Prefabs/Blocks/");
		FileInfo[] info = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
		foreach (FileInfo f in info) 
		{
			string fullPath = f.FullName.Replace("\\", "/");
			int dataPathLength = Application.dataPath.Length;
			string relativePath = fullPath.Substring(dataPathLength);
			GameObject go = AssetDatabase.LoadAssetAtPath("Assets"+relativePath, typeof(GameObject)) as GameObject;
			if(go)
				cubePrefabs.Add(go);
		}
	}

	void FindGrid()
	{
		if(_grid)
			return;
		_grid = GameObject.FindObjectOfType<IsoGrid>();
		if(!_grid)
		{
			_grid = new GameObject("IsoGrid").AddComponent<IsoGrid>();
		}
	}

	Transform FindLayer()
	{
		Transform layer = _grid.transform.FindChild("Layer"+_layer);
		if(!layer)
		{
			layer = (new GameObject("Layer"+_layer)).transform;
			layer.parent = _grid.transform;
		}
		return layer;
	}

	public GridObject AddNewCubeToGrid(GridObject cube)
	{
		GridObject newCube =  GridObjectsFactory.Create(cubePrefabs.Find(c=>c.name == cube.name));
		newCube.transform.rotation = cube.transform.rotation;
		newCube.transform.parent = FindLayer();
		newCube.Grid = _grid;
		newCube.Pos = _cursorPos;
		return newCube;
	}

	[MenuItem("Iso/Top View")]
	static void TopView()
	{
		SceneView.lastActiveSceneView.rotation = Quaternion.Euler(90f, 0f, 0f);
		SceneView.lastActiveSceneView.pivot = new Vector3(0f, 0f, 0f);
		SceneView.lastActiveSceneView.size = 10f; // unit
		SceneView.lastActiveSceneView.orthographic = true; // or false
	}
	[MenuItem("Iso/Iso View")]
	static void IsoView()
	{
		SceneView.lastActiveSceneView.rotation = Quaternion.Euler(30f, 45f, 0f);
		SceneView.lastActiveSceneView.pivot = new Vector3(0f, 0f, 0f);
		SceneView.lastActiveSceneView.size = 10f; // unit
		SceneView.lastActiveSceneView.orthographic = true; // or false
	}

	void SceneGUI(SceneView sceneView)
	{
		if (!_edithinOn)
			return;

		// This will have scene events including mouse down on scenes objects
		Event cur = Event.current;
		if(cur != null && cur.type == EventType.mouseMove && Camera.current && Camera.current == sceneView.camera)
		{
			Vector3 newPos = GridUtils.MousePosToGridPos(cur.mousePosition, _cursorPos.y, true);
			if(_cursorPos != newPos)
			{
				_cursorPos = newPos;
				if(_underMouseCube)
				{
					_underMouseCube.gameObject.SetActive(true);
				}
				_underMouseCube = _grid.GetFromCell(_cursorPos) as GridObject;
				if(_underMouseCube)
				{
					_underMouseCube.gameObject.SetActive(false);
				}
				if(_currentCube)
				{
					_currentCube.Pos = _cursorPos;
				}
				Repaint();
			}
		}

		if(cur != null && ((cur.type == EventType.MouseDown && cur.button == 0)||(cur.type == EventType.keyDown && cur.keyCode == KeyCode.A)))
		{
			if(_underMouseCube)
			{
				_grid.RemoveObject(_underMouseCube.GridPos, _underMouseCube);
				DestroyImmediate(_underMouseCube.gameObject);//?
			}
			if(_currentCube)
			{
				_underMouseCube = AddNewCubeToGrid(_currentCube);
			}
		}
	}
}
