using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
	public bool showText = false;
    public GameObject tutorialText;
    public GameObject shopWindow;
    public Button laserBuyButton;

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

        laserBuyButton.interactable = player.inventory.itemInventory[(int)Item.Type.C].amount < 1;
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