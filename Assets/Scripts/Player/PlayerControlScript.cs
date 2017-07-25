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
	public bool interacting;

    [Header("Movement")]
    public bool grounded;
    public bool hasDoubleJumped;

	[Header("Settings")]
	public bool canDoubleJump;
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
	}

	void FixedUpdate()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
		#region Movement

		if (Input.GetAxis(inputHorizontal) != 0f)
		{
			transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * self.status.movementSpeed * Time.deltaTime);
			//self.renderer.flipX = Input.GetAxis("Horizontal") > 0;
		}
		
		#endregion Movement
	}

    void Update()
	{
		if(PauseMenuManagerScript.Instance.paused) return;
        #region Movement

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

				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_PL_JUMPING);
            }
        }

        self.animator.SetFloat("HSpeedAbs", Mathf.Abs(Input.GetAxis("Horizontal") * self.status.movementSpeed));
        self.animator.SetFloat("VSpeed", self.rigidbody.velocity.y);

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

		#region CameraAdjust

		if(canMoveCamera)
		{
			Vector3 posP = transform.position;
			Vector3 posC = Extension.GetMousePosition();
			Vector3 camPos = new Vector3 (posP.x + ((posC.x - posP.x) * visionPercentage), posP.y + ((posC.y - posP.y) * visionPercentage), -10.0f);

			if(Vector3.Distance(Camera.main.transform.position, camPos) > cameraMovementBuffer)
			{
				Camera.main.transform.position += new Vector3((camPos.x - Camera.main.transform.position.x) * (Input.GetAxis("Horizontal") == 0.0f ? 1 : snapSensitivity), camPos.y - Camera.main.transform.position.y, 0.0f) * Time.deltaTime * mouseSensitivity;
			}
		}

		#endregion CameraAdjust
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
