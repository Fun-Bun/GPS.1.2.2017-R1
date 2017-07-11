using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandboxScript : MonoBehaviour
{
    [HideInInspector]
    public PlayerManager self;

    // Use this for initialization
    void OnTriggerStay2D (Collider2D other)
    {
		self.controls.SetGround(true);
    }

    void OnTriggerExit2D (Collider2D other)
    {
		self.controls.SetGround(false);
    }
}
