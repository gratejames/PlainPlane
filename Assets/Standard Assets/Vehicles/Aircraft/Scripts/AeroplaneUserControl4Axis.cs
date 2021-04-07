using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof (AeroplaneController))]
	public class AeroplaneUserControl4Axis : MonoBehaviour
	{
		public bool PAUSED_AT_ALL = false;

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

		private void Awake()
		{
			Time.timeScale = 1;
			// Set up the reference to the aeroplane controller.
			m_Aeroplane = GetComponent<AeroplaneController>();
			landinggear = GetComponent<LandingGear>();
		}


		private void FixedUpdate()
		{
			if (PAUSED_AT_ALL) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}
			// Read input for the pitch, yaw, roll and throttle of the aeroplane.
			float roll = CrossPlatformInputManager.GetAxis("Roll");
			float pitch = CrossPlatformInputManager.GetAxis("Pitch");
			//float roll = CrossPlatformInputManager.GetAxis("Mouse X");
			//float pitch = CrossPlatformInputManager.GetAxis("Mouse Y");
			m_AirBrakes = CrossPlatformInputManager.GetButton("Airbrake");
			m_Yaw = CrossPlatformInputManager.GetAxis("Yaw");
			m_Throttle = CrossPlatformInputManager.GetAxis("Throttle");

#if MOBILE_INPUT
		AdjustInputForMobileControls(ref roll, ref pitch, ref m_Throttle);
#endif
			// Pass the input to the aeroplane
			if ((!dead) && (!menu) && !(PAUSED_AT_ALL)) {
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
			//Check for a match with the specified name on any GameObject that collides with your GameObject
			if (collision.gameObject.name != "Runway") { Die(); }

			//Check for a match with the specific tag on any GameObject that collides with your GameObject
			if (collision.gameObject.name == "Runway") {
				var vel = GetComponent<Rigidbody>().velocity;		//to get a Vector3 representation of the velocity
				var speed = vel.magnitude;							// to get magnitude

				landingGearDown = landinggear.lowered;

				if ((speed < 60) && (landingGearDown == true)) {
					//Debug.Log("You have landed!");
				} else {
					Die();
				}
				if ((Mathf.Round(speed) == 0) && (menuAcnowledged == false) && (!dead)) {
					menuAcnowledged = true;
					menu = true;
					PAUSED_AT_ALL = true;
					MenuPanel.gameObject.SetActive(true);
				}
			}
		}
		void OnCollisionExit() {
			menuAcnowledged = false;
		}
		public void Resume() {
			menu = false;
			PAUSED_AT_ALL = false;
			Time.timeScale = 1;
			MenuPanel.gameObject.SetActive(false);
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
	}
}