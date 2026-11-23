using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingZoneNew : MonoBehaviour
{
    public float oxygenCharge = 2f;
    public float electricCharge = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            PlayerInformation.instance.currentOxygen += oxygenCharge;
            PlayerInformation.instance.currentElectricity += electricCharge;
        }
    }
}
