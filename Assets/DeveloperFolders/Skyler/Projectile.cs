using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The speed this bullet will move at.")]
    [SerializeField]
    protected float speed = 10f;
    [Tooltip("The amount of hit point damage this attack will do.")]
    [SerializeField]
    protected int power = 1;
    [Tooltip("The amount of knockback this projectile will appy when it hits an object that can be knocked back. Zero or fewer results in no knockback.")]
    [SerializeField]
    protected float knockbackForce = 0f;
    [Tooltip("The amount of time in seconds that objects hit by this projectile will be stunned. Values of zero or less result in no hitstun.")]
    [SerializeField]
    protected float hitstunDuration = 0f;
    [Tooltip("The layers that this projectile will ignore.")]
    [SerializeField]
    protected List<int> ignoreLayers;
    [SerializeField]
    protected float lifeTime = 5f;
    [Tooltip("Should this projectile move forward automatically. Or should it wait for a direction to be given.")]
    [SerializeField]
    protected bool moveForward = false;
    [Tooltip("Should this projectile be destroyed when it hits an object.")]
    [SerializeField]
    protected bool destroyOnHit = true;

    protected Vector2 dir;
    protected Rigidbody2D rb;
    protected GameObject shooter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
        //if (moveForward) dir = transform.right;
    }

    private void Update()
    {
        if (moveForward)
        {
            rb.velocity = dir * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        HitBehaviour(col.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        HitBehaviour(col.gameObject);
    }

    private void HitBehaviour(GameObject hit)
    {
        // Don't collide with the object that shot it
        if (shooter == hit) return;

        // Don't collide with objects on an ignoreLayer
        foreach (int layer in ignoreLayers)
        {
            if (hit.layer == layer) return;
        }

        // Destroy the projectile if it hit something without health
        if(destroyOnHit && !hit.GetComponent<Health>())
        {
            Destroy(gameObject);
        }

        int damageDealt = 0;

        // Damage the collided object if it has health
        if (hit.GetComponent<Health>())
        {
            // If the damage dealt is greater than 0, destroy this projectile
            damageDealt = hit.GetComponent<Health>().TakeDamageInvincibility(power);
        }

        // If no damage was dealt then the don't do anything
        if (damageDealt == 0)
        {
            return;
        }

        // Destroy the projectile on hit
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 d)
    {
        dir = d.normalized;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetPower(int p)
    {
        power = p;
    }

    public void SetShooter(GameObject s)
    {
        shooter = s;
    }

    public void AddIgnoreLayer(int layer)
    {
        ignoreLayers.Add(layer);
    }

    public void SetLifeTime(float time)
    {
        lifeTime = time;
    }
}
