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


    private void Awake() {
       target = GameObject.Find("ProtoBot_Head").transform;
       rb = GetComponent<Rigidbody2D>();
    }

   private void Update() {
       Vector2 dif = target.position - transform.position;
       float distance = dif.magnitude;
       Vector2 dir = dif.normalized;
       print(distance);
       if(Mathf.Abs(dif.x) < attackDistance)Attack(dir);
       else if(Mathf.Abs(dif.x) < agroRange)Move(dir);

   }
   void Move(Vector2 dir){
       dir = new Vector2(dir.x, 0).normalized;
       rb.velocity = (dir * speed) + new Vector2(0, rb.velocity.y);
   }

   void Attack(Vector2 dir){
       if(attackTimer>0){
           attackTimer -= Time.deltaTime;
           return;
       }

        rb.velocity = dir * attackForce;
        attackTimer = attackCooldown;



   }
}
