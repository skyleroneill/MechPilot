using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityHatch : MonoBehaviour
{
    Transform target;
    [SerializeField]
    float hatchRange = 5;
    private void Awake()
    {
        target = GameObject.Find("ProtoBot_Head").transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - target.position).magnitude < hatchRange)
        {
            GetComponent<Animator>().SetBool("Hatch", true);
        }
    }
}
