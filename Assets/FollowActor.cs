using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActor : MonoBehaviour
{
    [SerializeField]
    Transform target;
    // Start is called before the first frame update
    [SerializeField]
    float strength;

    // Update is called once per frame

    private void Awake(){
        transform.parent = null;
    }

    void Update(){
        transform.position = Vector3.Lerp(transform.position, target.position, strength * Time.deltaTime);
    }
}
