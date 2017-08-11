using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.S))
		{
			transform.position = Extension.GetMousePosition();
            GetComponent<PlayerManager>().inventory.money = 99999;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
