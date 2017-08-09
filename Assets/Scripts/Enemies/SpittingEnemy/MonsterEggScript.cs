using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEggScript : MonoBehaviour 
{
	public Resource health;
	public float hatchTimer;
	public float hatchDuration;
    public GameObject enemyPrefab;
	private Animator animator;

	// Use this for initialization
	void Start () 
    {
		animator = GetComponent<Animator>();
		hatchTimer = hatchDuration;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(PauseMenuManagerScript.Instance.paused) return;

        hatchTimer -= Time.deltaTime;

		if(health.value <= 0)
		{
			animator.SetBool("IsDead", true);
			Destroy(gameObject, 0.7f);
		}
		else if(hatchTimer <= 0.0f)
		{
			Instantiate (enemyPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}
        else if(hatchTimer <= 1.0f)
        {
			animator.SetBool("IsHatched", true);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileScript>())
        {
			health.Reduce(1); 
        }
    }
}
