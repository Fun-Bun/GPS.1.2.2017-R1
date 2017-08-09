using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpitShotPointScript : MonoBehaviour
{
	private new SpriteRenderer renderer;

	void Start()
	{
		renderer = GetComponentInParent<SpriteRenderer>();
	}

	void Update()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		if(renderer.flipX) transform.localScale = new Vector3(-1f, 1f, 1f);
		else transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
