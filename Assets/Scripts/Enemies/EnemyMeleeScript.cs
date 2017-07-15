using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
	private new SpriteRenderer renderer;
	public bool canHit;

	void Start()
	{
		renderer = GetComponentInParent<SpriteRenderer>();
		canHit = false;
	}

	void Update()
	{
		if(renderer.flipX) transform.localScale = new Vector3(-1f, 1f, 1f);
		else transform.localScale = new Vector3(1f, 1f, 1f);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(canHit && other.GetComponent<PlayerManager>())
		{
			PlayerStatusScript status = other.GetComponent<PlayerManager>().status;
			if(!status.isHit)
			{
				status.health.Reduce(1);
				status.ApplyInvincibility();
			}
		}
	}
}
