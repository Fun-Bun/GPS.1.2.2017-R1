using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTeleport : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.S))
		{
			transform.position = Extension.GetMousePosition();
		}
	}
}
