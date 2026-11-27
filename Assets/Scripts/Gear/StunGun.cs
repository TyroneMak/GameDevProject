using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Stun gun shooting. Left click fires a ray from the centre of the screen; each shot costs energy
/// only works while the gun is the equipped hotbar item.
/// </summary>
public class StunGun : MonoBehaviour
{
    private PlayerInformation playerinfo;

    public Camera viewCam;
    public InputActionReference fireAction;

    private bool GunEquipped;
    private float  shotTime;
    public float range = 50f;
    public float cooldown = 0.2f;
    public float energyPerShot = 10f;

    void Awake()
    {
        
    }

    void Start()
    {
        playerinfo = PlayerInformation.instance;
    }

    void Update()
    {
        if (viewCam == null || !GunEquipped) return;

        if (fireAction.action.WasPressedThisFrame() && Time.time - shotTime > cooldown){
            if (playerinfo.currentEnergy >= energyPerShot){
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Ray ray = viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if ((hit.transform.tag == "Enemy" || hit.transform.tag == "Ground") && hit.distance <= range)
            {
                hit.transform.GetComponent<MonsterFunctions>().StunEffect();
                Debug.Log("Enemy stunned");
            }
            else{
                Debug.Log("Looking at nothing");
            }

            // instantiates a blue ray 
                // Instantiate(ray, ...)
            // if nothings hit it just goes max distance where player facing
            // else it stops where it hits
        }
        playerinfo.EnergyChange(-energyPerShot);
        shotTime = Time.time;
    }
}
