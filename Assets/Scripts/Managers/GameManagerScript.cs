using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
	#region Singleton
	private static GameManagerScript mInstance;

	public static GameManagerScript Instance {get { return mInstance; }}

	void Awake ()
	{
		if (mInstance == null) //Assign this object to this reference
			mInstance = this;
		else if (mInstance != this) //Existed two or more instances, destroy duplicates
			Destroy(this.gameObject);

		DontDestroyOnLoad(this.gameObject);
	}
	#endregion Singleton

	void Start()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
