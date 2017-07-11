using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusScript : MonoBehaviour
{
	[HideInInspector]
	public EnemyManager self;

	public Resource health;
	public float speed;
	public float jumpHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(health.value <= 0)
		{
			self.controls.state = EnemyControlScript.AIState.Death;
		}
	}
}
