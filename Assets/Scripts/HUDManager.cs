using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	public Rigidbody RigidBody;

	[Header("Text")]
	public Text Speed;
	public Text Altitude;
	public Text LargeMsg;
	public Text Throttle;
	public Text TimeText;

	[Header("Missle Slider")]
	public Image    Missle_Foreground;
	public Image    Missle_Background;

	private float   Missle_Value;
	private float   Missle_Threshold;
	private float   Missle_FullWidth;
	private float   Missle_StartingPosX;
	private Vector2 Missle_StartingPos;

	[Header("Rocket Slider")]
	public Image    Rocket_Foreground;
	public Image    Rocket_Background;

	private float   Rocket_Value;
	private float   Rocket_Threshold;
	private float   Rocket_FullWidth;
	private float   Rocket_StartingPosX;
	private Vector2 Rocket_StartingPos;

	[Header("Overheat Slider")]
	public Image    Overheat_Foreground;
	public Image    Overheat_Background;

	private float   Overheat_Value;
	private float   Overheat_Max;
	private float   Overheat_FullWidth;
	private float   Overheat_StartingPosX;
	private Vector2 Overheat_StartingPos;

	[Header("Panels")]
	public GameObject DeadPanel;
	public GameObject MenuPanel;

	[Header("Counter")]
	public Text CarsDisp;
	public int cars;
	public Text BoxesDisp;
	public int boxes;

	public GameObject plane;

	private string speedStr;
	private string AltitStr;

	private int delay;

	void Start() {
		LargeMsg.GetComponent<UnityEngine.UI.Text>().text = ("Pause Menu");
		DeadPanel.gameObject.SetActive(false);
		MenuPanel.gameObject.SetActive(false);
		
		Missle_StartingPos = Missle_Foreground.rectTransform.localPosition;
		Missle_Threshold = plane.GetComponent<RaycastShootComplete>().missleFireRate;
		Missle_StartingPosX = Missle_StartingPos.x;
		Missle_FullWidth = Missle_Background.rectTransform.sizeDelta.x;
		
		Rocket_StartingPos = Rocket_Foreground.rectTransform.localPosition;
		Rocket_Threshold = plane.GetComponent<RaycastShootComplete>().rocketReloadRequirments;
		Rocket_StartingPosX = Rocket_StartingPos.x;
		Rocket_FullWidth = Rocket_Background.rectTransform.sizeDelta.x;

		Overheat_StartingPos = Overheat_Foreground.rectTransform.localPosition;
		Overheat_Max = plane.GetComponent<RaycastShootComplete>().gunOverheatMax;
		Overheat_StartingPosX = Overheat_StartingPos.x;
		Overheat_FullWidth = Overheat_Background.rectTransform.sizeDelta.x;
	}

	// Update is called once per frame
	void Update()
	{
		TimeText.GetComponent<UnityEngine.UI.Text>().text = (Mathf.Round(Time.timeSinceLevelLoad*10)/10).ToString();
		Speed.GetComponent<UnityEngine.UI.Text>().text = ("Speed: " + Mathf.Round(RigidBody.velocity.magnitude).ToString());
		Altitude.GetComponent<UnityEngine.UI.Text>().text = ("Altitude: " + Mathf.Round(RigidBody.transform.position.y).ToString());
		Throttle.GetComponent<UnityEngine.UI.Text>().text = ("%: " + Mathf.Round(plane.GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneController>().Throttle*100).ToString());

		CarsDisp.GetComponent<UnityEngine.UI.Text>().text = (cars.ToString());
		BoxesDisp.GetComponent<UnityEngine.UI.Text>().text = (boxes.ToString());

		if ((cars + boxes) == 0) {
			LargeMsg.GetComponent<UnityEngine.UI.Text>().text = ("Success!");
		} else {
			LargeMsg.GetComponent<UnityEngine.UI.Text>().text = ("Pause Menu");
		}
		Missle_Value = plane.GetComponent<RaycastShootComplete>().missleNextFire - Time.time;
		if (Missle_Value > 0) {
			Missle_Foreground.rectTransform.sizeDelta = new Vector2(map(Missle_Value, Missle_Threshold, Missle_FullWidth) + Missle_FullWidth, 20);
			Missle_Foreground.rectTransform.localPosition = new Vector2((-(map(Missle_Value, Missle_Threshold, Missle_FullWidth) + Missle_FullWidth)/2), 0) + Missle_StartingPos;
		} else {
			Missle_Foreground.rectTransform.sizeDelta = new Vector2(Missle_FullWidth, 20);
			Missle_Foreground.rectTransform.localPosition = new Vector2(-Missle_FullWidth/2, 0) + Missle_StartingPos;
		}

		Rocket_Value = plane.GetComponent<RaycastShootComplete>().rocketProgress;
		Rocket_Foreground.rectTransform.sizeDelta = new Vector2(-map(Rocket_Value, Rocket_Threshold, Rocket_FullWidth), 20);
		Rocket_Foreground.rectTransform.localPosition = new Vector2((map(Rocket_Value, Rocket_Threshold, Rocket_FullWidth)/2), 0) + Rocket_StartingPos;


		Overheat_Value = plane.GetComponent<RaycastShootComplete>().gunOverheat;
		if (Overheat_Value <= Overheat_Max) {
			Overheat_Foreground.rectTransform.sizeDelta = new Vector2(-map(Overheat_Value, Overheat_Max, Overheat_FullWidth), 20);
			Overheat_Foreground.rectTransform.localPosition = new Vector2((map(Overheat_Value, Overheat_Max, Overheat_FullWidth)/2), 0) + Overheat_StartingPos;
		} else {
			Overheat_Foreground.rectTransform.sizeDelta = new Vector2(Overheat_FullWidth, 20);
			Overheat_Foreground.rectTransform.localPosition = new Vector2(-Overheat_FullWidth/2, 0) + Overheat_StartingPos;			
		}
	}
	float map(float Value, float Thres, float Target) {
		return -((Value/Thres)*Target);
	}
}