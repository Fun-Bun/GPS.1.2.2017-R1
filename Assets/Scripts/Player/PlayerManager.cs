using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[Header("System")]
	public new BoxCollider2D collider;
	public new Rigidbody2D rigidbody;
	public new SpriteRenderer renderer;
	public Animator animator;

    [Header("Developer")]
	public PlayerControlScript controls;
	public PlayerStatusScript status;
	public PlayerInventoryScript inventory;
	public PlayerWeaponScript weapon;
	public PlayerUIScript ui;
	public PlayerLandboxScript landbox;
	public PlatformReceiverScript platformReceiver;

	// Use this for initialization
	void Start ()
    {
        collider = GetComponent<BoxCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();

		controls = GetComponent<PlayerControlScript>();
		status = GetComponent<PlayerStatusScript>();
		inventory = GetComponent<PlayerInventoryScript>();
		weapon = GetComponentInChildren<PlayerWeaponScript>();
		ui = GetComponentInChildren<PlayerUIScript>();
		landbox = GetComponentInChildren<PlayerLandboxScript>();
		platformReceiver = GetComponent<PlatformReceiverScript>();

		if (controls    != null)   controls.self    = this;
		if (status      != null)   status.self      = this;
		if (inventory   != null)   inventory.self   = this;
		if (weapon   	!= null)   weapon.self   	= this;
		if (ui		   	!= null)   ui.self   		= this;
		if (landbox     != null)   landbox.self     = this;
	}
}
