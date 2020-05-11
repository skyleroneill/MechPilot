#region Imports 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class FlyingAI : MonoBehaviour{

    #region Serialized Fields
    Transform target;
    [SerializeField]
    float force = 5;
    [SerializeField]
    float MaxVelocity;
    [SerializeField]
    float minRadius;
    [SerializeField]
    float maxRadius;
    [SerializeField]
    float targetChangeTime;
    [SerializeField]
    float shootTime;
    [SerializeField]
    Transform projectile;

    [SerializeField]
    float minAngle;
    [SerializeField]
    float maxAngle;
    [SerializeField]
    float projectileSpeed;

    [SerializeField]
    bool invertFacingDir;

    bool hostile = false;
    [SerializeField]
    float hostileStartRange;

    #endregion
  
    #region Private Variables
    Rigidbody2D rb;

    float targetDistance;
    float targetAngle;

    float curChangeTimer = 0;
    
    float curShootTimer = 0;
    #endregion
  
    #region AI Movement
    void ChangeTargetPoint(){
        curChangeTimer += Time.deltaTime;
        if(curChangeTimer < targetChangeTime)
            return;
        curChangeTimer = 0;
        targetDistance = minRadius + UnityEngine.Random.value * (maxRadius-minRadius);
        targetAngle = minAngle + UnityEngine.Random.value * (maxAngle-minAngle);
    }



    void Move(){
        Vector2 targetPos = (Vector2)target.position + (DegreeToVector2(targetAngle)*targetDistance);

        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        Vector2 targetDir = target.position - transform.position;
        float xScale = Mathf.Abs( transform.localScale.x);
        float lookDir = targetDir.x/Mathf.Abs(targetDir.x);
        lookDir = invertFacingDir ? -lookDir : lookDir;
        transform.localScale = new Vector3(-xScale*lookDir, transform.localScale.y, transform.localScale.z);

        rb.AddForce(force * dir * Time.deltaTime);
        Vector2 vel = Vector2.ClampMagnitude(rb.velocity, MaxVelocity);
        rb.velocity = vel;
    }
    #endregion
  
    #region AI Combat
    void Shoot(){
        curShootTimer += Time.deltaTime;
        if(curShootTimer < shootTime)
            return;
        curShootTimer = 0;

        Transform newProjectile = Instantiate(projectile, null);
        newProjectile.position = transform.position;

        float random = UnityEngine.Random.value * 5;
        Vector3 targetPosition = target.position;
        bool aimAtHead = (UnityEngine.Random.value - .3 < 0);
        if(!aimAtHead)
        targetPosition -= Vector3.up*random;
        Vector2 dir = (targetPosition - transform.position).normalized;
        //newProjectile.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
        newProjectile.GetComponent<Projectile>().SetDirection(dir);
        newProjectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        newProjectile.GetComponent<Projectile>().SetShooter(gameObject);
    }
    #endregion

    void CheckHostility(){
        if((target.position - transform.position).magnitude < hostileStartRange)hostile = true;
        if(hostileStartRange == 0) hostile = true;
        if(GetComponent<Health>().GetMaxHealth() != GetComponent<Health>().GetHealth()) {
            hostile = true;
            print("NO" + GetComponent<Health>().GetMaxHealth() + " " + GetComponent<Health>().GetHealth());
        }
        if(hostile){
            GetComponentInChildren<Animator>().SetBool("Hostile", true);
        }
    }

    #region Initialization
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("ProtoBot_Head").transform;
        curChangeTimer = targetChangeTime;

        float lookDir = invertFacingDir ? -1 : 1;
        transform.localScale = new Vector3(-transform.localScale.x*lookDir, transform.localScale.y, transform.localScale.z);
    }   
    #endregion
    
    #region Update Loop
    void Update(){
        CheckHostility();
        if(!hostile) return;
        ChangeTargetPoint();
        Shoot();
        Move();
    }
    #endregion

    #region Helper Functions
        public static Vector2 RadianToVector2(float radian){
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    
        public static Vector2 DegreeToVector2(float degree){
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }
    #endregion 
    
}
