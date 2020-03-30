using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechPartConsole : MonoBehaviour
{
    public MechPartController mechPart;
    public KeyCode activationKey = KeyCode.E;
    public KeyCode repairKey = KeyCode.Q;
    public GameObject pilot;
    public bool canActivate = true;
    public bool checkPartHealth = true;
    public float repairCooldown = 0.5f;
    public int repairRate = 1;

    private bool pilotNearby = false;
    private bool activatable = true;
    private bool isActive = false;
    private bool isRepairing = false;
    private Health partHealth;
    private Animator anim;

    private float repairTimer = 0f;

    private void Start()
    {
        // Don't check for health if there is none
        checkPartHealth = mechPart.gameObject.GetComponent<Health>() != null;
        partHealth = mechPart.gameObject.GetComponent<Health>();
        anim = mechPart.gameObject.GetComponent<Animator>();
        activatable = canActivate;
    }

    private void Update()
    {
        activatable = checkPartHealth ? CheckPartAlive() : canActivate;
        ActivatePart();
        RepairPart();

        if (anim)
            AnimControl();
    }

    private void AnimControl()
    {
        // Prioritize the active animation over the repair animation
        isActive = mechPart.IsActive();
        isRepairing = isActive ? false : repairTimer > 0f;

        anim.SetBool("active", isActive);
        anim.SetBool("repairing", isRepairing);

            // player.GetComponent<PlayerController>().HideOurShame(isActive || isReparing);
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
            // Disable the mech part if it is not activatable, but is active
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
