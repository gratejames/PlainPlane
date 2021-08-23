using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTP : MonoBehaviour
{
	public GameObject main;
	void Start() 
	{
		this.gameObject.SetActive(false);
		main.gameObject.SetActive(true);
	}
	// Start is called before the first frame update
	public void Button() 
	{
		this.gameObject.SetActive(true);
		main.gameObject.SetActive(false);
	}
	public void BACK() {
		this.gameObject.SetActive(false);
		main.gameObject.SetActive(true);
	}
}
