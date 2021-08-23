using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteScript : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    void Update()
    {
    	if (Input.GetButtonDown("Mute")) {
    		AudioListener.pause = !AudioListener.pause;
    	}
    }
}
