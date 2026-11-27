using System.Reflection;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Jetpack for the booster item
/// Sits switched off on the Player until the booster item is consumed from the hotbar
/// then stays on and spends energy from ElectricitySystem when in use.
/// Hold the jump key in mid-air to thrust; drains energy per second while thrusting.
/// </summary>
public class Boosting : MonoBehaviour
{
    private PlayerController playerControl;
    private PlayerInformation playerinfo;
    public InputActionReference boostButton;

    public float energyNeeded = 2f;
    public float jetAcceleration = 10f;
    public float energyTick = 0.75f;
    private float lastBoost;

    public bool boosting = false;

    void Awake()
    {
        
    }

    void Start()
    {
        //boosting = false;
        lastBoost = 0f;
        playerControl = PlayerController.instance;
        playerinfo = PlayerInformation.instance;
    }

    void Update()
    {
        if (boostButton.action.IsPressed() && playerinfo.currentEnergy >= energyNeeded && !playerControl.isGrounded){
            boosting = true;
            if (Time.time - lastBoost > energyTick){
                playerinfo.EnergyChange(-energyNeeded);
                lastBoost = Time.time;
            }
        }
        else{
            lastBoost = Time.time;
            boosting = false;
        }
    }

    void FixedUpdate()
    {
        if (boosting){
            Vector3 temp = playerControl.theRB.velocity;
            temp.y += jetAcceleration * 0.02f;
            playerControl.theRB.velocity = temp;
        }
    }
}
