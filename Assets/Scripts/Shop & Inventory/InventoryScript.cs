using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
	public int money = 0;
	public List<Item> itemInventory = new List<Item>();
	public bool isShop = false;

	void Start ()
	{
		for(int i = 0; i < (int)Item.Type.Total; i++)
		{
			itemInventory.Add(new Item((Item.Type)i, 0));
		}
	}

	public void AddItem(Item.Type itemType, int amount)
	{
		if (itemType == Item.Type.BlackFluid)
		{
			money++;
			return;
		}
		if(Item.IsAbstractItemType(itemType)) return;
		if(isShop) return;

		itemInventory[(int)itemType].amount += amount;
	}

	public void RemoveItem(Item.Type itemType, int amount)
	{
		if(Item.IsAbstractItemType(itemType)) return;
		if(isShop) return;

		if(HasEnoughItems(itemType, amount))
		{
			itemInventory[(int)itemType].amount -= amount;
		}
	}

	public bool HasEnoughItems(Item.Type itemType, int amount)
	{
		if(Item.IsAbstractItemType(itemType)) return false;
		if(isShop) return true;

		return itemInventory [(int)itemType].amount >= amount;
	}

	public void Trade(Item.Type itemType, int amount, InventoryScript toWho)
	{
		int moneyRequired = Item.GetPrice(itemType) * amount;

		if(toWho.isShop || toWho.money >= moneyRequired)
		{
			if(HasEnoughItems(itemType, amount))
			{
				toWho.AddItem(itemType, amount);
				RemoveItem(itemType, amount);

				toWho.money -= moneyRequired;
				money += moneyRequired;
			}
			else
			{
				Debug.Log("Not enough items to sell");
			}
		}
		else
		{
			Debug.Log("Not enough money to buy");
		}
	}
}
