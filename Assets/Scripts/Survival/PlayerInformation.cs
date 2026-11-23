using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
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
        StaminaRegen();
        // when sprinting stamina goes down if(isSprinting){...}
    }

    // fixed rate
    void FixedUpdate()
    {

    }

    public void HealthChange(float change){
        currentHealth += change;
        healthTxt.text = currentHealth.ToString("F0");

        if (currentHealth > maxUniversal){
            currentHealth = 100;
        }
        else if (currentHealth < 0){
            currentHealth = 0;
            // disable UI and enable death screen
        }
    }

    public void OxygenChange(float change){
        currentOxygen += change;
        oxygenTxt.text = currentOxygen.ToString("F0");

        if (currentOxygen > maxUniversal){
            currentOxygen = 100;
        }
        else if (currentOxygen < 0){
            currentOxygen = 0;
            // disable UI and enable death screen
        }
    }

    public void StaminaChange(float change){
        currentStamina += change;
        staminaTxt.text = currentStamina.ToString("F0");

        if (currentStamina > maxUniversal){
            currentStamina = 100;
        }
        else if (currentStamina < 0){
            currentStamina = 0;
        }
    }

    public void ElectricityChange(float change){
        currentElectricity += change;
        electricityTxt.text = currentElectricity.ToString("F0");

        if (currentElectricity > maxUniversal){
            currentElectricity = 100;
        }
        else if (currentElectricity < 0){
            currentElectricity = 0;
        }
    }

    public void StaminaRegen(){
        if (PlayerController.instance.moveSpeed == PlayerController.instance.walkSpeed){
           // stamina regens
        }
    }
}
