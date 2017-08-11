using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusScript : MonoBehaviour
{
	[HideInInspector]
	public EnemyManager self;

	[Header("Stats")]
	public Resource health;
	public float speed;
	public float jumpHeight;

	[Header("Combat")]
	public bool isHit;
	public float invincibleTimer;
	public float invincibleDuration;

    private bool hasDroppedCurrency = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;

		if(health.value <= 0)
		{
			self.controls.SetState(AIState.Death);
            if(!hasDroppedCurrency) 
            {
                Instantiate(StorageManagerScript.Instance.droppedItem, transform.position, Quaternion.identity);
                hasDroppedCurrency = true;
            }
		}

		if(isHit)
		{
			if(invincibleTimer < invincibleDuration)
			{
				invincibleTimer += Time.deltaTime;
			}
			else
			{
				invincibleTimer = 0;
				isHit = false;
			}
		}
	}
}
