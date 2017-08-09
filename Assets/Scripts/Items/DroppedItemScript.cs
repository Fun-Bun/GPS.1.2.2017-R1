using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DroppedItemScript : MonoBehaviour
{
	private Item.Type prevType = Item.Type.None;
	public Item data;

	void Update()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		if(prevType != data.type)
		{
			SetItemSprite();
			prevType = data.type;
		}
	}

	void SetItemData(Item.Type type, int amount)
	{
		data.type = type;
		data.amount = amount;
		SetItemSprite();
	}

	void SetItemSprite()
	{
		SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
		if(Item.IsAbstractItemType(data.type))
		{
			renderer.sprite = null;
			renderer.color = Color.clear;
		}
		else
		{
			renderer.sprite = data.GetSprite();
			renderer.color = Color.white;
		}
	}
}
