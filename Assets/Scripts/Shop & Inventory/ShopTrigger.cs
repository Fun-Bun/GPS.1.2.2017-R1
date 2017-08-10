using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
	public bool showText = false;
	public GameObject tutorialText;
	public GameObject shopWindow;

	private PlayerManager player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			showText = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			showText = false;
		}
	}

	void Update()
	{
		tutorialText.SetActive (showText && !shopWindow.activeInHierarchy);
		if (player.controls.interacting && showText == true)
		{
			player.controls.interacting = false;
			PopUi();
		}
	}

	public void PopUi ()
	{
		shopWindow.SetActive(true);
		player.DisableControls();
	}

	public void PupUi()
	{
		shopWindow.SetActive(false);
		player.EnableControls();
	}
}