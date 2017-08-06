using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	public Weapon.WeaponType type;
	public float speed;
	private bool isHit;

	void Start()
	{
		GetComponent<Rigidbody2D>().AddForce((Vector2)transform.right * -speed, ForceMode2D.Impulse);
		isHit = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(!isHit && !other.isTrigger && !other.GetComponent<PlayerManager>())
		{
			Instantiate(StorageManagerScript.Instance.weapons.settings[(int)type].hitFX, transform.position, transform.rotation);
			isHit = true;

			if(other.GetComponent<EnemyManager>())
			{
				Instantiate(StorageManagerScript.Instance.enemies.bloodSplatterFX, transform.position, transform.rotation);
				EnemyManager enemy = other.GetComponent<EnemyManager>();

				enemy.status.health.Reduce(1);
				enemy.controls.SetTargetToPlayer();
				enemy.controls.SetTriggerRange(6.0f);

				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_MS_RECEIVEDMG);
			}

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
