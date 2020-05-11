using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialRB : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    bool xInFacingDirection;
    [SerializeField]
    bool invert;
    [SerializeField]
    Vector2 velocity;
    [SerializeField]
    float angularVelocity;
    void Awake()
    {
        if (xInFacingDirection)
        {
            float dir;
            if (transform.localScale.x < 0) dir = -1;
            else dir = -1;

            if (invert) dir = -dir;
            velocity = new Vector2(velocity.x * dir, velocity.y);
        }



        if(velocity != Vector2.zero)
            GetComponent<Rigidbody2D>().velocity = velocity;
        if (angularVelocity != 0)
            GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
    }
}
