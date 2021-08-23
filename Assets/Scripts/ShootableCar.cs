using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShootableCar : MonoBehaviour {
	private Vector3 pos;
	private GameObject hud;
	private int turnAround = 1;
	public bool Died = false;

	private Rigidbody m_rigidbody;
	private HUDManager HUDManager;

	[Header("Health Slider")]
	public Image    Health_Foreground;
	public Image    Health_Background;

	public float   Health_Value = 30;
	public float   Health_Max;
	public float   Health_FullWidth;
	public float   Health_StartingPosX;
	public Vector2 Health_StartingPos;

	void Start() {
		Health_Foreground = this.transform.Find("Canvas/HPSliderFg").GetComponent<Image>();
		Health_Background = this.transform.Find("Canvas/HPSliderBg").GetComponent<Image>();
		Health_StartingPos = Health_Foreground.rectTransform.localPosition;
		Health_Max = Health_Value;
		Health_FullWidth = Health_Background.rectTransform.sizeDelta.x;

		hud = GameObject.Find("/HUD");
		hud.GetComponent<HUDManager>().Cars = hud.GetComponent<HUDManager>().Cars + 1;
		m_rigidbody = this.GetComponent<Rigidbody>();
		HUDManager = hud.GetComponent<HUDManager>();
		Damage(0);
		Debug.Log(Health_Foreground);
	}

	void FixedUpdate() {
		if (Health_Value > 0) 
		{
			pos = this.transform.position;
			if (pos.z >  3900) {
				turnAround = -1;
			}
			if (pos.z < 0) {
				turnAround = 1;
			}
			if (m_rigidbody.velocity.magnitude < 20) {
				// Debug.Log(turnAround);
				m_rigidbody.AddForce(transform.forward * turnAround * 100f);
			}
			pos = this.transform.position;
			// if (pos.y < -10) {
			// 	pos.y = 10;
			// 	this.transform.position = pos;
			// }
		}
	}	

	public void Damage(float damageAmount)
	{
		//subtract damage amount when Damage function is called
		Health_Value -= damageAmount;
		Debug.Log("CAR Hit!");

		Health_Foreground.rectTransform.sizeDelta = new Vector2(map(0, Health_Max, 0, Health_FullWidth, Health_Value), 5);
		Health_Foreground.rectTransform.localPosition = new Vector2((-(map(0, Health_Max, 0, Health_FullWidth, Health_Value))/2), 0) + Health_StartingPos;

		//Check if health has fallen below zero
		if ((Health_Value <= 0) && !Died)
		{
			//if health has fallen below zero, deactivate it 
			gameObject.SetActive (false);
			Died = true;
			HUDManager.Cars = HUDManager.Cars - 1;
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