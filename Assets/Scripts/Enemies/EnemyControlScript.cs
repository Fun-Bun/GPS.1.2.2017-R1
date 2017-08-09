using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public abstract class EnemyControlScript : MonoBehaviour
{
	[HideInInspector]
	public EnemyManager self;
	protected PlayerManager player;

	public AIState state;

	public abstract void SetTargetToPlayer();
	public virtual void SetState(AIState newState)
	{
		state = newState;
	}

	public abstract void SetTriggerRange(float newRange);
}
