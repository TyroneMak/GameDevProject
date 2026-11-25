using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingZoneNew : MonoBehaviour
{
    public float oxygenCharge = 1f;
    public float electricCharge = 2f;
    public float chargeSpeed = 0.2f;
    private float tickStart;

    // Start is called before the first frame update
    void Start()
    {
        tickStart = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Time.time - tickStart > chargeSpeed){ // every 0.2 of a second
            PlayerInformation.instance.OxygenChange(oxygenCharge);
            PlayerInformation.instance.ElectricityChange(electricCharge);
            tickStart = Time.time;
        }
    }
}
