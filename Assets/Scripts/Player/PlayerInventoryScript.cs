using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
	[HideInInspector]
	public PlayerManager self;

	public List<Item> items;

	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < (int)Item.ItemType.TotalItems; i++)
		{
			items.Add(new Item((Item.ItemType)i, 0));
		}
	}

	public void AddItem(Item.ItemType type, int amount)
	{
		if(Item.IsAbstractItemType(type)) return;

		items[(int)type].amount += amount;
	}

	public void RemoveItem(Item.ItemType type, int amount)
	{
		if(Item.IsAbstractItemType(type)) return;

		items[(int)type].amount -= amount;
		if(items[(int)type].amount < 0) items[(int)type].amount = 0;
	}

	public void DropItem(Item.ItemType type, int amount)
	{
		if(Item.IsAbstractItemType(type)) return;

		RemoveItem(type, amount);

		//Drop Item
	}

	public bool HasEnoughItem(Item.ItemType type, int amount)
	{
		if(Item.IsAbstractItemType(type)) return false;

		return items[(int)type].amount >= amount;
	}
}
