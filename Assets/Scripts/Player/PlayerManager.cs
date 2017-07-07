using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("System")]
    public new SpriteRenderer renderer;
    public new BoxCollider2D collider;
    public new Rigidbody2D rigidbody;
    public Animator animator;

    [Header("Developer")]
    public PlayerControlScript controls;
    public PlayerStatusScript status;
    public PlayerLandboxScript landbox;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        controls = GetComponent<PlayerControlScript>();
        status = GetComponent<PlayerStatusScript>();
        landbox = GetComponentInChildren<PlayerLandboxScript>();

        if (controls    != null)   controls.self    = this;
        if (status      != null)   status.self      = this;
        if (landbox     != null)   landbox.self     = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
