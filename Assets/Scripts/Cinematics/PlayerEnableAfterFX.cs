using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnableAfterFX : MonoBehaviour
{
	void OnDestroy ()
	{
		GameObject.FindGameObjectWithTag("Player").SetActive(true);
	}
}
