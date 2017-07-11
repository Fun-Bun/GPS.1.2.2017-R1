using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
	private new SpriteRenderer renderer;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<PlayerManager>())
		{
			other.GetComponent<PlayerManager>().status.health.Reduce(1);
		}
	}

	void Start()
	{
		renderer = GetComponentInParent<SpriteRenderer>();
	}

	void Update()
	{
		if(renderer.flipX) transform.localScale = new Vector3(-1f, 1f, 1f);
		else transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
