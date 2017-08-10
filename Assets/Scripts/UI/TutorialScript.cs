using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
	public enum TutorialType
	{
		Move = 0,
		Jump,
		Interact,
		LMB,
		RMB,
		Roll,
		UseItem
	}

	public TutorialType type;
	public Sprite[] spritePreview;

	void OnEnable()
	{
		switch(type)
		{
			case TutorialType.Move:
				GetComponent<Animator>().Play("A&D_ToMove");
				break;
			case TutorialType.Jump:
				GetComponent<Animator>().Play("Space_ToJump");
				break;
			case TutorialType.Interact:
				GetComponent<Animator>().Play("E_ToInteract");
				break;
			case TutorialType.LMB:
				GetComponent<Animator>().Play("LMB_ToShoot");
				break;
			case TutorialType.RMB:
				GetComponent<Animator>().Play("RMB_ToSwitch");
				break;
			case TutorialType.Roll:
				GetComponent<Animator>().Play("DoubleTap_Roll");
				break;
			case TutorialType.UseItem:
				GetComponent<Animator>().Play("Q&W_UseItem");
				break;
		}
	}

	void OnValidate()
	{
		GetComponent<SpriteRenderer>().sprite = spritePreview[(int)type];
	}
}
