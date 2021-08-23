using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamScript : MonoBehaviour
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
        transform.position = new Vector3(Airplane.transform.position.x, 40000, Airplane.transform.position.z);
        transform.rotation = Quaternion.Euler(90, 0, -Airplane.transform.rotation.eulerAngles.y);
    }
}
