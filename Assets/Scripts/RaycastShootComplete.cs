using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RaycastShootComplete : MonoBehaviour {

	[Header("Gun")]
	private bool gunOverheated = false;
	public float gunOverheatSubtractor = -0.5f;								// Is the gun overheated
	public float gunOverheatAdder = 1f;										// How slowly does the overheat fall (should be negative)
	public float gunOverheatMax = 10f;										// How fast it rises (Should be greater than above)
	public float gunDamage = 1f;											// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public float gunFireRate = 0.25f;										// Number in seconds which controls how often the player can fire
	private float gunNextFire;												// Float to store the time the player will be allowed to fire again, after firing
	public float gunOverheat;												// Float to store the time the player will be allowed to fire again, after firing
	public float gunForce = 500f;										 	// Amount of force which will be added to objects with a rigidbody shot by the player
	public Transform Gun;													// Holds a reference to the gun end object, marking the muzzle location of the gun

	[Header("Missle Setting")]
	public int missleDamage = 4;											// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public float missleFireRate = 0.75f;									// Number in seconds which controls how often the player can fire
	public float missleNextFire;											// Float to store the time the player will be allowed to fire again, after firing
	public float missleForce = 500f;									 	// Amount of force which will be added to objects with a rigidbody shot by the player
	public Transform Missle;												// Holds a reference to the gun end object, marking the muzzle location of the gun
	public MissleLeft MissleLeft;
	public MissleRight MissleRight;


	[Header("Rocket Setting")]
	public int rocketDamage = 4;											// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public int rocketReloadRequirments = 6;
	public int rocketProgress;
	public float rocketForce = 500f;									 	// Amount of force which will be added to objects with a rigidbody shot by the player
	public Transform Rocket;												// Holds a reference to the gun end object, marking the muzzle location of the gun

	[Header("Other")]
	public float weaponRange = 50f;										 	// Distance in Unity units over which the player can fire

	public Camera fpsCam;													// Holds a reference to the first person camera
	private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);		// WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
	private AudioSource gunAudio;											// Reference to the audio source which will play our shooting sound effect
	private LineRenderer laserLine;											// Reference to the LineRenderer component which will display our laserline

	private bool paused;

	private UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl4Axis PlaneControlComponent;

	private bool Mobile;

	private GameObject hud;
	public Transform MobilePanel;

	public MobileButtons ShootBtn;
	public MobileButtons MissleBtn;
	public MobileButtons ReloadBtn;
	public MobileButtons RocketBtn;

	void Start () 
	{
		laserLine = GetComponent<LineRenderer>();
		gunAudio = GetComponent<AudioSource>();
		PlaneControlComponent = GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl4Axis>();
		MissleLeft = GameObject.Find("LeftMissleObj").GetComponent<MissleLeft>();
		MissleRight = GameObject.Find("RightMissleObj").GetComponent<MissleRight>();

		Mobile = GameObject.Find("INFO_OBJECT").GetComponent<INFO_SCRIPT>().MOBILE_CONTROLS_ENABLED;
		hud = GameObject.Find("/HUD");
		MobilePanel = hud.transform.Find("MobilePanel");

		ShootBtn = MobilePanel.transform.Find("ShootButton").GetComponent<MobileButtons>();
		MissleBtn = MobilePanel.transform.Find("MissleButton").GetComponent<MobileButtons>();
		ReloadBtn = MobilePanel.transform.Find("RocketReloadButton").GetComponent<MobileButtons>();
		RocketBtn = MobilePanel.transform.Find("RocketButton").GetComponent<MobileButtons>();
	}


	void FixedUpdate ()
	{
		paused = PlaneControlComponent.dead || PlaneControlComponent.menu;
		if (gunOverheat > 0 && (!paused)) { 
			gunOverheat += gunOverheatSubtractor;
		}
		if (gunOverheat >= gunOverheatMax) {
			gunOverheated = true;
		}
		if (gunOverheated == true && gunOverheat <= 0) {
			gunOverheated = false;
		}
		if ((Input.GetButton("Shoot") || ShootBtn.selected) && (Time.time > gunNextFire) && (gunOverheated == false) && (!paused)) 
		{
			gunOverheat += gunOverheatAdder;
			gunNextFire = Time.time + gunFireRate;
			StartCoroutine (ShotEffect());
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			RaycastHit hit;
			laserLine.SetPosition (0, Gun.position);
			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				laserLine.SetPosition(1, hit.point);

				ShootableBox health1 = hit.collider.GetComponent<ShootableBox>();
				ShootableCar health2 = hit.collider.GetComponent<ShootableCar>();
				ShootableAA  health3 = hit.collider.GetComponent<ShootableAA>();

				if (health1 != null) { health1.Damage(gunDamage); }
				if (health2 != null) { health2.Damage(gunDamage); }
				if (health3 != null) { health3.Damage(gunDamage); }

				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce (-hit.normal * gunForce);
				}
			}
			else {laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));}
		}
		if ((Input.GetButton("Missle") || MissleBtn.selected) && (Time.time > missleNextFire) && (!paused))
		{
			// missleNextFire = Time.time + missleFireRate;

			if (!MissleLeft.Flying) {
				MissleLeft.shoot();
   				missleNextFire = Time.time + missleFireRate;
			} else if (!MissleRight.Flying){
				MissleRight.shoot();
   				missleNextFire = Time.time + missleFireRate;
			}

			// StartCoroutine(ShotEffect());
			// Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			// RaycastHit hit;
			// laserLine.SetPosition (0, Missle.position);
			// if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			// {
			// 	laserLine.SetPosition(1, hit.point);
			// 	ShootableBox health1 = hit.collider.GetComponent<ShootableBox>();
			// 	ShootableCar health2 = hit.collider.GetComponent<ShootableCar>();

			// 	if (health1 != null) { health1.Damage(missleDamage); }
			// 	if (health2 != null) { health2.Damage(missleDamage); }

			// 	if (hit.rigidbody != null)
			// 	{
			// 		hit.rigidbody.AddForce (-hit.normal * missleForce);
			// 	}
			// }
			// else {laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));}
		}
		if ((Input.GetButton("RocketReload") || ReloadBtn.selected) && (Time.time > missleNextFire) && (rocketProgress < rocketReloadRequirments) && (!paused)) {
			rocketProgress += 1;
			missleNextFire = Time.time + missleFireRate;
		}
		if ((Input.GetButton("Rocket") || RocketBtn.selected) && (rocketProgress >= rocketReloadRequirments) && (!paused))
		{
			rocketProgress = 0;
			StartCoroutine(ShotEffect());
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			RaycastHit hit;
			laserLine.SetPosition (0, Rocket.position);
			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				laserLine.SetPosition(1, hit.point);
				ShootableBox health1 = hit.collider.GetComponent<ShootableBox>();
				ShootableCar health2 = hit.collider.GetComponent<ShootableCar>();
				ShootableAA  health3 = hit.collider.GetComponent<ShootableAA>();

				if (health1 != null) { health1.Damage(rocketDamage); }
				if (health2 != null) { health2.Damage(rocketDamage); }
				if (health3 != null) { health3.Damage(rocketDamage); }

				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce (-hit.normal * rocketForce);
				}
			}
			else {laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));}
		}
	}


	private IEnumerator ShotEffect()
	{
		// Play the shooting sound effect
		gunAudio.Play ();

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