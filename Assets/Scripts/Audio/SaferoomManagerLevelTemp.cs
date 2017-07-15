using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaferoomManagerLevelTemp : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		SoundManagerScript.Instance.StopBGM();
		SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_CREEPY);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
