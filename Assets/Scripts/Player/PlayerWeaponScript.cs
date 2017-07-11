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

	public WeaponState state;

	public List<Weapon> weaponList;

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

		Weapon weapon = weaponList[0];
		weaponList.RemoveAt(0);
		weaponList.Add(weapon);
	}

    private float cooldownTimer;
	private float reloadTimer;
	private float overheatTimer;

	[Header("Settings")]
	public float coneScaleUpwards = 0.75f;
	public float coneScaleDownwards = 0.75f;

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
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(GetActiveWeapon() != null)
		{
			RotateGun();

			switch (state)
			{
				case WeaponState.Cooldown:
					cooldownTimer += Time.deltaTime;
					if(cooldownTimer >= GetActiveWeaponSettings().cooldownDuration)
						CooldownDone();
					break;
				case WeaponState.Reloading:
					reloadTimer += Time.deltaTime;
					if(reloadTimer >= GetActiveWeaponSettings().reloadDuration)
						ReloadDone();
					break;
				case WeaponState.Overheat:
					overheatTimer += Time.deltaTime;
					if(overheatTimer >= GetActiveWeaponSettings().overheatDuration)
						OverheatDone();
					break;
			}
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
			CheckRemainingBullet();

			if(state == WeaponState.Ready/* || (GetActiveWeapon().type == WeaponType.ParticleCannon && state == WeaponState.Reloading)*/)
	        {
				Vector3 direction = Extension.GetMousePosition() - this.transform.position;
				direction.Normalize ();
				float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 180.0f;

				Instantiate(StorageManagerScript.Instance.weapons.settings[0].projectile, fireSpot.position, Quaternion.Euler(0f, 0f, angle));

				animator.Play("Gun_Default_Shoot", 0, 0.0f);
				GetActiveWeapon().ammo.Reduce(1);

				cooldownTimer = 0;
	            state = WeaponState.Cooldown;
			}
		}
    }

	void CooldownDone()
	{
		cooldownTimer = 0;
		switch(GetActiveWeapon().type)
		{
			case Weapon.WeaponType.Pistol:
				state = WeaponState.Ready;
				break;
			/*
			case Weapon.WeaponType.ParticleCannon:
				if(GetActiveWeapon().ammo.value < GetActiveWeapon().ammo.max) state = WeaponState.Reloading;
				else state = WeaponState.Ready;
				break;
			*/
		}
	}

	public void Reload()
	{
		switch(GetActiveWeapon().type)
		{
			case Weapon.WeaponType.Pistol:
				if(state != WeaponState.Reloading)
				{
					reloadTimer = 0;
					animator.Play("Gun_Default_Reload");
					state = WeaponState.Reloading;
				}
				break;
			/*
			case Weapon.WeaponType.ParticleCannon:
				break;
			*/
		}
	}

	void ReloadDone()
	{
		reloadTimer = 0;
		switch(GetActiveWeapon().type)
		{
			case Weapon.WeaponType.Pistol:
				GetActiveWeapon().ammo.value = GetActiveWeapon().ammo.max;
				state = WeaponState.Ready;
				break;
			/*
			case Weapon.WeaponType.ParticleCannon:
				if(GetActiveWeapon().ammo.value < GetActiveWeapon().ammo.max)
				{
					GetActiveWeapon().ammo.Extend(1);
					state = WeaponState.Reloading;
				}
				else
				{
					state = WeaponState.Ready;
				}
				break;
			*/
		}
	}

	void OverheatDone()
	{
		switch(GetActiveWeapon().type)
		{
			/*
			case Weapon.WeaponType.ParticleCannon:
				overheatTimer = 0;
				state = WeaponState.Reloading;
				break;
			*/
		}
	}

	public void CheckRemainingBullet()
	{
		if(GetActiveWeapon() != null)
		{
			switch(GetActiveWeapon().type)
			{
				case Weapon.WeaponType.Pistol:
					if(GetActiveWeapon().ammo.value <= 0)
					{
						GetActiveWeapon().ammo.value = 0;
						Reload();
					}
					break;
				/*
				case Weapon.WeaponType.ParticleCannon:
					if(GetActiveWeapon().ammo.value <= 0)
					{
						GetActiveWeapon().ammo.value = 0;
	                    overheatTimer = 0;
						state = WeaponState.Overheat;
					}
					break;
				*/
			}
		}
	}
}
