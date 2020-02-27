#region Imports 

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
        targetDistance = minRadius + Random.value * (maxRadius-minRadius);
        targetAngle = minAngle + Random.value * (maxAngle-minAngle);
    }

    void Move(){
        Vector2 targetPos = (Vector2)target.position + (DegreeToVector2(targetAngle)*targetDistance);

        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;


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
        Vector2 dir = (target.position - transform.position).normalized;
        //newProjectile.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
        newProjectile.GetComponent<Projectile>().SetDirection(dir);
        newProjectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        newProjectile.GetComponent<Projectile>().SetShooter(gameObject);
    }
    #endregion

    #region Initialization
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("ProtoBot_Head").transform;
        curChangeTimer = targetChangeTime;
    }   
    #endregion
    
    #region Update Loop
    void Update(){
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
