using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectile : MonoBehaviour {

    [HideInInspector]
    public PlayerManager player;

    public GameObject monsterEgg;
    public Transform spawnPoint;
    public bool inDamageRange;
    public float damageTriggerRange;

    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    void Update()
    {
        if(PauseMenuManagerScript.Instance.paused)
        {
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerStatusScript status = other.gameObject.GetComponent<PlayerManager>().status;
            if(!status.isHit)
            {
                status.health.Reduce(1);
                status.ApplyInvincibility();
            }
        }

        else if(other.gameObject.tag == "EnemyProjectile" || other.gameObject.tag == "MonsterEgg")
        {
            Destroy(gameObject);
        }

        else Instantiate (monsterEgg, spawnPoint.position, spawnPoint.rotation);
        Destroy(gameObject);
    }
}
