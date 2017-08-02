using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour 
{
    public float speed;
    PlayerStatusScript player;
    StationaryShootingEnemyController enemy;
    public int hpReduced;

    public GameObject monsterEgg;
    public Transform spawnPoint;
    private Transform playerTransform; 
    private Vector3 direction;
    private Vector3 playerPosition;

    // Use this for initialization
    void Start () 
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStatusScript>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<StationaryShootingEnemyController>();
        //playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //playerPosition = playerTransform.position;
        //direction = (playerPosition -  transform.position).normalized;
   
        if(player.transform.position.x > enemy.transform.position.x)
        {
             speed = -speed;
        }

        else speed = speed;

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        //transform.position += direction * speed * Time.deltaTime;
        if(PauseMenuManagerScript.Instance.paused)
        {
            return;
        }

        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerManager>())
        {
            PlayerStatusScript status = other.GetComponent<PlayerManager>().status;
            if(!status.isHit)
            {
                status.health.Reduce(1);
                status.ApplyInvincibility();
            }
        }

        else if(other.GetComponent<ArcProjectile>() || other.GetComponent<MonsterEgg>())
        {
            Destroy(gameObject);
        }

        else if(other.GetComponent<ProjectileScript>() || other.GetComponent<EnemyManager>()) return;

        else Instantiate (monsterEgg, spawnPoint.position, spawnPoint.rotation);
        Destroy(gameObject);
    }
}