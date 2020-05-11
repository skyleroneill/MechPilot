using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTransform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform target;
    void Awake()
    {
        transform.SetParent(target);
        target.localPosition = Vector3.zero;
    }


}
