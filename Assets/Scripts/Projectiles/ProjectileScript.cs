using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	public Weapon.WeaponType type;
	public float speed;
	private bool isHit;
	private int enemiesHit;

	void Start()
	{
		isHit = false;
		enemiesHit = 0;
		switch(type)
		{
			case Weapon.WeaponType.Pistol:
				GetComponent<Rigidbody2D>().AddForce((Vector2)transform.right * -speed, ForceMode2D.Impulse);
				break;
			case Weapon.WeaponType.Laser:
				Destroy(gameObject, 1.0f);
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(!isHit && !other.isTrigger && !other.GetComponent<PlayerManager>())
		{
			switch(type)
			{
				case Weapon.WeaponType.Pistol:
					Instantiate(StorageManagerScript.Instance.weapons.settings[(int)type].hitFX, transform.position, transform.rotation);

					if(other.GetComponent<EnemyManager>())
					{
						Instantiate(StorageManagerScript.Instance.enemies.bloodSplatterFX, transform.position, transform.rotation);
						EnemyManager enemy = other.GetComponent<EnemyManager>();

						enemy.status.health.Reduce(1);
						enemy.controls.SetTargetToPlayer();
						enemy.controls.SetTriggerRange(6.0f);

						SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_MS_RECEIVEDMG);
					}

					Destroy(gameObject);
					break;
				case Weapon.WeaponType.Laser:
					if(other.GetComponent<EnemyManager>())
					{
						EnemyManager enemy = other.GetComponent<EnemyManager>();
						
						if(!enemy.status.isHit)
						{
							Instantiate(StorageManagerScript.Instance.enemies.bloodSplatterFX, transform.position, transform.rotation);
							enemy.status.health.Reduce(1);
							enemy.status.isHit = true;
							enemy.controls.SetTargetToPlayer();
							enemy.controls.SetTriggerRange(6.0f);

							SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_MS_RECEIVEDMG);
						}
					}
					break;
			}

			if(++enemiesHit >= StorageManagerScript.Instance.weapons.settings[(int)type].maxTargets)
				isHit = true;

			if(other.GetComponent<MonsterEggScript>())
			{
				Instantiate(StorageManagerScript.Instance.enemies.bloodSplatterFX, transform.position, transform.rotation);
				MonsterEggScript enemy = other.GetComponent<MonsterEggScript>();

				enemy.health.Reduce(1);

//				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_MS_RECEIVEDMG);
			}

			Destroy(gameObject);
		}
	}

}
