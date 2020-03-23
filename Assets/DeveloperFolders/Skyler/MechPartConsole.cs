using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechPartConsole : MonoBehaviour
{
    public MechPartController mechPart;
    public KeyCode activationKey;
    public KeyCode repairKey;
    public GameObject pilot;
    public bool canActivate = true;
    public bool checkPartHealth = true;
    public float repairCooldown = 0.5f;
    public int repairRate = 1;

    private bool pilotNearby = false;
    private bool activatable = true;
    private Health partHealth;

    private float repairTimer = 0f;

    private void Start()
    {
        // Don't check for health if there is none
        checkPartHealth = mechPart.gameObject.GetComponent<Health>() != null;
        partHealth = mechPart.gameObject.GetComponent<Health>();
        activatable = canActivate;
    }

    private void Update()
    {
        activatable = checkPartHealth ? CheckPartAlive() : canActivate;
        ActivatePart();
        RepairPart();
    }

    private bool CheckPartAlive()
    {
        // Return true if the part is alive, false if dead
        return mechPart.gameObject.GetComponent<Health>().GetHealth() > 0;
    }

    private void ActivatePart()
    {
        if (!activatable)
        {
            if (mechPart.IsActive())
            {
                pilot.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                mechPart.ToggleActivation();
            }  
            return;
        }

        if (Input.GetKeyDown(activationKey) && pilotNearby)
        {
            mechPart.ToggleActivation();

            // Freeze the player or not
            if (mechPart.IsActive())
                pilot.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            else
                pilot.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void RepairPart()
    {
        if (checkPartHealth && partHealth.GetHealth() < partHealth.GetMaxHealth())
        {
            if (Input.GetKey(repairKey) && pilotNearby && !mechPart.IsActive())
            {
                repairTimer += Time.deltaTime;
            }
            else
            {
                repairTimer = 0f;
                return;
            }
            
            if(repairTimer >= repairCooldown)
            {
                partHealth.Heal(repairRate);
                repairTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == pilot.layer)
        {
            pilotNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == pilot.layer)
        {
            pilotNearby = false;
        }
    }
}
