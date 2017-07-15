using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject enemy;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Q))
			Instantiate(enemy, new Vector3(Random.Range(-4.0f, 4.0f), 2.0f, 0.0f), Quaternion.identity);
	}
}
