using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Global energy for gadgets. Gadgets spend energy (one-off with TryUse, continuous with SetDrain).
/// Energy refills inside a ChargingZone (the ship). With no energy left, gadgets should stop working
/// (check HasPower). Put it on the Player next to OxygenSystem.
/// </summary>
public class ElectricitySystem : MonoBehaviour
{
    [Header("Energy")]
    public float maxEnergy = 100f;
    [Tooltip("Energy gained per second inside a charging zone.")]
    public float refillPerSecond = 25f;
    [Range(0f, 1f)] public float lowThreshold = 0.2f;
    [Tooltip("Also recharge inside OxygenZones (handy if your ship is already an oxygen zone).")]
    public bool chargeInOxygenZones = false;

    [Header("Events")]
    public UnityEvent onEmpty;
    public UnityEvent onLow;

    public float Current { get; private set; }
    public float Normalized => maxEnergy > 0f ? Current / maxEnergy : 0f;
    public bool HasPower => Current > 0f;
    public bool IsCharging { get; private set; }
    public bool IsLow => Normalized <= lowThreshold;
    /// <summary>Total energy per second currently being drained by active gadgets.</summary>
    public float TotalDrain { get; private set; }

    readonly Dictionary<Object, float> _drains = new Dictionary<Object, float>();
    bool _warnedLow, _wasEmpty;

    void Awake() => Current = maxEnergy;

    /// <summary>One-off cost (e.g. a camera flash). Returns false and spends nothing if there isn't enough.</summary>
    public bool TryUse(float amount)
    {
        if (Current < amount) return false;
        Current -= amount;
        return true;
    }

    /// <summary>
    /// Continuous drain while a gadget is on (e.g. a flashlight). Call with the gadget as source and its
    /// energy-per-second; call again with 0 (or when turned off) to stop.
    /// </summary>
    public void SetDrain(Object source, float perSecond)
    {
        if (perSecond <= 0f) _drains.Remove(source);
        else _drains[source] = perSecond;
    }

    public void AddEnergy(float amount) => Current = Mathf.Clamp(Current + amount, 0f, maxEnergy);

    void Update()
    {
        IsCharging = InAnyZone();

        if (IsCharging)
        {
            Current = Mathf.Min(maxEnergy, Current + refillPerSecond * Time.deltaTime);
            TotalDrain = 0f;
        }
        else
        {
            TotalDrain = 0f;
            foreach (var kv in _drains)
                if (kv.Key != null) TotalDrain += kv.Value;
            Current = Mathf.Max(0f, Current - TotalDrain * Time.deltaTime);
        }

        if (!IsLow) _warnedLow = false;
        else if (!_warnedLow) { _warnedLow = true; onLow?.Invoke(); }

        bool empty = !HasPower;
        if (empty && !_wasEmpty) onEmpty?.Invoke();
        _wasEmpty = empty;
    }

    bool InAnyZone()
    {
        foreach (var z in ChargingZone.Active)
            if (z.isActiveAndEnabled && z.Contains(transform.position)) return true;

        if (chargeInOxygenZones)
            foreach (var z in ChargingZone.Active)
                if (z.isActiveAndEnabled && z.Contains(transform.position)) return true;

        return false;
    }
}
