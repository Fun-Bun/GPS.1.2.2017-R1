using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
	[System.Serializable]
	public enum WeaponType
	{
		None = -1,
		Pistol = 0,
		TotalWeapons
	}

	public WeaponType type;
	public Resource ammo = new Resource();

	public Weapon(WeaponType type)
	{
		this.type = type;
		if(!IsAbstractWeaponType(type))
			this.ammo.value = this.ammo.max = StorageManagerScript.Instance.weapons.settings[(int) type].maxAmmo;
	}

	public string GetName()
	{
		return GetName(type);
	}

	public static string GetName(WeaponType type)
	{
		if(IsAbstractWeaponType(type)) return null;
		return StorageManagerScript.Instance.weapons.settings[(int) type].name;
	}

	public string GetDesc()
	{
		return GetDesc(type);
	}

	public static string GetDesc(WeaponType type)
	{
		if(IsAbstractWeaponType(type)) return null;
		return StorageManagerScript.Instance.weapons.settings[(int) type].description;
	}

	public Sprite GetSprite()
	{
		return GetSprite(type);
	}

	public static Sprite GetSprite(WeaponType type)
	{
		if(IsAbstractWeaponType(type)) return null;
		return StorageManagerScript.Instance.weapons.settings[(int) type].sprite;
	}

	public float GetCooldownDuration()
	{
		return GetCooldownDuration(type);
	}

	public static float GetCooldownDuration(WeaponType type)
	{
		if(IsAbstractWeaponType(type)) return 0.0f;
		return StorageManagerScript.Instance.weapons.settings[(int) type].cooldownDuration;
	}

	public float GetReloadDuration()
	{
		return GetReloadDuration(type);
	}

	public static float GetReloadDuration(WeaponType type)
	{
		if(IsAbstractWeaponType(type)) return 0.0f;
		return StorageManagerScript.Instance.weapons.settings[(int) type].reloadDuration;
	}

	public float GetOverheatDuration()
	{
		return GetOverheatDuration(type);
	}

	public static float GetOverheatDuration(WeaponType type)
	{
		if(IsAbstractWeaponType(type)) return 0.0f;
		return StorageManagerScript.Instance.weapons.settings[(int) type].overheatDuration;
	}

	public static bool IsAbstractWeaponType(WeaponType type)
	{
		if(type == WeaponType.None) return true;
		if((int)type >= (int)WeaponType.TotalWeapons) return true;
		return false;
	}

	public static Item.ItemType ToItem(WeaponType type)
	{
		switch(type)
		{
			case WeaponType.None:
				break;
		}
		return Item.ItemType.None;
	}
}
