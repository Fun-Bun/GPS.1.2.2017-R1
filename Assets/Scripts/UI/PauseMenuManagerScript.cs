using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManagerScript : MonoBehaviour
{
	#region Singleton
	private static PauseMenuManagerScript mInstance;

	public static PauseMenuManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject temp = GameObject.FindGameObjectWithTag("PauseMenuManager");

				if(temp == null)
				{
					temp = Instantiate(ManagerControllerScript.Instance.pauseMenuManagerPrefab, Vector3.zero, Quaternion.identity);
				}
				mInstance = temp.GetComponent<PauseMenuManagerScript>();
//				DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}
	#endregion Singleton

	void Awake () 
	{
		if(PauseMenuManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
	}

	public bool paused;
	public GameObject bgmSlider;
	public GameObject sfxSlider;
	public GameObject brightnessSlider;
	public string quitGameScene;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			if(paused)
			{
				Pause();
			}
			else
			{
				Resume();
			}
		}
	}

	public void Pause()
	{
		paused = true;
		GetComponent<MainMenuManagerScript>().OpenMenu(0);
		GetComponent<MainMenuManagerScript>().SetupBGM(bgmSlider);
		GetComponent<MainMenuManagerScript>().SetupSFX(sfxSlider);
		GetComponent<MainMenuManagerScript>().SetupBrightness(brightnessSlider);
		Time.timeScale = 0f;
	}

	public void Resume()
	{
		paused = false;
		GetComponent<MainMenuManagerScript>().CloseMenu(0);
		Time.timeScale = 1f;
	}
}
