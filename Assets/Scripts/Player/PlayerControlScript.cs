using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    [HideInInspector]
    public PlayerManager self;

    [Header("Input Settings")]
    public string inputHorizontal;
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
    //public Platform platform;

    [Header("Movement")]
    public bool grounded;
    public bool hasDoubleJumped;

    void Start()
    {
        grounded = false;
        hasDoubleJumped = false;
    }

    void Update()
    {
        #region Movement

        if (Input.GetAxis(inputHorizontal) != 0f)
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * self.status.movementSpeed * Time.deltaTime);
            self.renderer.flipX = Input.GetAxis("Horizontal") > 0;
        }

        if (Input.GetButtonDown(inputJump))
        {
            bool canJump = false;

            if (grounded)
            {
                canJump = true;
            }
            else if (!hasDoubleJumped)
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

        self.animator.SetFloat("HSpeedAbs", Mathf.Abs(Input.GetAxis("Horizontal") * self.status.movementSpeed));
        self.animator.SetFloat("VSpeed", self.rigidbody.velocity.y);

        #endregion Movement

        #region Attack

        if(Input.GetButtonDown(inputAttack))
        {

        }

        #endregion Movement
    }

    public void SetGround(bool isGrounded)
    {
        grounded = isGrounded;
        if (grounded) hasDoubleJumped = false;
        self.animator.SetBool("Grounded", grounded);
    }
}
