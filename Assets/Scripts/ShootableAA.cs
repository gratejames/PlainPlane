using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class ShootableAA : MonoBehaviour {
	private UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl4Axis PlaneControlComponent;
	private bool paused;

	//The box's current health point total
	private GameObject hud;
	public bool Died = false;
	public GameObject Airplane;
	public GameObject gunObj;

	[Header("Gun")]
	// private bool gunOverheated = false;
	// public float gunOverheatSubtractor = -0.5f;                             // Is the gun overheated
	// public float gunOverheatAdder = 1f;                                     // How slowly does the overheat fall (should be negative)
	// public float gunOverheatMax = 10f;                                      // How fast it rises (Should be greater than above)
	public float gunDamage = 1f;                                            // Set the number of hitpoints that this gun will take away from shot objects with a health script
	public float gunFireRate = 0.25f;                                       // Number in seconds which controls how often the player can fire
	private float gunNextFire;                                              // Float to store the time the player will be allowed to fire again, after firing
	public float gunOverheat;                                               // Float to store the time the player will be allowed to fire again, after firing
	public float gunForce = 500f;                                           // Amount of force which will be added to objects with a rigidbody shot by the player
	public Transform FireFrom;                                              // Holds a reference to the gun end object, marking the muzzle location of the gun
	// public Transform FireTo;                   	                            // Holds a reference to the gun end object, marking the muzzle location of the gun
	public float weaponRange = 10f;                                         // Distance in Unity units over which the player can fire

	private LineRenderer laserLine;                                         // Reference to the LineRenderer component which will display our laserline
	public Camera fpsCam;													// Holds a reference to the first person camera
	private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);		// WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible

	[Header("Health Slider")]
	public Image    Health_Foreground;
	public Image    Health_Background;

	public float   Health_Value = 10;
	public float   Health_Max;
	public float   Health_FullWidth;
	public float   Health_StartingPosX;
	public Vector2 Health_StartingPos;

	void Start() {
		laserLine = GetComponent<LineRenderer>();
		Airplane = GameObject.Find("AircraftJet");
		PlaneControlComponent = Airplane.GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl4Axis>();
		Health_Foreground = this.transform.Find("Canvas/HPSliderFg").GetComponent<Image>();
		Health_Background = this.transform.Find("Canvas/HPSliderBg").GetComponent<Image>();
		gunObj = this.transform.Find("GunScaler/GunPointer").gameObject;

		Health_StartingPos = Health_Foreground.rectTransform.localPosition;
		Health_Max = Health_Value;
		// Health_StartingPosX = Health_StartingPos.x;
		Health_FullWidth = Health_Background.rectTransform.sizeDelta.x;

		hud = GameObject.Find("/HUD");
		hud.GetComponent<HUDManager>().Box = hud.GetComponent<HUDManager>().Box + 1;
		Damage(0);
	}
	void Update() {
		paused = PlaneControlComponent.dead || PlaneControlComponent.menu;
		gunObj.transform.LookAt(Airplane.transform);
		if ((Time.time > gunNextFire) && (!paused)) 
		{
			gunNextFire = Time.time + gunFireRate;
			// Debug.Log("Shooting");
			float dist = Vector3.Distance(Airplane.transform.position, gunObj.transform.position);
			if (dist <= weaponRange) {
				laserLine.SetPosition(0, Airplane.transform.position);
				laserLine.SetPosition(1, gunObj.transform.position);
				StartCoroutine(ShotEffect());
				// Debug.Log("Damaging");
				PlaneControlComponent.Damage(gunDamage);				
			}
			// Debug.Log(dist);
			// Vector3 rayOrigin = FireFrom.position;
			// RaycastHit hit;
			// bool RaycastHitBool = Physics.Raycast(rayOrigin, gunObj.transform.forward, out hit, weaponRange);
			// if (RaycastHitBool)
			// {
			// 	Debug.Log("Cast");
			// 	// laserLine.SetPosition(1, hit.point);
			// 	var name = "none";
			// 	try {
			// 		name = hit.collider.transform.parent.parent.gameObject.name;
			// 	} catch (Exception e) {
			// 		Debug.Log("parent.parent failed? " + e);
			// 	}
			// 	Debug.Log("name: " + name);

			// 	if (name == "AircraftJet") {
			// 		laserLine.SetPosition (0, rayOrigin);
			// 		laserLine.SetPosition(1, hit.point);
			// 		StartCoroutine(ShotEffect());
			// 		Debug.Log("Damaging");
			// 		PlaneControlComponent.Damage(gunDamage);
			// 	}
			// 	name = "none";
			// } else {
			// 	Debug.Log("RaycastHitBool" + RaycastHitBool);
			// }
			// else {laserLine.SetPosition (1, rayOrigin + (gunObj.transform.forward * weaponRange));}
		}
	}
	public void Damage(float damageAmount)
	{
		Health_Value -= damageAmount;
		Debug.Log("AA Hit!");

		Health_Foreground.rectTransform.sizeDelta = new Vector2(map(0, Health_Max, 0, Health_FullWidth, Health_Value), 1);
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

	private IEnumerator ShotEffect()
	{
		// Play the shooting sound effect
		// gunAudio.Play ();

		// Turn on our line renderer
		laserLine.enabled = true;
		//laserLine.enabled = true;

		//Wait for .07 seconds
		yield return shotDuration;

		// Deactivate our line renderer after waiting
		laserLine.enabled = false;
		//laserLine.enabled = false;
	}
}