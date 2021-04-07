using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtn : MonoBehaviour
{
	// Start is called before the first frame update
	public void Play()
	{
		Debug.Log("Playing!");
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainSceneV2");
	}
}