using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class INFO_SCRIPT : MonoBehaviour
{
	public bool MOBILE_CONTROLS_ENABLED = false;
	public Text Disp;
	public bool THEONE;

	// private int t = 0;

	public void UPDATE_CONTROLS_MODE() {
		Disp.GetComponent<UnityEngine.UI.Text>().text = MOBILE_CONTROLS_ENABLED.ToString();
	}

	void Start() {
		Debug.Log(instance.ToString());
		if (!THEONE) {
			Destroy(this.gameObject);
		}
	}

	private static INFO_SCRIPT _instance;

	public static INFO_SCRIPT instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType(typeof (INFO_SCRIPT)) as INFO_SCRIPT;

				_instance.THEONE = true;
				DontDestroyOnLoad(_instance);
			}
			return _instance;
		}
	}
}
