using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEgg : MonoBehaviour 
{
    public bool isDestroyed = false;
    public float hatchTimer;
    public float countDownTimer;
    public GameObject enemy;
    public Transform spawnPoint;
    public int healthPoints;

	// Use this for initialization
	void Start () 
    {
        hatchTimer = countDownTimer;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(PauseMenuManagerScript.Instance.paused) return;

        hatchTimer -= Time.deltaTime;

        if(hatchTimer <= 0 && !isDestroyed)
        {
            Instantiate (enemy, spawnPoint.position, spawnPoint.rotation);
            isDestroyed = true;
        }

        if(healthPoints <= 0)
        {
            isDestroyed = true;
        }

        if(isDestroyed) Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileScript>())
        {
            healthPoints--; 
        }
    }
}
