using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyControlScript : EnemyControlScript
{
	[Header("Status")]
	public bool inVicinity;
	public bool transforming;
	public bool hasTransformed;

	[Header("Settings")]
	public float triggerRange;
	public float movementBuffer;
	public float attackBuffer;
	public float idleTimer;
	public float idleDuration;
	public float timer;

	[Header("Prefabs")]
	public GameObject targetPrefab;

	[Header("Target")]
	public EnemyTargetScript target;
	public Transform targetStart;
	public Transform targetEnd;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
		target = Instantiate(targetPrefab, this.transform.position, Quaternion.identity).GetComponent<EnemyTargetScript>();
		target.self = this.self;
		target.SetPosition(transform.position);

		// Random Health
		int randomHP = Random.Range(2, 5);
		self.status.health.max = randomHP;
		self.status.health.value = self.status.health.max;
		float size = (randomHP - 3) * 0.15f;
		Vector3 newScale = transform.localScale;
		newScale.x += size;
		newScale.y += size;
		transform.localScale = newScale; 
	}

	bool Move(Transform targetTransform, float buffer)
	{
		if(this.transform.position.x + buffer < targetTransform.position.x)
		{
			//Move Right
			this.transform.Translate(Time.deltaTime * self.status.speed * Vector3.right);
			self.renderer.flipX = true;
		}
		else if(targetTransform.position.x < this.transform.position.x - buffer)
		{
			//Move Left
			this.transform.Translate(Time.deltaTime * self.status.speed * Vector3.left);
			self.renderer.flipX = false;
		}
		else
		{
			return true;
		}
		return false;
	}

	public override void SetTargetToPlayer()
	{
		if(player.platformReceiver.platform != null)
			target.SetPosition(player.transform.position);
		if(!transforming) transforming = true;
	}

	public override void SetTriggerRange(float newRange)
	{
		triggerRange = newRange;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		self.animator.SetBool("IsMoving", state == AIState.Walk || state == AIState.Jump || state == AIState.Drop);
		self.animator.SetFloat("VSpeed", self.rigidbody.velocity.y);
		self.animator.SetBool("Midair", self.platformReceiver.platform == null);

		if(player != null)
		{
			float distanceToPlayer = Vector2.Distance((Vector2)this.transform.position, (Vector2)player.transform.position);
			inVicinity = distanceToPlayer <= triggerRange;
			if(inVicinity)
			{
				SetTargetToPlayer();
			}
			
			if(transforming && !hasTransformed)
			{
				self.animator.Play("Monster_Transform");

				timer += Time.deltaTime;
				if(timer >= 1.2f)
				{
					hasTransformed = true;
					timer = 0.0f;
					self.status.speed *= 4.0f;
					target.SetPosition(player.transform.position);
				}

				return;
			}
		}

		switch(state)
		{
			case AIState.Drop:
				if(Move(targetStart, movementBuffer))
					state = AIState.Walk;
//				if(self.platformReceiver.platform != null && Mathf.Abs(transform.position.x - targetStart.transform.position.x) < movementBuffer)
//				{
//					state = AIState.Walk;
//				}
//				else
//				{
//					Move(targetStart);
//				}
				break;

			case AIState.Jump:
				if(Move(targetStart, movementBuffer))
				{
					//Reached, jump now!!
					GetComponent<Rigidbody2D>().AddForce(Vector2.up * self.status.jumpHeight, ForceMode2D.Impulse);

					if(hasTransformed)
						self.animator.Play("Monster_Jump");
					else
						self.animator.Play("Monster_PreWalk");

					state = AIState.Land;
				}
				break;

			case AIState.Land:
				if(Move(targetEnd, movementBuffer))
				{
					//Landing successful
					state = AIState.Walk;
				}
				break;

			case AIState.Walk:
				if(self.platformReceiver.platform != null && self.platformReceiver.platform.whoStepOnMe.Contains(target.platformReceiver))
				{
					if(Move(target.transform, attackBuffer))
					{
						//Attack
						if(inVicinity)
						{
							self.renderer.flipX = this.transform.position.x < target.transform.position.x;
							state = AIState.Attack;
							timer = 0.0f;
						}
						else
							state = AIState.Idle;
					}
				}
				else
				{
					if(self.platformReceiver.platform != null && target.platformReceiver.platform != null)
					{
						PlatformScript targetPlatform;
						Transform follow;
						if(self.platformReceiver.platform.transform.position.y - attackBuffer > target.platformReceiver.platform.transform.position.y)
						{
							//Find self's platform
							targetPlatform = self.platformReceiver.platform;

							follow = target.transform;

							//Enter Dropping State
							state = AIState.Drop;
						}
						else
						{
							//Find player's platform
							targetPlatform = target.platformReceiver.platform;

							follow = this.transform;

							//Enter Jumping State
							state = AIState.Jump;
						}

						//! if targetstart is higher than me(targetStart is too far)
						//! needs a way to jump to targetStart first
						//! redirect to targetStart before returning to original path(original target)
						float distance1 = Mathf.Abs(targetPlatform.jumpPoints[0].jumpStart.transform.position.x - follow.position.x);
						float distance2 = Mathf.Abs(targetPlatform.jumpPoints[1].jumpStart.transform.position.x - follow.position.x);

						//Find nearest target
						if(distance1 <= distance2)
						{
							if(targetPlatform.jumpPoints[0].jumpStart.gameObject.activeInHierarchy)
								targetStart = targetPlatform.jumpPoints[0].jumpStart;
							else
								targetStart = targetPlatform.jumpPoints[1].jumpStart;
							
							if(targetPlatform.jumpPoints[0].jumpEnd.gameObject.activeInHierarchy)
								targetEnd = targetPlatform.jumpPoints[0].jumpEnd;
							else
								targetEnd = targetPlatform.jumpPoints[1].jumpEnd;
						}
						else
						{
							if(targetPlatform.jumpPoints[1].jumpStart.gameObject.activeInHierarchy)
								targetStart = targetPlatform.jumpPoints[1].jumpStart;
							else
								targetStart = targetPlatform.jumpPoints[0].jumpStart;

							if(targetPlatform.jumpPoints[1].jumpEnd.gameObject.activeInHierarchy)
								targetEnd = targetPlatform.jumpPoints[1].jumpEnd;
							else
								targetEnd = targetPlatform.jumpPoints[0].jumpEnd;
						}
					}
					else
					{
						if(Move(target.transform, attackBuffer))
						{
							//Attack
							if(inVicinity)
							{
								self.renderer.flipX = this.transform.position.x < target.transform.position.x;
								state = AIState.Attack;
								timer = 0.0f;
							}
							else
								state = AIState.Idle;
						}
					}
				}
				break;
			case AIState.Attack:
				self.animator.Play("Monster_Attack");
				timer += Time.deltaTime;
				if(timer >= 0.7f)
				{
					timer = 0f;
					state = AIState.Walk;
				}
				break;

			case AIState.Death:
                self.animator.Play("Monster_Death");
				Destroy(gameObject, 0.8f);
	            break;

			case AIState.Idle:
			default:
				if(hasTransformed)
					self.animator.Play("Monster_Idle");
				else
					self.animator.Play("Monster_PreIdle");
				
				if(inVicinity && player.platformReceiver.platform != null)
				{
					state = AIState.Walk;
				}
				else
				{
					idleTimer += Time.deltaTime;
					if(idleTimer >= idleDuration)
					{
						idleTimer = 0;
						if(self.platformReceiver.platform != null)
						{
							float randX = Random.Range
							(
								self.platformReceiver.platform.transform.position.x - (self.platformReceiver.platform.halfLength - movementBuffer),
								self.platformReceiver.platform.transform.position.x + (self.platformReceiver.platform.halfLength - movementBuffer)
							);
							target.SetPosition(new Vector3(randX, this.transform.position.y, this.transform.position.z));

							state = AIState.Walk;
						}
					}
				}
				break;
		}
	}
}
