using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPlayerScript : MonoBehaviour
{
	public GameObject player;
	public GameObject[] tutorials;

	public void HidePlayer ()
	{
		player.SetActive(false);
		for(int i = 0; i < tutorials.Length; i++)
		{
			tutorials[i].SetActive(false);
        }
        gameObject.SetActive(true);
	}

	public void RevealPlayer ()
	{
		player.SetActive(true);
		for(int i = 0; i < tutorials.Length; i++)
		{
			tutorials[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
