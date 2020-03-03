using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechPartEnergy : MonoBehaviour
{
    public bool debug = false;
    private KeyCode debugKey = KeyCode.P;

    [SerializeField]
    private int maxEnergy = 10;
    [SerializeField]
    private int currentEnergy = 0;
    [SerializeField]
    [Tooltip("The amount of energy regened at after regen time has passed.")]
    private int regenRate = 1;
    [SerializeField]
    [Tooltip("The amount of time in seconds until regen rate energy is regained.")]
    private float regenTime = 0.5f;
    [SerializeField]
    [Tooltip("The amount of time after using energy to wait before beginning regen.")]
    private float regenCooldown = 1f;

    private float regenCooldownTimer;
    private float regenTimer = 0f;

    private void Start()
    {
        currentEnergy = maxEnergy;
        regenCooldownTimer = regenCooldown;
    }

    private void Update()
    {
        if (debug && Input.GetKeyDown(debugKey))
            UseEnergy(1);

        RegenTimer();
    }

    private void RegenTimer()
    {
        if (currentEnergy == maxEnergy) return;

        if (regenCooldownTimer < regenCooldown)
        {
            regenCooldownTimer += Time.deltaTime;
        }
        else if (regenTimer < regenTime)
        {
            regenTimer += Time.deltaTime;
        }
        else
        {
            regenTimer = 0f;
            ApplyRegen();
        }
    }

    private void ApplyRegen()
    {
        currentEnergy += regenRate;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
    }

    public bool UseEnergy(int amount)
    {
        if (amount > currentEnergy) return false;

        regenCooldownTimer = 0f;
        currentEnergy -= amount;
        return true;
    }
}
