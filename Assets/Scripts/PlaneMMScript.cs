using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMMScript : MonoBehaviour
{
    public GameObject Airplane;

    // Start is called before the first frame update
    void Start()
    {
        Airplane = GameObject.Find("AircraftJet");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Airplane.transform.position;
        transform.rotation = Quaternion.Euler(0, Airplane.transform.rotation.eulerAngles.y, 0);
    }
}
