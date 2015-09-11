using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteChanger : MonoBehaviour 
{
	public List<Sprite> sourceSprites;
	public List<Sprite> targetSprites;
	protected SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Awake () {
		_spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (_spriteRenderer) 
		{
			//Debug.Log(_spriteRenderer.sprite.name);
			for(int i=0; i<sourceSprites.Count; i++)
			{
				if(_spriteRenderer.sprite == sourceSprites[i])
				{
					_spriteRenderer.sprite = targetSprites[i];
				}
			}
		}
	}
}
