using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
	private Animator animator;

	[Header("Booleans")]
	private bool isTouching;
	public bool isActive;
	public bool canUse;
	private bool showTutorial;

	[Header("Settings")]
	public DoorScript door;
	public TutorialScript tutorial;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		animator.SetBool("IsActive", isActive);
		tutorial.gameObject.SetActive(showTutorial);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(canUse && other.GetComponent<PlayerManager>())
		{
			showTutorial = true;
			PlayerManager player = other.GetComponent<PlayerManager>();
			if(player.controls.interacting)
			{
				isActive = !isActive;
				door.isOpened = isActive;
				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_CONTROLP_LACTIVATED);
				player.controls.interacting = false;
			}
		}
		else
		{
			showTutorial = false;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		showTutorial = false;
	}
}
