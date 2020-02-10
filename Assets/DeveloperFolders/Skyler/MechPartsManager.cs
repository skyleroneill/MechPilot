using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MechPartSlot
{
    [Tooltip("The slot this part will be placed at.")]
    public Transform slot;
    [Tooltip("The mech part instance.")]
    public GameObject partInstance;
}

public class MechPartsManager : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("The currently active part slot. Values less than zero designate no part is currently active.")]
    [SerializeField]
    private int activePart = -1;
    [SerializeField]
    private MechPartSlot[] partSlots;

    private void Start()
    {
        SetUpParts();
    }

    private void SetUpParts()
    {
        int i;
        for (i = 0; i < partSlots.Length; i++)
        {
            // The slot was found, so spawn the part as a child of the slot
            if (partSlots[i].slot)
            {
                partSlots[i].partInstance.transform.parent = partSlots[i].slot;
                partSlots[i].partInstance.transform.localPosition = Vector3.zero; // add in offset
                partSlots[i].partInstance.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // add in a z rotation offset
            }
            // The slot wasn't found, so just spawn the part as a child of this gameobject
            else
            {
                if (debug)
                    Debug.Log("Invalid slot on mech part " + i);

                partSlots[i].partInstance.transform.parent = transform;
                partSlots[i].partInstance.transform.localPosition = Vector3.zero; // TODO: add in offset
                partSlots[i].partInstance.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // TODO: add in a z rotation offset
            }
        }
    }

    public bool ChangeActivePart(int slotNum)
    {
        if (slotNum > partSlots.Length - 1)
            return false;

        activePart = slotNum;
        return true;
    }
}
