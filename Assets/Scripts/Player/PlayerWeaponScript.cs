using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponScript : MonoBehaviour
{
	[HideInInspector]
	public PlayerManager self;

	private new SpriteRenderer renderer;
	private Animator animator;
	public Transform fireSpot;

	public enum WeaponState
	{
		Ready = 0,
		Cooldown,
		Reloading,
		Overheat
	}

	[Header("States")]
	public WeaponState[] state = new WeaponState[(int)Weapon.WeaponType.TotalWeapons];

	[Header("Timers")]
	public float[] cooldownTimer = new float[(int)Weapon.WeaponType.TotalWeapons];
	public float[] reloadTimer = new float[(int)Weapon.WeaponType.TotalWeapons];
	public float[] overheatTimer = new float[(int)Weapon.WeaponType.TotalWeapons];
	public float switchTimer = 0.0f;

	public List<Weapon> weaponList;

	[Header("Settings")]
	public float coneScaleUpwards = 0.75f;
	public float coneScaleDownwards = 0.75f;
	public bool isSwitching;

	public Weapon GetActiveWeapon()
	{
		if(weaponList.Count < 1) return null;
		return weaponList[0];
	}

	public WeaponSettings GetActiveWeaponSettings()
	{
		if(weaponList.Count < 1) return null;
		return StorageManagerScript.Instance.weapons.settings[(int)GetActiveWeapon().type];
	}

	public void CycleWeapon()
	{
		if(weaponList.Count <= 1) return;

		if(weaponList[1].type == Weapon.WeaponType.Laser && state[(int)Weapon.WeaponType.Laser] != WeaponState.Ready)
			return;

		Weapon weapon = weaponList[0];
		weaponList.RemoveAt(0);
		weaponList.Add(weapon);

		isSwitching = true;
	}

	void Start()
	{
		renderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();

		if(weaponList.Count > 0)
		{
			foreach(Weapon w in weaponList)
			{
				if(!Weapon.IsAbstractWeaponType(w.type))
					w.ammo.value = w.ammo.max = StorageManagerScript.Instance.weapons.settings[(int)w.type].maxAmmo;
				else
					w.ammo.value = w.ammo.max = 0;
			}

			for(int i = 0; i < (int)Weapon.WeaponType.TotalWeapons; i++)
			{
				cooldownTimer[i] = 0.0f;
				reloadTimer[i] = 0.0f;
				overheatTimer[i] = 0.0f;
				state[i] = WeaponState.Ready;
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		if(GetActiveWeapon() != null)
		{
			RotateGun();
			
			for(int i = 0; i < (int)Weapon.WeaponType.TotalWeapons; i++)
			{
				switch (state[i])
				{
					case WeaponState.Cooldown:
						cooldownTimer[i] += Time.deltaTime;
						if(cooldownTimer[i] >= StorageManagerScript.Instance.weapons.settings[i].cooldownDuration)
							CooldownDone((Weapon.WeaponType)i);
						break;
					case WeaponState.Reloading:
						reloadTimer[i] += Time.deltaTime;
						if(reloadTimer[i] >= StorageManagerScript.Instance.weapons.settings[i].reloadDuration)
							ReloadDone((Weapon.WeaponType)i);
						break;
					case WeaponState.Overheat:
						overheatTimer[i] += Time.deltaTime;
						if(overheatTimer[i] >= StorageManagerScript.Instance.weapons.settings[i].overheatDuration)
							OverheatDone((Weapon.WeaponType)i);
						break;
				}
			}

			if(isSwitching)
			{
				switchTimer += Time.deltaTime;
				if(switchTimer >= 0.1f)
				{
					isSwitching = false;
					switchTimer = 0.0f;
				}
			}

			self.ui.SetReload(state[(int)GetActiveWeapon().type] == WeaponState.Reloading);
			self.ui.SetSwitch(isSwitching);
		}
	}
	
	void RotateGun()
	{
		Vector3 direction = Extension.GetMousePosition() - this.transform.position;
		direction.Normalize ();
		float angle = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg) - 90.0f;

		// Change angle to [0, 360] range
		angle = Quaternion.Euler(0f, 0f, angle).eulerAngles.z;

		// Make the angle to restrict to only certain percent of full angle
		angle -= 180.0f;
		if(Mathf.Abs(angle) > 0.0f && Mathf.Abs(angle) < 90.0f)
			angle = angle * coneScaleDownwards + (((1.0f - coneScaleDownwards) / 2 * 180) * Mathf.Sign(angle));
		else if(Mathf.Abs(angle) > 90.0f && Mathf.Abs(angle) < 180.0f)
			angle = angle * coneScaleUpwards + (((1.0f - coneScaleUpwards) / 2 * 180) * Mathf.Sign(angle));
		angle += 180.0f;

		transform.rotation = Quaternion.Euler(0f, 0f, angle);

		if(angle > 0 && angle < 180)
		{
			renderer.flipY = false;
			self.renderer.flipX = false;
		}
		else if(angle > 180 && angle < 360)
		{
			renderer.flipY = true;
			self.renderer.flipX = true;
		}
	}

    public void Shoot()
	{
		if(GetActiveWeapon() != null)
		{
			switch(GetActiveWeapon().type)
			{
				case Weapon.WeaponType.Pistol:
					if(state[(int)GetActiveWeapon().type] == WeaponState.Ready)
			        {
						Vector3 direction = Extension.GetMousePosition() - this.transform.position;
						direction.Normalize ();
						float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 180.0f;

						Instantiate(StorageManagerScript.Instance.weapons.settings[(int)GetActiveWeapon().type].projectile, fireSpot.position, Quaternion.Euler(0f, 0f, angle));

						animator.Play("Gun_Default_Shoot", 0, 0.0f);

						SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_GUN_SHOOTINGNORMAL);

						GetActiveWeapon().ammo.Reduce(StorageManagerScript.Instance.weapons.settings[(int)GetActiveWeapon().type].ammoPerShot);

						cooldownTimer[(int)GetActiveWeapon().type] = 0;
						state[(int)GetActiveWeapon().type] = WeaponState.Cooldown;
					}
					break;
				case Weapon.WeaponType.Laser:
					if(state[(int)GetActiveWeapon().type] == WeaponState.Ready /*WeaponState.Reloading*/)
					{
						Vector3 direction = Extension.GetMousePosition() - this.transform.position;
						direction.Normalize ();
						float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 180.0f;

						Instantiate(StorageManagerScript.Instance.weapons.settings[(int)GetActiveWeapon().type].projectile, fireSpot.transform);

						animator.Play("Gun_Default_Shoot", 0, 0.0f);

						SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_GUN_SHOOTINGNORMAL);

						GetActiveWeapon().ammo.Reduce(StorageManagerScript.Instance.weapons.settings[(int)GetActiveWeapon().type].ammoPerShot);

						cooldownTimer[(int)GetActiveWeapon().type] = 0;
						state[(int)GetActiveWeapon().type] = WeaponState.Cooldown;
					}
					break;
			}

			CheckRemainingBullet();
		}
    }

	void CooldownDone(Weapon.WeaponType type)
	{
		cooldownTimer[(int)type] = 0;
		state[(int)type] = WeaponState.Ready;
	}

	public void Reload()
	{
		switch(GetActiveWeapon().type)
		{
			case Weapon.WeaponType.Pistol:
			case Weapon.WeaponType.Laser:
				if(state[(int)GetActiveWeapon().type] != WeaponState.Reloading && cooldownTimer[(int)GetActiveWeapon().type] <= 0.0f)
				{
					reloadTimer[(int)GetActiveWeapon().type] = 0;
					animator.Play("Gun_Default_Reload");

					SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_GUN_RELOAD);

					state[(int)GetActiveWeapon().type] = WeaponState.Reloading;
				}
				break;
			/*
			case Weapon.WeaponType.ParticleCannon:
				break;
			*/
		}

		if(GetActiveWeapon().type == Weapon.WeaponType.Laser)
			CycleWeapon();
	}

	void ReloadDone(Weapon.WeaponType type)
	{
		reloadTimer[(int)type] = 0;

		for(int i = 0; i < weaponList.Count; i++)
		{
			if(weaponList[i].type == type)
				weaponList[i].ammo.value = weaponList[i].ammo.max;
		}

		state[(int)type] = WeaponState.Ready;
	}

	void OverheatDone(Weapon.WeaponType type)
	{
		
	}

	public void CheckRemainingBullet()
	{
		if(GetActiveWeapon() != null)
		{
			if(GetActiveWeapon().ammo.value <= 0)
			{
				GetActiveWeapon().ammo.value = 0;
				Reload();
			}
		}
	}
}
