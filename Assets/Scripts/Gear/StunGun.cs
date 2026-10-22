using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Stun gun shooting. Left click fires a ray from the centre of the screen; each shot costs energy
/// only works while the gun is the equipped hotbar item.
/// </summary>
public class StunGun : MonoBehaviour
{
    [Header("Setup")]
    public Camera viewCam;
    [Tooltip("Inventory item id that must be equipped for the gun to work.")]
    public string itemId = "stungun";

    [Header("Input")]
    public InputActionReference fireAction;
    bool _enabledAction;

    [Header("Shooting")]
    public float range = 50f;
    [Tooltip("Minimum seconds between shots.")]
    public float cooldown = 0.2f;
    [Tooltip("Energy spent per shot.")]
    public float energyPerShot = 10f;

    Inventory _inv;
    ElectricitySystem _power;
    float _nextShotTime;

    void Awake()
    {
        _inv = GetComponentInParent<Inventory>();
        _power = GetComponentInParent<ElectricitySystem>();
        if (viewCam == null) viewCam = Camera.main;
    }

    bool GunEquipped => _inv == null || (_inv.Equipped != null && _inv.Equipped.id == itemId);

    void Update()
    {
        if (viewCam == null || !GunEquipped) return;

        if (fireAction.action.WasPressedThisFrame() && Time.time >= _nextShotTime){
            if (_power != null && !_power.TryUse(energyPerShot)) return; // energy
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if (hit.transform.tag == "Enemy" || hit.transform.tag == "Ground")
            {
                hit.transform.GetComponent<MonsterFunctions>().TakeDamage();
                Debug.Log("Enemy/Ground stunned");
            }
            else{
                Debug.Log("Looking at nothing");
            }
        }
        _nextShotTime = Time.time + cooldown;
    }
}
