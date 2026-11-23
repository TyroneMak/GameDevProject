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

    }

    // fixed rate
    void FixedUpdate()
    {
        
    }

    public void OxygenChange(float change){
        currentOxygen += change;
    }

    public void StaminaChange(float change){
        currentStamina += change;
    }

    public void ElectricityChange(float change){
        currentElectricity += change;
    }

    public void HealthChange(float change){
        currentHealth += change;
    }
}
