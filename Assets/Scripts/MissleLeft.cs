using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleLeft : MonoBehaviour
{
	public bool Flying = false;
	public Rigidbody m_Rigidbody;
	public int m_speed = 4;
	public int m_acceleration = 1;
	public GameObject Airplane;
	public Transform Lock;
	public ParticleSystem Particles;
	private int missleDamage;
	// Update is called once per frame
	private float missleReturn;
	void Start()
	{
		Particles = this.GetComponent<ParticleSystem>();
		Airplane = GameObject.Find("AircraftJet");
		m_Rigidbody = this.GetComponent<Rigidbody>();
		missleDamage = Airplane.GetComponent<RaycastShootComplete>().missleDamage;
		Physics.IgnoreCollision(GameObject.Find("WingRightBox").GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(GameObject.Find("WingLeftBox").GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(GameObject.Find("AileronLeftBox").GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(GameObject.Find("AileronRightBox").GetComponent<Collider>(), GetComponent<Collider>());
		// Physics.IgnoreCollision(GameObject.Find("WheelLeft").GetComponent<Collider>(), GetComponent<Collider>());
		returnToPlane();
	}
	void Update()
	{
		if ((transform.position.y < -5 || transform.position.y > 5000) && Flying) {
			returnToPlane();
		}
		if (Flying) {
			if (Time.time > missleReturn) {
				returnToPlane();
			}
		} else {
			transform.position = Lock.position;
			transform.rotation = Airplane.transform.rotation;
		}
	}
	public void shoot() {
		missleReturn = Time.time + 6;
		Flying = true;
		m_Rigidbody.velocity = Vector3.zero;
		m_Rigidbody.angularVelocity = Vector3.zero;
		m_Rigidbody.AddForce(transform.forward * 30000);
		Particles.Play();
	}
	void returnToPlane() {
		Flying = false;
		Particles.Clear();
		Particles.Stop();
	}
	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.name == "AircraftJet") {
			// Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
		} else {
			Debug.Log("Left Collided with " + collision.gameObject.name);

			ShootableBox health1 = collision.collider.GetComponent<ShootableBox>();
			ShootableCar health2 = collision.collider.GetComponent<ShootableCar>();
			ShootableAA  health3 = collision.collider.GetComponent<ShootableAA>();

			if (health1 != null) { health1.Damage(missleDamage); }
			if (health2 != null) { health2.Damage(missleDamage); }
			if (health3 != null) { health3.Damage(missleDamage); }
		}
		returnToPlane();
	}
}