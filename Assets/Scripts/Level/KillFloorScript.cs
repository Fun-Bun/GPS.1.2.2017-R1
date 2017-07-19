using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloorScript : MonoBehaviour
{
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.GetComponent<PlayerManager>())
		{
			other.GetComponent<PlayerManager>().status.health.value = 0;
		}
		else if(other.GetComponent<EnemyManager>())
		{
			other.GetComponent<EnemyManager>().status.health.value = 0;
		}
	}
}
