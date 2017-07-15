using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManagerScript : MonoBehaviour
{
	private static PrefabManagerScript mInstance = null;

	public static PrefabManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("PrefabManager");

				if(tempObject == null)
				{
					Debug.LogError("PrefabManagerScript DOES NOT EXITST in the scene!!!");
				}
				else
				{
					mInstance = tempObject.GetComponent<PrefabManagerScript>();
				}
			}
			return mInstance;
		}
	}

	public GameObject soundManagerPrefab;
	public GameObject UIManagerPrefab;
}
