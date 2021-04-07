using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
	private void update() {
		if (Input.GetButtonDown("Respawn")) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainSceneV2");
		}
	}
    // Start is called before the first frame update
    public void RespawnFunc()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainSceneV2");
    	// Debug.Log("Respawn");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().ToString()) ;
    }
}
