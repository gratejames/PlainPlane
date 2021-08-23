using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxUpdater : MonoBehaviour
{
	private Toggle MyToggle;
	private INFO_SCRIPT InfoScript;
	private int t;


	// Start is called before the first frame update
	void Start()
	{
		MyToggle = this.GetComponent<Toggle>();
		InfoScript = GameObject.Find("INFO_OBJECT").GetComponent<INFO_SCRIPT>();
		MyToggle.isOn = InfoScript.MOBILE_CONTROLS_ENABLED;
		t=0;
	}

	// Update is called once per frame
	void Update()
	{
		t += 1;
		if (t % 10 == 1) {
			InfoScript.MOBILE_CONTROLS_ENABLED = MyToggle.isOn;
			InfoScript.UPDATE_CONTROLS_MODE();
		}
	}
}