using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Specific Variables")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpPower = 12.5f; // Tried values: 10, 15, 12.5
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    private float jumpCounter = 1;
    private bool isGrounded;
    private RaycastHit2D hit2d;
    private RaycastHit2D ladder;
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sprite;
    
    private bool isFacingRight = false;

    [Header("")]
    public Transform feetPosition;
    public LayerMask groundLayer;
    public LayerMask ladderLayer;
    public bool isHacking = false;
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (!isHacking)
        {
            Jump();
            JumpDown();
            
            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity += Time.deltaTime * Physics2D.gravity.y * (fallMultiplier - 1) * Vector2.up;
            }
            else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                print("Slowing jump");
                rb2d.velocity += Time.deltaTime * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Vector2.up;
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (!isHacking)
        {
            var horizontalMovement = Input.GetAxisRaw("Horizontal"); 
            rb2d.velocity = new Vector2(horizontalMovement * movementSpeed, rb2d.velocity.y);

            // Kinda hacky, but looks WAY better in game
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
                

            if (isFacingRight == false && horizontalMovement > 0)
                Flip();
            else if (isFacingRight == true && horizontalMovement < 0)
                Flip();
        
            isGrounded = Physics2D.OverlapCircle(feetPosition.position, 0.15f, groundLayer);

            if (isGrounded)
            {
                jumpCounter = 1;
                
                if (rb2d.velocity.y <= 0f)
                    anim.SetBool("isJumping", false);
            }
            else
            {
                anim.SetBool("isJumping", true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(feetPosition.position, 0.15f);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S) && jumpCounter > 0)
        {
            anim.SetTrigger("startJump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            jumpCounter--;
        }
        
        // To help prevent floating
        // rb2d.velocity += Time.deltaTime * Physics2D.gravity.y * (fallMultiplier - 1) * Vector2.up;
    }

    private void JumpDown()
    {
        if ((Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            hit2d = Physics2D.CircleCast(feetPosition.position, 0.5f, Vector2.down, 0f, groundLayer);
            print($"Object's name: {hit2d.collider.name}");
            
            if (hit2d.collider.gameObject.layer == LayerMask.NameToLayer("Jumpable") && hit2d.collider.gameObject.layer != LayerMask.NameToLayer("BaseFloor"))
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void HideOurShame(bool hidePlayer)
    {
        // Player can't move
        // Hide sprite
        if (hidePlayer)
        {
            isHacking = true;
            sprite.enabled = false;
        }
        else
        {
            isHacking = false;
            sprite.enabled = true;
        }
    }
}
