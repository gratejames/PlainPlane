using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityStandardAssets.CrossPlatformInput;
// CROSS_PLATFORM_INPUT

public class HUDManager : MonoBehaviour
{
	public Rigidbody RigidBody;

	[Header("Text")]
	public Text LargeMsg;
	public Text Speed;
	public Text Altitude;
	public Text Throttle;
	public Text TimeText;
	private UnityEngine.UI.Text LargeMsgComponent;
	private UnityEngine.UI.Text SpeedComponent;
	private UnityEngine.UI.Text AltitudeComponent;
	private UnityEngine.UI.Text ThrottleComponent;
	private UnityEngine.UI.Text TimeTextComponent;

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
	public GameObject MobilePanel;

	[Header("Mobile")]
	public GameObject Mobile_Slider;
	//public GameObject Mobile_Top;
	//public MobileButtons Mobile_TopScript;
	//public GameObject Mobile_Bottom;
	//public MobileButtons Mobile_BottomScript;
	private Slider Mobile_SliderComponent;

	[Header("Counter")]
	public Text CarsDisp;
	public Text BoxDisp;
	public int Cars;
	public int Box;
	private UnityEngine.UI.Text CarsDispComponent;
	private UnityEngine.UI.Text BoxDispComponent;

	public GameObject plane;
	private AeroplaneController Plane_Controller;
	private RaycastShootComplete Plane_RaycastShootComplete;

	private int delay;
	private bool Mobile;

	void Start() {
		LargeMsgComponent 			= 		LargeMsg.GetComponent<UnityEngine.UI.Text>();
		SpeedComponent 				= 		Speed   .GetComponent<UnityEngine.UI.Text>();
		AltitudeComponent 			= 		Altitude.GetComponent<UnityEngine.UI.Text>();
		ThrottleComponent 			= 		Throttle.GetComponent<UnityEngine.UI.Text>();
		TimeTextComponent 			= 		TimeText.GetComponent<UnityEngine.UI.Text>();
		CarsDispComponent			=		CarsDisp.GetComponent<UnityEngine.UI.Text>();
		BoxDispComponent			=		BoxDisp .GetComponent<UnityEngine.UI.Text>();

		//Mobile_TopScript 			= 		Mobile_Top.GetComponent<MobileButtons>();
		//Mobile_BottomScript 		= 		Mobile_Bottom.GetComponent<MobileButtons>();
		Mobile_SliderComponent		=		Mobile_Slider.GetComponent<Slider>();

		Plane_Controller 			= 		plane.GetComponent<AeroplaneController>();
		Plane_RaycastShootComplete 	= 		plane.GetComponent<RaycastShootComplete>();

		Mobile = GameObject.Find("INFO_OBJECT").GetComponent<INFO_SCRIPT>().MOBILE_CONTROLS_ENABLED;
		if (Mobile) {
			MobilePanel.SetActive(true);
		} else {
			MobilePanel.SetActive(false);
		}
		LargeMsgComponent.text = ("Pause Menu");
		DeadPanel.gameObject.SetActive(false);
		MenuPanel.gameObject.SetActive(false);

		Missle_StartingPos = Missle_Foreground.rectTransform.localPosition;
		Missle_Threshold = Plane_RaycastShootComplete.missleFireRate;
		Missle_StartingPosX = Missle_StartingPos.x;
		Missle_FullWidth = Missle_Background.rectTransform.sizeDelta.x;
		
		Rocket_StartingPos = Rocket_Foreground.rectTransform.localPosition;
		Rocket_Threshold = Plane_RaycastShootComplete.rocketReloadRequirments;
		Rocket_StartingPosX = Rocket_StartingPos.x;
		Rocket_FullWidth = Rocket_Background.rectTransform.sizeDelta.x;

		Overheat_StartingPos = Overheat_Foreground.rectTransform.localPosition;
		Overheat_Max = Plane_RaycastShootComplete.gunOverheatMax;
		Overheat_StartingPosX = Overheat_StartingPos.x;
		Overheat_FullWidth = Overheat_Background.rectTransform.sizeDelta.x;
	}

	// Update is called once per frame
	void Update()
	{
        if (Mobile)
        {
            Plane_Controller.Throttle = Mobile_SliderComponent.value;
            //	if (Mobile_TopScript.selected) {
            //              Debug.Log("PRESSED");
            //              CrossPlatformInputManager.SetAxis("Pitch", CrossPlatformInputManager.GetAxis("Pitch") + 0.1f);
            //          }
            //          if (Mobile_BottomScript.selected)
            //          {
            //              CrossPlatformInputManager.SetAxis("Pitch", CrossPlatformInputManager.GetAxis("Pitch") - 0.1f);
            //          }
        }

        TimeTextComponent.text = (Mathf.Round(Time.timeSinceLevelLoad*10)/10).ToString();
		SpeedComponent.text = ("Speed: " + Mathf.Round(RigidBody.velocity.magnitude).ToString());
		AltitudeComponent.text = ("Altitude: " + Mathf.Round(Plane_Controller.Altitude).ToString());
		ThrottleComponent.text = ("%: " + Mathf.Round(Plane_Controller.Throttle*100).ToString());

		CarsDispComponent.text = (Cars.ToString());
		BoxDispComponent.text = (Box.ToString());

		if ((Cars + Box) == 0) {
			LargeMsgComponent.text = ("Success!");
		} else {
			LargeMsgComponent.text = ("Pause Menu");
		}
		Missle_Value = Plane_RaycastShootComplete.missleNextFire - Time.time;
		if (Missle_Value > 0) {
			Missle_Foreground.rectTransform.sizeDelta = new Vector2(map(Missle_Value, Missle_Threshold, Missle_FullWidth) + Missle_FullWidth, 20);
			Missle_Foreground.rectTransform.localPosition = new Vector2((-(map(Missle_Value, Missle_Threshold, Missle_FullWidth) + Missle_FullWidth)/2), 0) + Missle_StartingPos;
		} else {
			Missle_Foreground.rectTransform.sizeDelta = new Vector2(Missle_FullWidth, 20);
			Missle_Foreground.rectTransform.localPosition = new Vector2(-Missle_FullWidth/2, 0) + Missle_StartingPos;
		}

		Rocket_Value = Plane_RaycastShootComplete.rocketProgress;
		Rocket_Foreground.rectTransform.sizeDelta = new Vector2(-map(Rocket_Value, Rocket_Threshold, Rocket_FullWidth), 20);
		Rocket_Foreground.rectTransform.localPosition = new Vector2((map(Rocket_Value, Rocket_Threshold, Rocket_FullWidth)/2), 0) + Rocket_StartingPos;


		Overheat_Value = Plane_RaycastShootComplete.gunOverheat;
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
		// Used to channge vales between a range of Thresh - 0 to a range of 0 - Target
		// This is so that the time, for example, before the missle reloads can be changed
		// to a value that can be directly applied to the size/position of the slider objects
	}
	float map2(float a1, float a2, float b1, float b2, float value) {
		var output = ((value - a1)/(a2 - a1)) * (b2 - b1) + b1;
		return(output);
		// Used to channge vales between a range of a1 - a2 to a range of b1 - b2
		// This is so that the time, for example, before the missle reloads can be changed
		// to a value that can be directly applied to the size/position of the slider objects
	}
}