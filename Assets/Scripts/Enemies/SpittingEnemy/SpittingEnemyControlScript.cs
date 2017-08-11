using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpittingEnemyControlScript : EnemyControlScript
{
    [Header("Status")]
    public bool inArcShotVicinity;

    [Header("Settings")]
    public float arcShotTriggerRange;
    public Transform shotPoint;
    public float arcProjectileCD;
    public float arcTimer;
    public float buffer;

	[Header("Prefabs")]
	public GameObject targetPrefab;

    [Header("Arc projectile settings")]
	public EnemyTargetScript target;
    public float timeTillHit = 1f;
    public GameObject arcProjectile;

    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
		target = Instantiate(targetPrefab, this.transform.position, Quaternion.identity).GetComponent<EnemyTargetScript>();
		target.self = this.self;
		target.SetPosition(transform.position);
		arcTimer = arcProjectileCD;
	}

	public override void SetTargetToPlayer()
	{
		target.SetPosition(player.transform.position);
	}

	public override void SetTriggerRange(float newRange)
	{
		arcShotTriggerRange = newRange;
	}

	public void Shoot()
	{
		SetTargetToPlayer();

		float xDistance = target.transform.position.x - shotPoint.position.x;
		float yDistance = target.transform.position.y - shotPoint.position.y;
		float throwAngle = Mathf.Atan((yDistance + 4.905f) / xDistance);

		float totalVelo = xDistance / Mathf.Cos(throwAngle);

		float xVelo = totalVelo * Mathf.Cos (throwAngle);
		float yVelo = totalVelo * Mathf.Sin (throwAngle);

		GameObject bulletInstance = Instantiate(arcProjectile, shotPoint.position, Quaternion.identity);
		bulletInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2 (xVelo, yVelo), ForceMode2D.Impulse);

		arcTimer = arcProjectileCD;
		state = AIState.Idle;
	}

    void Update()
    {
        arcTimer -= Time.deltaTime;

        if(PauseMenuManagerScript.Instance.paused) return;

        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance((Vector2)this.transform.position, (Vector2)player.transform.position);
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

        switch(state)
		{
			case AIState.Attack:
				if(arcTimer <= 0)
				{
					self.animator.SetBool("IsShooting", true);
				}
				break;

			case AIState.Death:
                self.animator.SetBool("IsDead", true);
                Destroy(gameObject, 1.0f);
               
                break;
            
			case AIState.Idle:
            default:
				self.animator.SetBool("IsShooting", false);

                if(inArcShotVicinity)
                {
					state = AIState.Attack;
                }
                break;
        }
    }
}
