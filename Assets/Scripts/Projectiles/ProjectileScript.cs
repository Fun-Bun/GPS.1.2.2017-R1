using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	public Weapon.WeaponType type;
	public float speed;

	void Start()
	{
		GetComponent<Rigidbody2D>().AddForce((Vector2)transform.right * -speed, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(!other.isTrigger && !other.GetComponent<PlayerManager>())
		{
			Instantiate(StorageManagerScript.Instance.weapons.settings[(int)type].hitFX, transform.position, transform.rotation);

			Destroy(gameObject);

			if(other.GetComponent<EnemyManager>())
			{
				EnemyManager enemy = other.GetComponent<EnemyManager>();

				enemy.status.health.Reduce(1);

				Destroy(gameObject);
			}
		}
	}

}
