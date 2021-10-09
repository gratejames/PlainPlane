using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class MobileButtons : MonoBehaviour
{
	public bool selected = false;
	public bool toggledState = false;
	public void OnButtonDown()
	{
		selected = true;
		toggledState = !toggledState;
		//Debug.Log("Button was selected");
	}

	public void OnButtonUp()
	{
		selected = false;
		//Debug.Log("Button was deselected");
	}
}
