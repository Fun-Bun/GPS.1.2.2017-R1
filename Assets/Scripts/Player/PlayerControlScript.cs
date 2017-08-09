﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    [HideInInspector]
    public PlayerManager self;

	[Header("Input Settings")]
	public string inputHorizontal;
	public string inputLeft;
	public string inputRight;
    public string inputJump;
    public string inputAttack;
    public string inputInteract;
    public string inputReload;
    public string inputSwitchWeapon;
    public string inputUseItem;
    /*
     * Move to UI later
    public string inputSubmit;
    public string inputCancel;
     *
     */

    [Header("Detection")]
	public bool interacting;

    [Header("Movement")]
	public bool grounded;
	public float moveSpeedFactor;
	public bool hasDoubleJumped;
	public bool hasPressedLeft;
	public bool hasPressedRight;
	private float leftTimer = 0.0f;
	private float rightTimer = 0.0f;
	public bool isRolling;
	private float rollTimer = 0.0f;
	public bool rollReady;
	private float rollCooldownTimer = 0.0f;
	public float rollCooldownDuration;

	[Header("Settings")]
	public bool canDoubleJump;
	public bool canRoll;
	public bool canMoveCamera;
	[Tooltip("How far can player see?")]
	public float visionPercentage;
	[Tooltip("How fast the camera follows the mouse?")]
	public float mouseSensitivity;
	[Tooltip("How fast the camera traces back when moving?")]
	public float snapSensitivity;
	[Tooltip("How much threshold buffer for camera's position?")]
	public float cameraMovementBuffer;

    void Start()
    {
        grounded = false;
        hasDoubleJumped = false;
		moveSpeedFactor = 0.0f;
	}

	void FixedUpdate()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		#region Movement

		if(!isRolling)
			moveSpeedFactor = Input.GetAxis(inputHorizontal);
		else
			moveSpeedFactor = 2.0f * Mathf.Sign(Input.GetAxis(inputHorizontal));
		
		if (moveSpeedFactor != 0f)
		{
			transform.Translate(Vector3.right * moveSpeedFactor * self.status.movementSpeed * Time.deltaTime);
			//self.renderer.flipX = moveSpeedFactor > 0;
		}
		
		#endregion Movement
	}

    void Update()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		#region Movement

		if(canRoll)
		{
			if(!isRolling)
			{
				if(rollReady)
				{
					if(grounded && Mathf.Abs(moveSpeedFactor) >= 0.5f)
					{
						if(Input.GetButtonDown(inputLeft) && !self.renderer.flipX)
						{
							if(hasPressedLeft)
							{
								isRolling = true;
								rollReady = false;
								hasPressedLeft = false;
							}
							else
							{
								hasPressedLeft = true;
							}
						}
						else if(Input.GetButtonDown(inputRight) && self.renderer.flipX)
						{
							if(hasPressedRight)
							{
								isRolling = true;
								rollReady = false;
								hasPressedRight = false;
							}
							else
							{
								hasPressedRight = true;
							}
						}

						if(hasPressedLeft)
						{
							leftTimer += Time.deltaTime;
							if(leftTimer >= 1.0f)
							{
								hasPressedLeft = false;
								leftTimer = 0.0f;
							}
						}

						if(hasPressedRight)
						{
							rightTimer += Time.deltaTime;
							if(rightTimer >= 1.0f)
							{
								hasPressedRight = false;
								rightTimer = 0.0f;
							}
						}
					}
				}
				else
				{
					rollCooldownTimer += Time.deltaTime;
					if(rollCooldownTimer >= rollCooldownDuration)
					{
						rollReady = true;
						rollCooldownTimer = 0.0f;
					}
				}
			}
			else
			{
				rollTimer += Time.deltaTime;
				if(rollTimer >= 0.5f)
				{
					isRolling = false;
					rollTimer = 0.0f;
				}
			}
		}

        if (Input.GetButtonDown(inputJump))
        {
            bool canJump = false;

            if (grounded)
            {
                canJump = true;
            }
			else if (canDoubleJump && !hasDoubleJumped)
            {
                hasDoubleJumped = true;
                canJump = true;
            }

            if (canJump)
            {
                self.rigidbody.velocity = Vector2.zero;
                self.rigidbody.AddForce(Vector2.up * self.status.jumpHeight, ForceMode2D.Impulse);
            }
        }

        #endregion Movement

        #region Attack

        if(Input.GetButtonDown(inputAttack))
        {
			self.weapon.Shoot();
        }

        #endregion Attack

		#region Interact

		if(Input.GetButtonDown(inputInteract))
			interacting = true;
		else if(Input.GetButtonUp(inputInteract))
			interacting = false;

		#endregion Interact

		#region Reload

		if(Input.GetButtonDown(inputReload))
			self.weapon.Reload();
		
		#endregion Reload

		#region SwitchWeapon

		if(Input.GetButtonDown(inputSwitchWeapon))
			self.weapon.CycleWeapon();
		
		#endregion SwitchWeapon

		#region CameraAdjust

		if(canMoveCamera)
		{
			Vector3 posP = transform.position;
			Vector3 posC = Extension.GetMousePosition();
			Vector3 camPos = new Vector3 (posP.x + ((posC.x - posP.x) * visionPercentage), posP.y + ((posC.y - posP.y) * visionPercentage), -10.0f);

			if(Vector3.Distance(Camera.main.transform.position, camPos) > cameraMovementBuffer)
			{
				Camera.main.transform.position += new Vector3((camPos.x - Camera.main.transform.position.x) * (moveSpeedFactor == 0.0f ? 1 : snapSensitivity), camPos.y - Camera.main.transform.position.y, 0.0f) * Time.deltaTime * mouseSensitivity;
			}
		}

		#endregion CameraAdjust
		
		self.animator.SetFloat("HSpeedAbs", Mathf.Abs(moveSpeedFactor * self.status.movementSpeed));
		self.animator.SetFloat("VSpeed", self.rigidbody.velocity.y);
		self.animator.SetBool("IsDoubleJumping", hasDoubleJumped);
		self.animator.SetBool("IsFlip", self.renderer.flipX);
		self.animator.SetBool("IsRolling", isRolling);
    }

    public void SetGround(bool isGrounded)
    {
        grounded = isGrounded;
        if (grounded) hasDoubleJumped = false;
        self.animator.SetBool("Grounded", grounded);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		//Touch items
		if(other.GetComponent<DroppedItemScript>())
		{
			DroppedItemScript drop = other.GetComponent<DroppedItemScript>();

			self.inventory.AddItem(drop.data.type, drop.data.amount);
			Destroy(other.gameObject);
		}

		if(interacting)
		{
			//Other interactions
		}
	}
}
