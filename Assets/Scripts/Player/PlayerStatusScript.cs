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

	void Awake()
	{
		//healthDeplete.resource = health;
	}

	// Update is called once per frame
	void Update ()
	{
		//healthDeplete.Update(Time.deltaTime);
		CheckDeath();
	}

	void CheckDeath()
	{
		if(health.value <= 0)
		{
			Debug.Log("Player is dead.");
			//self.ui.UpdateHealth();
			gameObject.SetActive(false);
			SceneManager.LoadScene("Singleton");
		}
	}
}
