using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFXScript : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		Animator animator = GetComponent<Animator>();
		Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.1f);
	}
}
