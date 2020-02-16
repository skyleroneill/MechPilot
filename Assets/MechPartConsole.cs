using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechPartConsole : MonoBehaviour
{
    public MechPartController mechPart;
    public KeyCode activationKey;
    public GameObject pilot;

    private bool pilotNearby = false;

    private void Update()
    {
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
