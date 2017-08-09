using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterShootEventScript : MonoBehaviour
{
	public SpittingEnemyControlScript controller;

	public void Shoot()
	{
		controller.Shoot();
	}
}
