﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryShootingEnemyController : MonoBehaviour
{
	[HideInInspector]
	public EnemyManager self;
    private PlayerManager player;

	public enum AIState
    {
        idle = 0,
        avgShoot,
        arcShot,
        death
    }

    [Header("Status")]
	public AIState state;
    public bool inAvgVicinity;
    public bool inArcShotVicinity;

    [Header("Settings")]
    public int healthPoints;
    public float avgTriggerRange;
    public float arcShotTriggerRange;
    public Transform shotPoint;
    public GameObject avgProjectile;
    public float avgProjectileCD;
    public float avgTimer;
    public float arcProjectileCD;
    public float arcTimer;
    public float buffer;

    [Header("Arc projectile settings")]
    public Transform target;
    public float timeTillHit = 1f;
    public GameObject arcProjectile;

    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        avgTimer = avgProjectileCD;
        arcTimer = arcProjectileCD;
    }

    void Update()
    {
        avgTimer -= Time.deltaTime;
        arcTimer -= Time.deltaTime;

        if(PauseMenuManagerScript.Instance.paused) return;

        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance((Vector2)this.transform.position, (Vector2)player.transform.position);
            inAvgVicinity = distanceToPlayer <= avgTriggerRange;
            inArcShotVicinity = distanceToPlayer <= arcShotTriggerRange;
        }

        if(transform.position.x + buffer < player.transform.position.x)
		{
			self.renderer.flipX = true;
        }

        else if(transform.position.x + buffer > player.transform.position.x)
		{
			self.renderer.flipX = false;
        }

        if(healthPoints <= 0)
        {
			state = AIState.death;
        }

        switch(state)
        {
			case AIState.avgShoot:
                if(avgTimer <= 0)
                    {
                        Debug.Log(state + " : Shooting Average shot now!");
                        //Play shooting animation
                        Instantiate(avgProjectile, shotPoint.position, shotPoint.rotation);
                        avgTimer = avgProjectileCD;
						state = AIState.idle;
                    }
                break;

			case AIState.arcShot:
                    if(arcTimer <= 0)
                    {
                        Debug.Log(state + " : Shooting Arc shot now!");
                        //Play shooting animation
                        Throw(); // Instatiate projectile
                        arcTimer = arcProjectileCD;
						state = AIState.idle;
                    }
                break;

			case AIState.death:
               
                //Play Death Animation
                Destroy(gameObject);
               
                break;
            
			case AIState.idle:
            default:
                //Play idle animation;

                if(inAvgVicinity && !inArcShotVicinity)
                {
					state = AIState.avgShoot;
                }

                else if(inArcShotVicinity)
                {
					state = AIState.arcShot;
                }

                Debug.Log(state + " : Currently Idle");
                break;
        }
    }


    void Throw()
    {
            float xDistance;
            float yDistance;
            float throwAngle;

            xDistance = target.position.x - shotPoint.position.x;
            yDistance = target.position.y - shotPoint.position.y;
            throwAngle = Mathf.Atan((yDistance + 4.905f) / xDistance);

            float totalVelo = xDistance / Mathf.Cos(throwAngle);
            float xVelo, yVelo;

            xVelo = totalVelo * Mathf.Cos (throwAngle);
            yVelo = totalVelo * Mathf.Sin (throwAngle);

            GameObject bulletInstance = Instantiate(arcProjectile, shotPoint.position, Quaternion.Euler(new Vector3 (0,0,0))) as GameObject;
            Rigidbody2D rigid;
            rigid = bulletInstance.GetComponent<Rigidbody2D>();

            rigid.velocity = new Vector2 (xVelo, yVelo);

        Debug.Log("Launched arc shot!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileScript>())
        {
            healthPoints --;
        }
    }
}
