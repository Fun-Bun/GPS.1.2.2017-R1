using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFXScript : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		Animator animator = GetComponent<Animator>();
		Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
	}
}
