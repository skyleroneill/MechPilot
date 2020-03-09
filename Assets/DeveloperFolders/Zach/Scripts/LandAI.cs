using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandAI : MonoBehaviour{

    [SerializeField]
    float agroRange = 5;
    
    [SerializeField]
    float speed = 1;
    [SerializeField]
    float attackDistance = 4f;
    Transform target;
    Rigidbody2D rb;
    [SerializeField]
    float attackCooldown = 3;

    float attackTimer = 0;

    [SerializeField]
    float attackForce = 5f;

    Animator anim;

    bool air = false;

    float lookDir;

    
    private void Awake() {
       target = GameObject.Find("ProtoBot_Head").transform;
       rb = GetComponent<Rigidbody2D>();
       anim = GetComponentInChildren<Animator>();
    }

   private void Update() {
        Vector2 dif = target.position - transform.position;
        float distance = dif.magnitude;
        Vector2 dir = dif.normalized;
        lookDir = dir.x / Mathf.Abs(dir.x);
        anim.transform.localScale = new Vector2(lookDir, 1);

        if(Mathf.Abs(dif.x) < attackDistance)Attack(dir);
        else if(Mathf.Abs(dif.x) < agroRange)Move(dir);
        else anim.SetFloat("Speed", rb.velocity.magnitude);
            
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.01f);
        if(!hit)
            air = true;
        else
            air = false;
        anim.SetBool("Air", air);
        if(air)
            anim.SetFloat("Speed", 0);

   }
   void Move(Vector2 dir){
       dir = new Vector2(dir.x, 0).normalized;
       rb.velocity = (dir * speed) + new Vector2(0, rb.velocity.y);
       anim.SetFloat("Speed", rb.velocity.magnitude);
   }

   void Attack(Vector2 dir){
       if(air)return;
       if(attackTimer>0 ){
           attackTimer -= Time.deltaTime;
           anim.SetFloat("Speed", rb.velocity.magnitude);
           return;
       }
       anim.SetFloat("Speed", 0);
        float x = dir.x/ Mathf.Abs(dir.x);
        anim.SetTrigger("Jump");
        rb.velocity = new Vector2(x, 1) * attackForce;
        attackTimer = attackCooldown;
        



   }
}
