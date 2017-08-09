using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour 
{
    [HideInInspector]
    public PlayerManager player;

    [Header("Setting")]
    public float moveSpeed;
    public bool flyingVicinity;
    public float triggerRange;
    public float knockBackForce;
    public Transform[] patrolPoints;
    public float reachedPoint;

    [Header("Information")]
    public Transform currentPatrolPoint;

    int currentPatrolIndex;

    private Transform playerTransform; 
    private Vector3 direction;
    private Vector3 playerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerPosition = playerTransform.position;
        direction = (playerPosition - transform.position).normalized;

        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
    }

    void Update()
    {
        if(PauseMenuManagerScript.Instance.paused) return;

        if(Vector3.Distance(transform.position, currentPatrolPoint.position) < reachedPoint)
        {
            if(currentPatrolIndex + 1 < patrolPoints.Length)
            {
                currentPatrolIndex ++;
            }

            else currentPatrolIndex = 0;

            currentPatrolPoint = patrolPoints[currentPatrolIndex];
        }

        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance((Vector2)this.transform.position, (Vector2)player.transform.position);
            flyingVicinity = distanceToPlayer <= triggerRange;
        }

        if(flyingVicinity)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        else transform.position = Vector3.MoveTowards(transform.position, currentPatrolPoint.position, moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other) // Damage Player On Collision
    {
        if(other.gameObject.tag == ("Player"))
        {
            PlayerStatusScript status = other.gameObject.GetComponent<PlayerManager>().status;

            if(!status.isHit)
            {
                status.health.Reduce(1);
                status.ApplyInvincibility();
                //player.rigidbody.AddForce(direction * knockBackForce);
            }
        }
    }

    void OnDrawGizmosSelected() // Just for fun, allows visualization of the trigger range in Scene.
    {
        Gizmos.DrawSphere(transform.position, triggerRange);
    }
}
