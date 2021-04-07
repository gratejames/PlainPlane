using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTarget : MonoBehaviour
{
	private Vector3 pos;

    // Update is called once per frame
    void Update() {
    	pos = this.transform.position;
    	pos.z += 0.4f;
    	this.transform.position = pos;	
    }
}
