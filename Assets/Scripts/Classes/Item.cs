using UnityEngine;

[System.Serializable]
public class Item
{
	[System.Serializable]
	public enum Type
	{
		None = -1,
		A,
		B,
		C,
		Total,

		BlackFluid
	};

    public Type type;
	public int amount;

	public Item(Type type, int amount = 0)
	{
		this.type = type;
		this.amount = amount;
	}

	public string GetName()
	{
		return GetName(type);
	}

	public static string GetName(Type type)
	{
		if(IsAbstractItemType(type)) return null;
		return StorageManagerScript.Instance.items.settings[(int) type].name;
	}

	public static int GetPrice(Type type)
	{
		return StorageManagerScript.Instance.items.settings[(int) type].price;
	}

	public int GetPrice()
	{
		return GetPrice(type);
	}

	public string GetDesc()
	{
		return GetDesc(type);
	}

	public static string GetDesc(Type type)
	{
		if(IsAbstractItemType(type)) return null;
		return StorageManagerScript.Instance.items.settings[(int) type].description;
	}

	public Sprite GetSprite()
	{
		return GetSprite(type);
	}

	public static Sprite GetSprite(Type type)
	{
		if(IsAbstractItemType(type)) return null;
		return StorageManagerScript.Instance.items.settings[(int) type].sprite;
	}

	public static bool IsAbstractItemType(Type type)
	{
		if(type == Type.None) return true;
		if((int)type >= (int)Type.Total) return true;
		return false;
	}

	public static Weapon.WeaponType ToWeapon(Type type)
	{
		switch(type)
		{
		case Type.None:
				break;
		}
		return Weapon.WeaponType.None;
	}
}