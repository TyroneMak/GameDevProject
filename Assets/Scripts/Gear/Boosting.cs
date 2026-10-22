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
[DefaultExecutionOrder(-10)] // run before the character controller so our vertical velocity is used this frame
public class Boosting : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Id of the consumable item that switches Boosting on when it is used.")]
    public string itemId = "booster";
    public Key jumpKey = Key.Space;
    [Tooltip("Needs at least this much energy to start, so it doesn't stutter while recharging from empty.")]
    public float minEnergyToStart = 5f;

    [Header("Jetpack")]
    [Tooltip("Seconds you must be airborne and holding jump before the jetpack ignites (so a normal jump still works).")]
    public float holdDelay = 0.25f;
    public float jetSpeed = 6f;
    public float jetAcceleration = 25f;
    [Tooltip("Energy per second while thrusting.")]
    public float jetEnergyPerSecond = 12f;

    public bool IsBoosting { get; private set; }

    CharacterController _cc;
    ElectricitySystem _power;
    Inventory _inv;
    MonoBehaviour _controller;
    FieldInfo _verticalVelocity;
    float _airTime;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _power = GetComponent<ElectricitySystem>();
        _inv = GetComponent<Inventory>();

        _controller = GetComponent<ThirdPersonController>();
        if (_controller == null) _controller = GetComponent<FirstPersonController>();
        if (_controller != null)
            _verticalVelocity = _controller.GetType().GetField("_verticalVelocity", BindingFlags.Instance | BindingFlags.NonPublic);

        // Stay off until the booster item is consumed.
        enabled = false;

        if (_cc == null || _verticalVelocity == null || _inv == null)
        {
            Debug.LogWarning("Boosting: needs an Inventory, a CharacterController and a StarterAssets controller with _verticalVelocity. It will never switch on.", this);
            return;
        }
        _inv.Used += OnItemUsed;
    }

    void OnDestroy()
    {
        if (_inv != null) _inv.Used -= OnItemUsed;
    }

    void OnItemUsed(InventoryItem item)
    {
        if (item == null || item.id != itemId) return;
        enabled = true;
        _inv.Notify($"{item.displayName} active: hold {jumpKey} in mid-air to use the jetpack");
    }

    void OnDisable()
    {
        IsBoosting = false;
        if (_power != null) _power.SetDrain(this, 0f);
    }

    bool HasPower => _power == null || _power.HasPower;

    float Vertical
    {
        get => (float)_verticalVelocity.GetValue(_controller);
        set => _verticalVelocity.SetValue(_controller, value);
    }

    void Update()
    {
        bool grounded = _cc.isGrounded;
        if (grounded) _airTime = 0f;
        else _airTime += Time.deltaTime;

        var kb = Keyboard.current;
        bool held = kb != null && kb[jumpKey].isPressed;
        bool usable = !grounded && !_inv.IsOpen;

        bool thrust = false;

        if (usable && held && _airTime >= holdDelay && HasPower)
        {
            // Needs a little energy to ignite, but once going it runs until empty.
            thrust = IsBoosting || _power == null || _power.Current >= minEnergyToStart;
        }

        if (thrust)
        {
            Vertical = Mathf.MoveTowards(Vertical, jetSpeed, jetAcceleration * Time.deltaTime);
        }

        IsBoosting = thrust;
        if (_power != null) _power.SetDrain(this, thrust ? jetEnergyPerSecond : 0f);
    }
}
