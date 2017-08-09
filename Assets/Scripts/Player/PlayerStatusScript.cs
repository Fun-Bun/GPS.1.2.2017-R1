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

	void Awake()
	{
		//healthDeplete.resource = health;
	}

	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;

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

	public void Respawn()
    {
        Destroy(gameObject);
		SceneManager.LoadScene(self.respawnScene);
	}

	public void Quit()
    {
        Destroy(gameObject);
		SceneManager.LoadScene(self.quitScene);
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
