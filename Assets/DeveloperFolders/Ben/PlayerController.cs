﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float movementSpeed = 10f;
    private const float jumpPower = 12.5f; // Tried values: 10, 15, 12.5
    private float fallMultiplier = 2.5f;
    //private float lowJumpMultiplier = 2f;
    private bool isGrounded;
    private Rigidbody2D rb2d;
    private RaycastHit2D hit2d;
    private RaycastHit2D ladder;

    private bool isClimbing;
    
    public Transform feetPosition;
    public LayerMask groundLayer;
    public LayerMask ladderLayer;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isClimbing = false;
    }
    
    private void Update()
    {
        if (!isClimbing)
        {
            Jump();
            JumpDown();
        }
    }
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb2d.velocity.y);

        if (isClimbing)
        {
            rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed,Input.GetAxis("Vertical") * movementSpeed);
        }
        
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, 0.5f, groundLayer);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S) && isGrounded)
        {
            rb2d.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            isGrounded = false;
        }
        
        rb2d.velocity += Time.deltaTime * Physics2D.gravity.y * (fallMultiplier - 1) * Vector2.up;
    }

    private void JumpDown()
    {
        if ((Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            hit2d = Physics2D.CircleCast(feetPosition.position, 0.5f, Vector2.down, 0f, groundLayer);
            print($"Object's name: {hit2d.collider.name}");
            
            if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Jumpable"))
                StartCoroutine(TemporaryTrigger(hit2d));
        }
    }

    private IEnumerator TemporaryTrigger(RaycastHit2D hit)
    {
        hit.collider.isTrigger = true;
        
        rb2d.AddForce(new Vector2(0f, -jumpPower), ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.2f);

        hit.collider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climbable"))
            isClimbing = true;
        
        print("Found a ladder");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Climbable"))
            isClimbing = false;

        print("Bye bye ladder");
    }
}
