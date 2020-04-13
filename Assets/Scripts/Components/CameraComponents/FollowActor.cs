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

    [SerializeField]
    bool lockX;
    // Update is called once per frame

    private void Awake(){
        transform.parent = null;
    }

    void Update(){
        float x = transform.position.x;
        transform.position = Vector3.Lerp(transform.position, target.position, strength * Time.deltaTime);
        if (lockX) {
            Vector3 pos = transform.position;
            pos.SetX(x);
            transform.position = pos;
        }
    }
}
