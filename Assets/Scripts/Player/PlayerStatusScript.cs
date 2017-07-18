using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatusScript : MonoBehaviour
{
    [HideInInspector]
    public PlayerManager self;

    [Header("Stats")]
	public Resource health;
	//public Depletable healthDeplete;

    [Header("Movement")]
	public float movementSpeed;
	public float jumpHeight;

	[Header("Combat")]
	public bool isHit;
	public float invincibleTimer;
	public float invincibleDuration;

	[Header("Respawn")]
	public string respawnScene;

	void Awake()
	{
		//healthDeplete.resource = health;
	}

	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;

		CheckDeath();

		if(isHit)
		{
			if(invincibleTimer < invincibleDuration)
			{
				invincibleTimer += Time.deltaTime;
			}
			else
			{
				invincibleTimer = 0;
				isHit = false;
			}
		}
	}

	void CheckDeath()
	{
		if(health.value <= 0)
		{
			Debug.Log("Player is dead.");
			gameObject.SetActive(false);
			SceneManager.LoadScene(respawnScene);
		}
	}

	public void ApplyInvincibility()
	{
		if(!isHit)
		{
			isHit = true;
			invincibleTimer = 0.0f;
		}
	}
}
