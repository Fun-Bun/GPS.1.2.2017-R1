using UnityEngine;

[System.Serializable]
public class Item
{
	[System.Serializable]
	public enum ItemType
	{
		None = -1,
		BlackFluid = 0,
		TotalItems
	};

    public ItemType type;
	public int amount;

	public Item(ItemType type, int amount = 0)
	{
		this.type = type;
		this.amount = amount;
	}

	public string GetName()
	{
		return GetName(type);
	}

	public static string GetName(ItemType type)
	{
		if(IsAbstractItemType(type)) return null;
		return StorageManagerScript.Instance.items.settings[(int) type].name;
	}

	public string GetDesc()
	{
		return GetDesc(type);
	}

	public static string GetDesc(ItemType type)
	{
		if(IsAbstractItemType(type)) return null;
		return StorageManagerScript.Instance.items.settings[(int) type].description;
	}

	public Sprite GetSprite()
	{
		return GetSprite(type);
	}

	public static Sprite GetSprite(ItemType type)
	{
		if(IsAbstractItemType(type)) return null;
		return StorageManagerScript.Instance.items.settings[(int) type].sprite;
	}

	public static bool IsAbstractItemType(ItemType type)
	{
		if(type == ItemType.None) return true;
		if((int)type >= (int)ItemType.TotalItems) return true;
		return false;
	}

	public static Weapon.WeaponType ToWeapon(ItemType type)
	{
		switch(type)
		{
			case ItemType.None:
				break;
		}
		return Weapon.WeaponType.None;
	}
}