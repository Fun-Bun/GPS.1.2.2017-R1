﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
	[HideInInspector]
	public PlayerManager self;
	public Animator ammoAnimator;

	[Header("UI Elements")]
	public GameObject healthUI;
	private List<Image> healthImages;

	public GameObject weaponUI;
	private Image weaponImage;

	public Slider ammoUI_slider;
	public Text ammoUI_text;
	public GameObject ammoUI_bullet;
	private List<Image> ammoImages_bullet;

	public GameObject combatRollPanel;
	public GameObject laserPanel;
	public GameObject vaccinePanel;
	public GameObject mpaPanel;
	public Image combatRollIndicator;
	public Image laserIndicator;
	public Image combatRollIcon;
	public Image laserIcon;
	public Text vaccineCountText;
	public Text mpaCountText;
	public Text currencyText;

	public GameObject deadImage;

	// Use this for initialization
	void Start ()
	{
		healthImages = new List<Image>(healthUI.GetComponentsInChildren<Image>());
		healthImages.RemoveAt(0);
		weaponImage = weaponUI.GetComponentsInChildren<Image>()[1];
		ammoImages_bullet = new List<Image>(ammoUI_bullet.GetComponentsInChildren<Image>());
		deadImage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
		if(PauseMenuManagerScript.Instance.paused) return;
		UpdateHealth();
		UpdateWeapon();
		UpdateIndicators();
	}

    public void UpdateHealth()
	{
		if(self.status.health.value <= 0)
		{
			self.rigidbody.simulated = false;
			self.controls.enabled = false;
			self.renderer.enabled = false;
			deadImage.SetActive(true);
			this.enabled = false;
			return;
		}

		int partitionMax = StorageManagerScript.Instance.sprites.playerHealth.Length - 1;

		for(int i = 0; i < healthImages.Count; i++)
		{
			int cal = self.status.health.value - (partitionMax * i);
			int displayCount = (cal >= partitionMax ? partitionMax : (cal < 0 ? 0 : cal));
			healthImages[i].GetComponent<Animator>().SetInteger("HealthCount", displayCount);
//			healthImages[i].sprite = StorageManagerScript.Instance.sprites.playerHealth[displayCount];
//			if(displayCount > 0) healthImages[i].color = Color.white;
//			else healthImages[i].color = Color.clear;
		}
    }

	void UpdateWeapon()
	{
		weaponUI.SetActive(self.weapon.GetActiveWeapon() != null);
		ammoUI_bullet.SetActive(!Weapon.IsAbstractWeaponType(self.weapon.GetActiveWeapon().type));

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
				case Weapon.WeaponType.Laser:
					ammoUI_text.text = ((int)(self.weapon.GetActiveWeapon().ammo.value / StorageManagerScript.Instance.weapons.settings[(int)self.weapon.GetActiveWeapon().type].ammoPerShot)).ToString();
					int partitionMax = StorageManagerScript.Instance.sprites.playerAmmo.Length - 1;

					for(int i = 0; i < ammoImages_bullet.Count; i++)
					{
						int cal = self.weapon.GetActiveWeapon().ammo.value - (partitionMax * i);
						int displayCount = (cal >= partitionMax ? partitionMax : (cal < 0 ? 0 : cal));
						ammoImages_bullet[i].GetComponent<Animator>().SetInteger("AmmoCount", displayCount);
						ammoImages_bullet[i].GetComponent<Animator>().SetBool("IsLaser", self.weapon.GetActiveWeapon().type == Weapon.WeaponType.Laser);
						ammoImages_bullet[i].GetComponent<Animator>().SetBool("IsSwitching", self.weapon.isSwitching);
						ammoImages_bullet[i].GetComponent<Animator>().SetBool("IsEmpowered", self.weapon.empoweredBullet >= self.weapon.GetActiveWeapon().ammo.value - i);
//						ammoImages_bullet[i].sprite = StorageManagerScript.Instance.sprites.playerAmmo[displayCount];
//						if(displayCount > 0) ammoImages_bullet[i].color = Color.white;
//						else ammoImages_bullet[i].color = Color.clear;
					}
					break;
			}
		}
	}

	void UpdateIndicators()
	{
		if(self.controls.rollReady)
		{
			combatRollIndicator.fillAmount = 1.0f;
			combatRollIcon.fillAmount = 1.0f;
		}
		else
		{
			combatRollIndicator.fillAmount = self.controls.rollCooldownTimer / self.controls.rollCooldownDuration;
			combatRollIcon.fillAmount = self.controls.rollCooldownTimer / self.controls.rollCooldownDuration;
		}

		if(self.inventory.HasEnoughItems(Item.Type.C, 1))
		{
			laserPanel.SetActive(true);
			int laserIndex = (int)Weapon.WeaponType.Laser;

			if(self.weapon.state[laserIndex] == PlayerWeaponScript.WeaponState.Ready)
			{
				laserIndicator.fillAmount = 1.0f;
				laserIcon.fillAmount = 1.0f;
			}
			else
			{
				laserIndicator.fillAmount = self.weapon.reloadTimer[laserIndex] / StorageManagerScript.Instance.weapons.settings[laserIndex].reloadDuration;
				laserIcon.fillAmount = self.weapon.reloadTimer[laserIndex] / StorageManagerScript.Instance.weapons.settings[laserIndex].reloadDuration;
			}
		}
		else
		{
			laserPanel.SetActive(false);
		}

		int vacCount = self.inventory.itemInventory[(int)Item.Type.A].amount;
		if(vacCount > 0)
		{
			vaccinePanel.SetActive(true);
			vaccineCountText.text = vacCount.ToString("00");
		}
		else
		{
			vaccinePanel.SetActive(false);
			vaccineCountText.text = "--";
		}

		int mpaCount = self.inventory.itemInventory[(int)Item.Type.B].amount;
		if(mpaCount > 0)
		{
			mpaPanel.SetActive(true);
			mpaCountText.text = mpaCount.ToString("00");
		}
		else
		{
			mpaPanel.SetActive(false);
			mpaCountText.text = "--";
		}

		currencyText.text = self.inventory.money.ToString();
	}

	public void SetReload(bool isReloading)
	{
		ammoAnimator.SetBool("IsReloading", isReloading);
	}

	public void SetSwitch(bool isSwitching)
	{
		ammoAnimator.SetBool("IsSwitching", isSwitching);
		//ammoAnimator.Play("BulletBar_Switching");
	}
}
