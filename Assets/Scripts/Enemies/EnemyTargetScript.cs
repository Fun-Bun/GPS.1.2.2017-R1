using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetScript : MonoBehaviour
{
	[HideInInspector]
	public EnemyManager self;
	public float offsetY;
	public Queue<Transform> queuedTargets;
	public PlatformReceiverScript platformReceiver;

	// Use this for initialization
	void Start ()
	{
		platformReceiver = GetComponent<PlatformReceiverScript>();
	}

	public void SetPosition(Vector3 pos)
	{
		this.transform.position = pos - new Vector3(0, offsetY, 0);
	}
}
