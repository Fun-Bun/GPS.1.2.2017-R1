using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictorySceneScript : MonoBehaviour
{
    public GameObject TBC;
    public GameObject STS;

	public void Help ()
    {
        STS.SetActive(true);
	}

	public void Escape ()
    {
        TBC.SetActive(true);
	}

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
