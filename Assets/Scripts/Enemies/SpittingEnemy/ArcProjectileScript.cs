using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectileScript : MonoBehaviour
{
    public GameObject monsterEgg;
	private Rigidbody2D rigidbody;

    void Start()
    {
		rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.0f);
    }

    void Update()
    {
        if(PauseMenuManagerScript.Instance.paused) return;

		Vector3 direction = (Vector3)rigidbody.velocity;
		direction.Normalize();

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
		transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
		if(other.gameObject.GetComponent<PlayerManager>())
        {
			PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            if(!player.status.isHit)
            {
				player.status.health.Reduce(1);
				player.status.ApplyInvincibility();
            }
        }
		else
		{
			Instantiate (monsterEgg, transform.position, Quaternion.identity);
		}

		Instantiate(StorageManagerScript.Instance.enemies.bloodSplatterFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
