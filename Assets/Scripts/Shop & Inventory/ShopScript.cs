using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
	public InventoryScript myInventory;
	public InventoryScript playerInventory;


	// Use this for initialization
	void Start ()
	{
		myInventory = GetComponent<InventoryScript>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().inventory;
	}

	public void BuyA()
	{
		myInventory.Trade(Item.Type.A, 1, playerInventory);
	}

	public void SellA()
	{
		playerInventory.Trade(Item.Type.A, 1, myInventory);
	}

	public void BuyB()
	{
		myInventory.Trade (Item.Type.B, 1, playerInventory);
	}

	public void SellB()
	{
		playerInventory.Trade(Item.Type.B, 1, myInventory);
	} 

	public void BuyC()
	{
		myInventory.Trade (Item.Type.C, 1, playerInventory);
	}

	public void SellC()
	{
		playerInventory.Trade(Item.Type.C, 1, myInventory);
    }

    public void PlayButtonSound()
    {
        SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_UI_BUTTON);
    }
}

