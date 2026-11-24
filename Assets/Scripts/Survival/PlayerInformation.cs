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

    public float staminaDelay = 0.75f;
    public float regenDelay = 0.75f;
    public float chargeDelay = 1f;
    public float oxygenDelay = 0.1f;
    private float lastSprint;
    private float lastRegen;
    private bool takenDamage = false;
    private float lastO2;
    private float lastCharge;

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
        lastSprint = 0f;
        lastRegen = 0f;
        lastO2 = 0f;
        lastCharge = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        StaminaRegen();
        HealthRegen();
    }

    // fixed rate
    void FixedUpdate()
    {

    }

    public void HealthChange(float change){
        currentHealth += change;
        if (change < 0){
            takenDamage = true;
            lastRegen = Time.time;
        }

        if (currentHealth > maxUniversal){
            currentHealth = 100;
        }
        else if (currentHealth < 0){
            currentHealth = 0;
            // disable UI and enable death screen
        }

        healthTxt.text = currentHealth.ToString("F0");
    }

    public void OxygenChange(float change){
        currentOxygen += change;

        if (currentOxygen > maxUniversal){
            currentOxygen = 100;
        }
        else if (currentOxygen < 0){
            currentOxygen = 0;
            // disable UI and enable death screen
        }

        oxygenTxt.text = currentOxygen.ToString("F0");
    }

    public void StaminaChange(float change){
        currentStamina += change;

        if (currentStamina > maxUniversal){
            currentStamina = 100;
        }
        else if (currentStamina <= 0){
            currentStamina = 0;
        }

        staminaTxt.text = currentStamina.ToString("F0");
    }

    public void ElectricityChange(float change){
        currentElectricity += change;

        if (currentElectricity > maxUniversal){
            currentElectricity = 100;
        }
        else if (currentElectricity < 0){
            currentElectricity = 0;
        }

        electricityTxt.text = currentElectricity.ToString("F0");
    }

    public void StaminaRegen(){
        if (!PlayerController.instance.isSprinting){
            if (lastSprint == 0f){
               lastSprint = Time.time; 
            }
            if (Time.time - lastSprint > staminaDelay){
                StaminaChange(+1);
                lastSprint = Time.time;
                staminaDelay = 0.1f;
            }
        }
        else{
            staminaDelay = 1f;
            lastSprint = 0f;
        }
    }

    public void HealthRegen(){
        if (!takenDamage){
            if (lastRegen == 0f){
               lastRegen = Time.time; 
            }
            if (Time.time - lastRegen > regenDelay){
                HealthChange(+1);
                lastRegen = Time.time;
                regenDelay = 0.25f;
            }
        }
        else if (takenDamage){
            regenDelay = 1f;
            if (Time.time - lastRegen > regenDelay){
                takenDamage = false;
            }
        }
    }
}
