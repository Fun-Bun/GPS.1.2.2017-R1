using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLoadScript : MonoBehaviour
{
	public string firstScene = "MainMenu";

	void Start()
	{
		SceneManager.LoadScene(firstScene);
	}
}
