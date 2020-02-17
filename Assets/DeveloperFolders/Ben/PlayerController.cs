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
    private RaycastHit2D hit2d;
    
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
        //hit2d = Physics2D.CircleCast(feetPosition.position, 0.5f, Vector2.down, 0f, groundLayer);

        //print($"Player is on the ground: {isGrounded}");
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            rb2d.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void JumpDown()
    {
        if (Input.GetKeyDown(KeyCode.K) && isGrounded)
        //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            hit2d = Physics2D.CircleCast(feetPosition.position, 0.5f, Vector2.down, 0f, groundLayer);

            if (hit2d.collider.gameObject.layer == 10)
            {
                StartCoroutine(TemporaryTrigger(hit2d));
            }
        }
    }

    private IEnumerator TemporaryTrigger(RaycastHit2D hit)
    {
        hit.collider.isTrigger = true;
        
        rb2d.AddForce(new Vector2(0f, -jumpPower), ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.2f);

        hit.collider.isTrigger = false;
    }
}
