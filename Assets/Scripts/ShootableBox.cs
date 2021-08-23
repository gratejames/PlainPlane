using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ShootableBox : MonoBehaviour {

	//The box's current health point total
	private GameObject hud;
	public bool Died = false;

	[Header("Health Slider")]
	public Image    Health_Foreground;
	public Image    Health_Background;

	public float   Health_Value = 5;
	public float   Health_Max;
	public float   Health_FullWidth;
	public float   Health_StartingPosX;
	public Vector2 Health_StartingPos;

	void Start() {
		Health_Foreground = this.transform.Find("Canvas/HPSliderFg").GetComponent<Image>();
		Health_Background = this.transform.Find("Canvas/HPSliderBg").GetComponent<Image>();
		Health_StartingPos = Health_Foreground.rectTransform.localPosition;
		Health_Max = Health_Value;
		// Health_StartingPosX = Health_StartingPos.x;
		Health_FullWidth = Health_Background.rectTransform.sizeDelta.x;

		hud = GameObject.Find("/HUD");
		hud.GetComponent<HUDManager>().Box = hud.GetComponent<HUDManager>().Box + 1;
		Damage(0);
	}
	public void Damage(float damageAmount)
	{
		Health_Value -= damageAmount;
		// Debug.Log("Hit!");

		Health_Foreground.rectTransform.sizeDelta = new Vector2(map(0, Health_Max, 0, Health_FullWidth, Health_Value), 5);
		Health_Foreground.rectTransform.localPosition = new Vector2((-(map(0, Health_Max, 0, Health_FullWidth, Health_Value))/2), 0) + Health_StartingPos;

		if ((Health_Value <= 0) && !Died)
		{
			//if health has fallen below zero, deactivate it 
			gameObject.SetActive (false);
			Died = true;
			hud.GetComponent<HUDManager>().Box = hud.GetComponent<HUDManager>().Box - 1;
		}
	}
	float map(float a1, float a2, float b1, float b2, float value) {
		var output = ((value - a1)/(a2 - a1)) * (b2 - b1) + b1;
		return(output);
		// Used to channge vales between a range of a1 - a2 to a range of b1 - b2
		// This is so that the time, for example, before the missle reloads can be changed
		// to a value that can be directly applied to the size/position of the slider objects
	}
}