using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootableCar : MonoBehaviour {
	private Vector3 pos;
	public float currentHealth = 3;
	private GameObject hud;
	private int turnAround = 1;

	private Rigidbody m_rigidbody;
	private HUDManager HUDManager;

	void Start() {
		hud = GameObject.Find("/HUD");
		hud.GetComponent<HUDManager>().cars = hud.GetComponent<HUDManager>().cars + 1;
		m_rigidbody = this.GetComponent<Rigidbody>();
		HUDManager = hud.GetComponent<HUDManager>();
	}

	void FixedUpdate() {
		if (currentHealth > 0) 
		{
			pos = this.transform.position;
			if (pos.z >  3900) {
				turnAround = -1;
			}
			if (pos.z < 0) {
				turnAround = 1;
			}
			if (m_rigidbody.velocity.magnitude < 20) {
				m_rigidbody.AddForce(transform.forward * turnAround * 10f);
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
		currentHealth -= damageAmount;
		Debug.Log("Hit! - CAR");

		//Check if health has fallen below zero
		if ((currentHealth <= 0) && (currentHealth > -500))
		{
			//if health has fallen below zero, deactivate it 
			gameObject.SetActive (false);
			currentHealth = -600;
			HUDManager.cars = HUDManager.cars - 1;
		}
	}
}