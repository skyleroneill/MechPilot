using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float movementSpeed = 10f;
    private const float jumpPower = 10f;
    private bool isGrounded;
    private bool canJump;
    private Rigidbody2D rb2d;
    
    public Transform feetPosition; 
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Jump();
        
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movementSpeed * Time.deltaTime * movement;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, 0.5f);
        print($"Player is on the ground: {isGrounded}");
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                JumpDown();
            }
            else
            {
                rb2d.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            }
            
        }
    }

    private void JumpDown()
    {
        // If found, deactivate collider on platform (Maybe create a function on a script for platform prefabs?)
        if (isGrounded)
        {
            
        }
    }
}
