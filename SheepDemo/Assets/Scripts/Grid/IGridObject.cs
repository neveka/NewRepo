using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGridObject
{
	IIsoGrid Grid { get; set; }
	Vector3 Pos{ get; set; }
	Vector3 GridPos{ get; }
	void SetViewRotation(Vector3? center, Vector3 axis, float angle);

	void SnapToGrid();
	void SnapToAxis();

	void SetVisible(bool visible);
	void SetActive(bool active);
	bool IsVisible();
	string GetName();
	T GetProperty<T> ();
}

public interface IIsoGrid
{
	void SetToCell(Vector3 pos, IGridObject value);
	IGridObject GetFromCell(Vector3 pos);
	List<IGridObject> GetAllFromCell(Vector3 pos);
	void Clear();
}
