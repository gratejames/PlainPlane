using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof (AeroplaneController))]
	public class AeroplaneUserControl4Axis : MonoBehaviour
	{
		// these max angles are only used on mobile, due to the way pitch and roll input are handled
		public float maxRollAngle = 80;
		public float maxPitchAngle = 80;

		// reference to the aeroplane that we're controlling
		private AeroplaneController m_Aeroplane;
		private LandingGear landinggear;
		public float m_Throttle;
		private bool m_AirBrakes;
		private float m_Yaw;

		public bool dead = false;
		public GameObject DeadPanel;

		public bool menu = false;
		public GameObject MenuPanel;
		private bool landingGearDown;

		public bool menuAcnowledged = true;

		[Header("Health Slider")]
		public Image    Health_Foreground;
		public Image    Health_Background;

		public float   Health_Value = 80;
		public float   Health_Regen = 0.1f;
		public float   Health_Max;
		public float   Health_FullWidth;
		public float   Health_StartingPosX;
		public Vector2 Health_StartingPos;

		private bool Mobile;

        private GameObject hud;
		public Transform MobilePanel;

		private HUDManager m_HUDManager;

		public Joystick stick;
		public MobileButtons AirbrakeBtn;
		public MobileButtons PauseBtn;


		private void Awake()
		{
			// Set up the reference to the aeroplane controller.
			m_Aeroplane = GetComponent<AeroplaneController>();
			landinggear = GetComponent<LandingGear>();
			hud = GameObject.Find("/HUD");
			m_HUDManager = hud.GetComponent<HUDManager>();
			Health_Foreground = hud.transform.Find("HPSliderFg").GetComponent<Image>();
			Health_Background = hud.transform.Find("HPSliderBg").GetComponent<Image>();
			Health_StartingPos = Health_Foreground.rectTransform.localPosition;
			Health_Max = Health_Value;
			// Health_StartingPosX = Health_StartingPos.x;
			Health_FullWidth = Health_Background.rectTransform.sizeDelta.x;
			Damage(0);
			Mobile = GameObject.Find("INFO_OBJECT").GetComponent<INFO_SCRIPT>().MOBILE_CONTROLS_ENABLED;
			MobilePanel = hud.transform.Find("MobilePanel");
			stick = MobilePanel.transform.Find("Joystick").GetComponent<Joystick>();
			AirbrakeBtn = MobilePanel.transform.Find("AirbrakeButton").GetComponent<MobileButtons>();
			PauseBtn = MobilePanel.transform.Find("MenuButton").GetComponent<MobileButtons>();
		}

		private void Update() {
			if (Mobile) {
				if (PauseBtn.selected) {
					menu = true;
				}
			} else {
				if (Input.GetButtonDown("Pause"))
				{
					menu = !menu;
				}
			}
			if (menu) {
				Time.timeScale = 0;
				MenuPanel.gameObject.SetActive(true);
			} else {
				Time.timeScale = 1;
				MenuPanel.gameObject.SetActive(false);
			}
			if (Health_Value < Health_Max) {
				Health_Value += Health_Regen;

				Health_Foreground.rectTransform.sizeDelta = new Vector2(map(0, Health_Max, 0, Health_FullWidth, Health_Value), 20);
				Health_Foreground.rectTransform.localPosition = new Vector2((-(map(0, Health_Max, 0, Health_FullWidth, Health_Value))/2), 0) + Health_StartingPos;
			}
		}
		private void FixedUpdate()
		{
			// Read input for the pitch, yaw, roll and throttle of the aeroplane.
			float roll = 0f;
			float pitch = 0f;
            if (Mobile) {
				pitch = stick.vertical;// /2;
				roll = -stick.horizontal;// /2;
				m_AirBrakes = AirbrakeBtn.toggledState;
			}
			else
            {
				roll = CrossPlatformInputManager.GetAxis("Roll");
				pitch = CrossPlatformInputManager.GetAxis("Pitch");
				m_AirBrakes = CrossPlatformInputManager.GetButton("Airbrake");
			}
			//float roll = CrossPlatformInputManager.GetAxis("Mouse X");
			//float pitch = CrossPlatformInputManager.GetAxis("Mouse Y");
			m_Yaw = CrossPlatformInputManager.GetAxis("Yaw");
			m_Throttle = CrossPlatformInputManager.GetAxis("Throttle");

#if MOBILE_INPUT
		AdjustInputForMobileControls(ref roll, ref pitch, ref m_Throttle);
#endif
			// Pass the input to the aeroplane
			if ((!dead) && (!menu)) {
				m_Aeroplane.Move(roll, pitch, m_Yaw, m_Throttle, m_AirBrakes);
			}
		}


		private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
		{
			// because mobile tilt is used for roll and pitch, we help out by
			// assuming that a centered level device means the user
			// wants to fly straight and level!

			// this means on mobile, the input represents the *desired* roll angle of the aeroplane,
			// and the roll input is calculated to achieve that.
			// whereas on non-mobile, the input directly controls the roll of the aeroplane.

			float intendedRollAngle = roll*maxRollAngle*Mathf.Deg2Rad;
			float intendedPitchAngle = pitch*maxPitchAngle*Mathf.Deg2Rad;
			roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
			pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);
		}
		//Detect collisions between the GameObjects with Colliders attached
		void OnCollisionStay(Collision collision)
		{
			//Check for a match with the specific tag on any GameObject that collides with your GameObject
			if (collision.gameObject.name == "Runway") {
				var vel = GetComponent<Rigidbody>().velocity;		//to get a Vector3 representation of the velocity
				var speed = vel.magnitude;							// to get magnitude

				landingGearDown = landinggear.lowered;

				if ((vel.y < 8) && (landingGearDown == true)) {
					//Debug.Log("You have landed!");
				} else {
					Die();
				}
                if ((Mathf.Round(speed) == 0) && (menuAcnowledged == false) && (!dead) && (m_HUDManager.Cars + m_HUDManager.Box) == 0) {
                    menuAcnowledged = true;
                    menu = true;
                }
            } else if (collision.gameObject.tag != "IgnorePlayerCollisions") {
				Die();
				// Debug.Log(collision.gameObject.name);				
			}
		}
		void OnCollisionExit() {
			menuAcnowledged = false;
		}
		public void Resume() {
			menu = false;
		}
		public void Damage(float damageAmount)
		{
			Health_Value -= damageAmount;

			Health_Foreground.rectTransform.sizeDelta = new Vector2(map(0, Health_Max, 0, Health_FullWidth, Health_Value), 20);
			Health_Foreground.rectTransform.localPosition = new Vector2((-(map(0, Health_Max, 0, Health_FullWidth, Health_Value))/2), 0) + Health_StartingPos;

			if ((Health_Value <= 0) && !dead)
			{
				//if health has fallen below zero, deactivate it 
				// gameObject.SetActive(false);
				Die();
			}
		}
		void Die() {
			// Debug.Log("You crashed! :)");
			dead = true;
			Explode();
			DeadPanel.gameObject.SetActive(true);
		}

		void Explode() {
			ParticleSystem exp = GetComponent<ParticleSystem>();
			exp.Play();
			//Destroy(gameObject, exp.main.duration);
		}
		float map(float a1, float a2, float b1, float b2, float value) {
			var output = ((value - a1)/(a2 - a1)) * (b2 - b1) + b1;
			if (output > b2) {
				output = b2;
			}
			return(output);
			// Used to channge vales between a range of a1 - a2 to a range of b1 - b2
			// This is so that the time, for example, before the missle reloads can be changed
			// to a value that can be directly applied to the size/position of the slider objects
		}
	}
}