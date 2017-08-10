using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneList
{
    public List<string> keyScene;
    public List<string> valueScene;

    public SceneList()
    {
        keyScene = new List<string>();
        valueScene = new List<string>();
    }

    public bool ContainsKey(string key)
    {
        return keyScene.Contains(key);
    }

    public string GetValue(string key)
    {
        if(ContainsKey(key))
        {
            return valueScene[keyScene.IndexOf(key)];
        }
        else
        {
            return null;
        }
    }
}

public class PlayerManager : MonoBehaviour
{
	[Header("System")]
	public new CapsuleCollider2D collider;
	public new Rigidbody2D rigidbody;
	public new SpriteRenderer renderer;
	public Animator animator;

    [Header("Developer")]
	public PlayerControlScript controls;
	public PlayerStatusScript status;
	public PlayerWeaponScript weapon;
	public PlayerUIScript ui;
	public PlayerLandboxScript landbox;
    public PlatformReceiverScript platformReceiver;
    public InventoryScript inventory {get { return GameManagerScript.Instance.playerInventory; }}

    [Header("Respawn")]
    public string respawnScene;
    public string quitScene;

	// Use this for initialization
	void Start ()
    {
		collider = GetComponent<CapsuleCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();

		controls = GetComponent<PlayerControlScript>();
		status = GetComponent<PlayerStatusScript>();
		weapon = GetComponentInChildren<PlayerWeaponScript>();
		ui = GetComponentInChildren<PlayerUIScript>();
		landbox = GetComponentInChildren<PlayerLandboxScript>();
		platformReceiver = GetComponent<PlatformReceiverScript>();

		if (controls    != null)   controls.self    = this;
		if (status      != null)   status.self      = this;
		if (weapon   	!= null)   weapon.self   	= this;
		if (ui		   	!= null)   ui.self   		= this;
		if (landbox     != null)   landbox.self     = this;
	}

	public void EnableControls()
	{
		controls.enabled = true;
	}

	public void DisableControls()
	{
		controls.enabled = false;
		controls.moveSpeedFactor = 0.0f;
		SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_PL_WALKING);
		//Stop roll Sound
		animator.SetFloat("HSpeedAbs", 0f);
		animator.SetBool("Grounded", true);
	}
}
