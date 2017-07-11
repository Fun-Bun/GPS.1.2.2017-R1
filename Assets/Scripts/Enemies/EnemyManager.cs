using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[Header("System")]
	public new BoxCollider2D collider;
	public new Rigidbody2D rigidbody;
	public new SpriteRenderer renderer;
	public Animator animator;

	[Header("Developer")]
	public EnemyControlScript controls;
	public EnemyStatusScript status;
	public EnemyLandboxScript landbox;
	public PlatformReceiverScript platformReceiver;

	// Use this for initialization
	void Start ()
	{
		collider = GetComponent<BoxCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();

		controls = GetComponent<EnemyControlScript>();
		status = GetComponent<EnemyStatusScript>();
		landbox = GetComponentInChildren<EnemyLandboxScript>();
		platformReceiver = GetComponent<PlatformReceiverScript>();

		if (controls    != null)   controls.self    = this;
//		if (status      != null)   status.self      = this;
		if (landbox     != null)   landbox.self     = this;
	}
}
