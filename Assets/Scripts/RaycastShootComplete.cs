using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RaycastShootComplete : MonoBehaviour {

	[Header("Gun Missle")]
	private bool gunOverheated = false;
	public float gunOverheatSubtractor = -0.5f;								// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public float gunOverheatAdder = 1f;										// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public float gunOverheatMax = 10f;										// Set the number of hitpoints that this gun will take away from shot objects with a health script
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

	void Start () 
	{
		laserLine = GetComponent<LineRenderer>();
		gunAudio = GetComponent<AudioSource>();
	}


	void FixedUpdate ()
	{
		paused = this.GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl4Axis>().dead || this.GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl4Axis>().menu;
		if (gunOverheat > 0 && (!paused)) { 
			gunOverheat += gunOverheatSubtractor;
		}
		if (gunOverheat >= gunOverheatMax) {
			gunOverheated = true;
		}
		if (gunOverheated == true && gunOverheat <= 0) {
			gunOverheated = false;
		}
		if (Input.GetButton("Shoot") && (Time.time > gunNextFire) && (gunOverheated == false) && (!paused)) 
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

				if (health1 != null) { health1.Damage(gunDamage); }
				if (health2 != null) { health2.Damage(gunDamage); }

				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce (-hit.normal * gunForce);
				}
			}
			else {laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));}
		}
		if (Input.GetButton("Missle") && (Time.time > missleNextFire) && (!paused))
		{
			missleNextFire = Time.time + missleFireRate;
			StartCoroutine(ShotEffect());
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			RaycastHit hit;
			laserLine.SetPosition (0, Missle.position);
			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				laserLine.SetPosition(1, hit.point);
				ShootableBox health1 = hit.collider.GetComponent<ShootableBox>();
				ShootableCar health2 = hit.collider.GetComponent<ShootableCar>();

				if (health1 != null) { health1.Damage(missleDamage); }
				if (health2 != null) { health2.Damage(missleDamage); }

				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce (-hit.normal * missleForce);
				}
			}
			else {laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));}
		}
		if (Input.GetButton("RocketReload") && (Time.time > missleNextFire) && (rocketProgress < rocketReloadRequirments) && (!paused)) {
			rocketProgress += 1;
			missleNextFire = Time.time + missleFireRate;
		}
		if (Input.GetButton("Rocket") && (rocketProgress >= rocketReloadRequirments) && (!paused))
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

				if (health1 != null) { health1.Damage(rocketDamage); }
				if (health2 != null) { health2.Damage(rocketDamage); }

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