using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlScript : MonoBehaviour
{
	[HideInInspector]
	public EnemyManager self;
	private PlayerManager player;

	public enum AIState
	{
		Idle = 0,
		Walk,
		Jump,
		Land,
		Drop,
		Attack,
		Death
	}

	[Header("Status")]
	public AIState state;
	public bool inVicinity;
	public bool transforming;
	public bool hasTransformed;

	[Header("Settings")]
	public float triggerRange;
	public float movementBuffer;
	public float idleTimer;
	public float idleDuration;

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
	}

	bool Move(Transform targetTransform)
	{
		if(this.transform.position.x + movementBuffer < targetTransform.transform.position.x)
		{
			//Move Right
			this.transform.Translate(Time.deltaTime * self.status.speed * Vector3.right);
			self.renderer.flipX = true;
		}
		else if(targetTransform.transform.position.x < this.transform.position.x - movementBuffer)
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
	
	// Update is called once per frame
	void Update ()
	{
		if(player != null)
		{
			float distanceToPlayer = Vector2.Distance((Vector2)this.transform.position, (Vector2)player.transform.position);
			inVicinity = distanceToPlayer <= triggerRange;
			if(inVicinity)
			{
				target.SetPosition(player.transform.position);
				if(!hasTransformed) transforming = true;
			}
			
			if(transforming)
			{
				if(self.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
				{
					self.animator.Play("Enemy_TransformLeft");
				}
				else
				{
					transforming = false;
					hasTransformed = true;
					target.SetPosition(player.transform.position);
				}
			}
		}

		switch(state)
		{
			case AIState.Drop:
				if(self.platformReceiver.platform != null && Mathf.Abs(transform.position.x - targetStart.transform.position.x) < movementBuffer)
				{
					state = AIState.Walk;
				}
				else
				{
					Move(targetStart);
				}
				break;

			case AIState.Jump:
				if(Move(targetStart))
				{
					//Reached, jump now!!
					GetComponent<Rigidbody2D>().AddForce(Vector2.up * self.status.jumpHeight, ForceMode2D.Impulse);

					if(hasTransformed) self.animator.Play("Enemy_JumpLeft");
					else self.animator.Play("Enemy_PreWalkLeft");

					state = AIState.Land;
				}
				break;

			case AIState.Land:
				if(Move(targetEnd))
				{
					//Landing successful
					state = AIState.Walk;
				}
				break;

			case AIState.Walk:
				if(self.platformReceiver.platform != null && self.platformReceiver.platform.whoStepOnMe.Contains(target.platformReceiver))
				{
					if(Move(target.transform))
					{
						//Attack
						if(inVicinity)
						{
							state = AIState.Attack;
//							self.melee.hasHit = false;
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
						if(self.platformReceiver.platform.transform.position.y < target.platformReceiver.platform.transform.position.y)
						{
							//Find player's platform
							targetPlatform = target.platformReceiver.platform;

							//Enter Jumping State
							state = AIState.Jump;
						}
						else
						{
							//Find self's platform
							targetPlatform = self.platformReceiver.platform;

							//Enter Dropping State
							state = AIState.Drop;
						}

						float distance1 = Mathf.Abs(targetPlatform.jumpPoints[0].jumpStart.transform.position.x - this.transform.position.x);
						float distance2 = Mathf.Abs(targetPlatform.jumpPoints[1].jumpStart.transform.position.x - this.transform.position.x);

						//Find nearest target
						if(distance1 <= distance2)
						{
							targetStart = targetPlatform.jumpPoints[0].jumpStart;
							targetEnd = targetPlatform.jumpPoints[0].jumpEnd;
						}
						else
						{
							targetStart = targetPlatform.jumpPoints[1].jumpStart;
							targetEnd = targetPlatform.jumpPoints[1].jumpEnd;
						}
					}
					else
					{
						if(Move(target.transform))
						{
							//Attack
							if(inVicinity)
							{
								state = AIState.Attack;
//								self.melee.hasHit = false;
							}
							else
								state = AIState.Idle;
						}
					}
				}
				break;
			case AIState.Attack:
				if(self.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
					self.animator.Play("Enemy_AttackLeft");
				else
					state = AIState.Walk;
				break;

			case AIState.Death:
	            self.animator.Play("Enemy_Death");
	            break;

			case AIState.Idle:
			default:
				if(hasTransformed)
				{
					self.animator.Play("Enemy_IdleLeft");
				}
				else
				{
					self.animator.Play("Enemy_PreIdleLeft");
				}
				if(inVicinity)
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
