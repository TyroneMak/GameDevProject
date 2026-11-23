using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    public static PlayerInformation instance;

    public float currentOxygen;
    public float currentElectricity;
    public float currentHealth;
    public float  currentStamina;
    public float maxUniversal = 100f;
    public float tickRate = 0f;
    public float sprintDrainMultiplier = 1.5f;

    public Text healthTxt, oxygenTxt, electricityTxt, staminaTxt;

    private void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentElectricity = maxUniversal;
        currentOxygen = maxUniversal;
        currentHealth = maxUniversal;
        currentStamina = maxUniversal;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - tickRate > 1){
            setOxygen(1f);
        }
    }

    public void setOxygen(float decrease){
        currentOxygen -= decrease;
    }

    public void setOxygen(float decrease, float multiplier){
        currentOxygen -= decrease;
    }
}
