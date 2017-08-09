using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteStorage
{
	public Sprite[] playerHealth = new Sprite[3];
	public Sprite[] playerAmmo = new Sprite[2];
	public Sprite[] playerWeapon = new Sprite[2];
}

[System.Serializable]
public class ItemSettings
{
	public string name;
	public int price;
	public string description;
	public Sprite sprite;

	public ItemSettings()
	{
		name = "";
		price = 0;
		description = "";
		sprite = null;
	}
}

[System.Serializable]
public class ItemStorage
{
	public ItemSettings[] settings;
}

[System.Serializable]
public class WeaponSettings
{
	public string name;
	public string description;
	public Sprite sprite;
	[Header("Advanced")]
	public int maxAmmo;
	public int ammoPerShot;
	public int maxTargets;
	public float cooldownDuration;
	public float reloadDuration;
	public float overheatDuration;
	[Header("Projectile")]
	public GameObject projectile;
	public GameObject hitFX;

	public WeaponSettings()
	{
		name = "";
		description = "";
		sprite = null;
		maxAmmo = 6;
		cooldownDuration = 0.0f;
		reloadDuration = 0.0f;
		overheatDuration = 0.0f;
	}
}

[System.Serializable]
public class WeaponStorage
{
	public WeaponSettings[] settings;
}

[System.Serializable]
public class EnemyStorage
{
	public GameObject bloodSplatterFX;
}

public class StorageManagerScript : MonoBehaviour
{
	#region Singleton
	private static StorageManagerScript mInstance;

	public static StorageManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				StorageManagerScript temp = ManagerControllerScript.Instance.storageManager;

				if(temp == null)
				{
					temp = Instantiate(ManagerControllerScript.Instance.storageManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<StorageManagerScript>();
				}
				mInstance = temp;
				ManagerControllerScript.Instance.storageManager = mInstance;
				//DontDestroyOnLoad(mInstance.gameObject); //Editor bug
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}
	#endregion Singleton

	public SpriteStorage sprites;
	public ItemStorage items;
	public WeaponStorage weapons;
	public EnemyStorage enemies;

	void Awake () 
	{
		if(StorageManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
	}
}