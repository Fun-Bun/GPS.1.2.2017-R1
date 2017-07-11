using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
	public string startGameScene;
	public GameObject[] menuWindows;

	public void StartGame()
	{
		SceneManager.LoadScene(startGameScene);
	}

	public void OpenMenu(int menu)
	{
		menuWindows[menu].SetActive(true);
	}

	public void CloseMenu(int menu)
	{
		menuWindows[menu].SetActive(false);
	}

	public void SetupBGM(GameObject slider){}

	public void SetupSFX(GameObject slider){}

	public void ChangeBGM(GameObject slider){}

	public void ChangeSFX(GameObject slider){}
}
