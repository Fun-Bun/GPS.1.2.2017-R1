using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayerScript : MonoBehaviour
{
	public AudioClipID audioClipID;
	// Use this for initialization
	void Start ()
	{
		SoundManagerScript.Instance.PlayBGM(audioClipID);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
