using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
	[Header("Setup")]
	public PlayerManager self;

	[Header("UI Elements")]
	public GameObject healthUI;
	private Image[] healthImages;

	public GameObject weaponUI;
	private Image weaponImage;

	public GameObject ammoUI_bullet;
	private Image[] ammoImages_bullet;

	public GameObject ammoUI_energy;
	private Image[] ammoImages_energy;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Canvas>().worldCamera = Camera.main;
		healthImages = healthUI.GetComponentsInChildren<Image>();
		weaponImage = weaponUI.GetComponentsInChildren<Image>()[1];
		ammoImages_bullet = ammoUI_bullet.GetComponentsInChildren<Image>();
		ammoImages_energy = ammoUI_energy.GetComponentsInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		UpdateHealth();
		UpdateWeapon();
	}

    public void UpdateHealth()
    {
		int partitionMax = StorageManagerScript.Instance.sprites.playerHealth.Length - 1;

		for(int i = 0; i < healthImages.Length; i++)
		{
			int cal = self.status.health.value - (partitionMax * i);
			healthImages[i].sprite = StorageManagerScript.Instance.sprites.playerHealth[(cal >= partitionMax ? partitionMax : cal)];
        }
    }

	void UpdateWeapon()
	{
		weaponUI.SetActive(self.weapon.GetActiveWeapon() != null);
		ammoUI_bullet.SetActive(self.weapon.GetActiveWeapon().type == Weapon.WeaponType.Pistol);
		ammoUI_energy.SetActive(false);

		if(self.weapon.GetActiveWeapon() == null || Weapon.IsAbstractWeaponType(self.weapon.GetActiveWeapon().type))
		{
			weaponImage.sprite = null;
			weaponImage.color = Color.clear;
		}
		else
		{
			weaponImage.sprite = StorageManagerScript.Instance.weapons.settings[(int)self.weapon.GetActiveWeapon().type].sprite;
			weaponImage.color = Color.white;

			switch(self.weapon.GetActiveWeapon().type)
			{
				case Weapon.WeaponType.Pistol:
					int partitionMax = StorageManagerScript.Instance.sprites.playerAmmo.Length - 1;

					for(int i = 0; i < ammoImages_bullet.Length; i++)
					{
						int cal = self.weapon.GetActiveWeapon().ammo.value - (partitionMax * i);
						ammoImages_bullet[i].sprite = StorageManagerScript.Instance.sprites.playerAmmo[(cal >= partitionMax ? partitionMax : 0)];
					}
					break;
			}
		}
	}
}
