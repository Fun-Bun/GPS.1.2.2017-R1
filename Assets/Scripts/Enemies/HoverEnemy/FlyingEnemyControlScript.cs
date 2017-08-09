using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyControlScript : EnemyControlScript 
{
    [Header("Status")]
    public Transform currentPatrolPoint;
    int currentPatrolIndex;
    public bool goBackToPatrol;

    [Header("Setting")]
    public bool flyingVicinity;
    public float triggerRange;
    public float knockBackForce;
    public Transform[] patrolPoints;
    public float reachedPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
        goBackToPatrol = false;
    }

    public override void SetTargetToPlayer() {}

    public override void SetTriggerRange(float newRange)
    {
        triggerRange = newRange;
    }

    void Update()
    {
        if(PauseMenuManagerScript.Instance.paused) return;

        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance((Vector2)this.transform.position, (Vector2)player.transform.position);
            flyingVicinity = distanceToPlayer <= triggerRange;
        }

        switch(state)
        {
            case AIState.Idle:
                if(Vector3.Distance(transform.position, currentPatrolPoint.position) < reachedPoint)
                {
                    if(currentPatrolIndex + 1 < patrolPoints.Length)
                    {
                        currentPatrolIndex ++;
                    }

                    else currentPatrolIndex = 0;

                    currentPatrolPoint = patrolPoints[currentPatrolIndex];
                    goBackToPatrol = false;
                }

                self.renderer.flipX = transform.position.x < currentPatrolPoint.position.x;
                transform.position = Vector3.MoveTowards(transform.position, currentPatrolPoint.position, self.status.speed * Time.deltaTime);

                if(flyingVicinity && !goBackToPatrol)
                    state = AIState.Attack;
                break;
            case AIState.Attack:
                self.renderer.flipX = transform.position.x < player.transform.position.x;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, self.status.speed * Time.deltaTime);

                if(goBackToPatrol || !flyingVicinity)
                    state = AIState.Idle;
                break;
            case AIState.Death:
                self.animator.SetBool("IsDead", true);
                Destroy(gameObject, 0.5f);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other) // Damage Player On Collision
    {
        if(other.gameObject.GetComponent<PlayerManager>())
        {
            if(!player.status.isHit)
            {
                player.status.health.Reduce(1);
                player.status.ApplyInvincibility();

                Vector3 direction = player.transform.position - transform.position;
                direction.Normalize();

                player.rigidbody.AddForce(direction * knockBackForce, ForceMode2D.Impulse);

                goBackToPatrol = true;
            }
        }
    }

//    void OnDrawGizmosSelected() // Just for fun, allows visualization of the trigger range in Scene.
//    {
//        Gizmos.DrawSphere(transform.position, triggerRange);
//    }
}
