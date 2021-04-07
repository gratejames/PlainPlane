using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShootableBox : MonoBehaviour {

	//The box's current health point total
	public float currentHealth = 3;
	private GameObject hud;

	void Start() {
		hud = GameObject.Find("/HUD");
		hud.GetComponent<HUDManager>().boxes = hud.GetComponent<HUDManager>().boxes + 1;
	}
	public void Damage(float damageAmount)
	{
		currentHealth -= damageAmount;
		Debug.Log("Hit!");

		if ((currentHealth <= 0) && (currentHealth > -500))
		{
			//if health has fallen below zero, deactivate it 
			gameObject.SetActive (false);
			currentHealth = -600;
			hud.GetComponent<HUDManager>().boxes = hud.GetComponent<HUDManager>().boxes - 1;
		}
	}
}