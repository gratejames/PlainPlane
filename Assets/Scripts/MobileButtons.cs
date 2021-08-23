using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class MobileButtons : MonoBehaviour, IPointerDownHandler
{
	public bool selected = false;
	public void OnPointerDown(PointerEventData eventData)
	{
		selected = true;
		Debug.Log("Button was selected");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		selected = false;
		Debug.Log("Button was deselected");
	}
	// void Update(){
	// 	Debug.Log(this.state);
	// }
}
