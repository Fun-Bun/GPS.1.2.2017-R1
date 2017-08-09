using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SoundEffects : MonoBehaviour {
	//! Sound Effects from Animation Event.

	public void jumpingSound()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_PL_JUMPING);
	}

	public void walkingSound()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_PL_WALKING);
	}
}