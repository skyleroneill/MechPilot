using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float movementSpeed = 10f;
    private const float jumpPower = 10f;
    private bool isGrounded;
    private Rigidbody2D rb2d;
    private BoxCollider2D col2d;
    
    public Transform feetPosition;
    public LayerMask groundLayer;
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<BoxCollider2D>();
    }
    
    private void Update()
    {
        Jump();
        JumpDown();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb2d.velocity.y);
        
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, 0.5f, groundLayer);
        print($"Player is on the ground: {isGrounded}");
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            rb2d.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
        }
    }

    private void JumpDown()
    {
        // If found, deactivate collider on platform (Maybe create a function on a script for platform prefabs?)
        /*if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            ContactFilter2D c = new ContactFilter2D();
            List<Collider2D> coll = new List<Collider2D>();
            c.SetLayerMask(groundLayer);
            Physics2D.OverlapCircle(feetPosition.position, 0.5f, c, coll);
            print($"How many things OverlapCircle found: {coll.Count}");
        }*/

        if (Input.GetKeyDown(KeyCode.K) && isGrounded)
        {
            col2d.isTrigger = true;
            rb2d.AddForce(new Vector2(0f, -10f), ForceMode2D.Impulse);
            col2d.isTrigger = false;
        }
    }
}
