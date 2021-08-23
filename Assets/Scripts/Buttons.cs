using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    // Start is called before the first frame update
    public void _Menu()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene("Intro");
    }
	public void _StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
	}
	public void _QuitGame()
	{
		Application.Quit();
	}
}