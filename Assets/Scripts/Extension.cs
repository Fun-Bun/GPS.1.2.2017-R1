using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extension
{
	public static Vector3 GetMousePosition(bool fromScreenView = false)
	{
		if(fromScreenView)
			return Input.mousePosition;
		else
		{
			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			worldMousePos.z = 0;
			return worldMousePos;
		}
	}
}
