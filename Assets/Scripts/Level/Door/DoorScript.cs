using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
	private Animator animator;

	[Header("Booleans")]
	private bool isTouching;
	public bool isOpened;
	public bool canUse;
	private bool showTutorial;

	[Header("Settings")]
	public bool goToScene;
	public DoorScript otherDoor;
	public string sceneName;
	public TutorialScript tutorial;
	
	private Vector3 camPos;
	private Transform playerTrans;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
		camPos = transform.parent.position;
		camPos.z = -10.0f;
		playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		animator.SetBool("IsOpened", isOpened);
		tutorial.gameObject.SetActive(showTutorial);
	}

	void SetOtherDoor(DoorScript door)
	{
		otherDoor = door;
		door.otherDoor = this;
	}

	void Teleport(Transform t)
	{
		if(!goToScene)
		{
			t.position = otherDoor.transform.position;
			Camera.main.transform.position = otherDoor.camPos;
		}
		else
		{
			SceneManager.LoadScene(sceneName);
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(canUse && other.GetComponent<PlayerManager>())
		{
			showTutorial = true;
			PlayerManager player = other.GetComponent<PlayerManager>();
			if(player.controls.interacting)
			{
				Teleport(player.transform);
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

	public void PlayOpenDoorSound()
	{
		if(Vector3.Distance(playerTrans.position, transform.position) <= 6.0f)
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_CD_OPENDOOR);
	}

	public void PlayCloseDoorSound()
	{
		if(Vector3.Distance(playerTrans.position, transform.position) <= 6.0f)
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_CD_CLOSEDOOR);
	}
}
