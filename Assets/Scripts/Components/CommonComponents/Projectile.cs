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
    [Tooltip("The layers that this projectile will ignore.")]
    [SerializeField]
    protected LayerMask ignoreLayers;
    [SerializeField]
    protected float lifeTime = 5f;
    [Tooltip("Should this projectile move forward automatically. Or should it wait for a direction to be given.")]
    [SerializeField]
    protected bool moveForward = false;
    [Tooltip("Should this projectile be destroyed when it hits an object.")]
    [SerializeField]
    protected bool destroyOnHit = true;

    protected List<int> ignoreLayerNumbers;
    protected Vector2 dir;
    protected Rigidbody2D rb;
    protected GameObject shooter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
        SetLayerNumbers();
    }

    private void SetLayerNumbers()
    {

        ignoreLayerNumbers = new List<int>();

        for (int i = 0; i < 32; i++)
        {
            if(ignoreLayers == (ignoreLayers | (1 << i)))
            {
                ignoreLayerNumbers.Add(i);
            }
        }
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
        Health hitHealth = hit.GetComponent<Health>();

        // Use parent object's health if the hit object doesn't have health
        if (!hitHealth)
            hitHealth = hit.GetComponentInParent<Health>();

        // Don't collide with the object that shot it
        if (shooter == hit) return;

        // Don't collide with objects on an ignoreLayer
        foreach (int layer in ignoreLayerNumbers)
        {
            if (hit.layer == layer) return;
        }

        // Destroy the projectile if it hit something without health
        if(destroyOnHit && !hitHealth)
        {
            Destroy(gameObject);
        }

        int damageDealt = 0;

        // Damage the collided object if it has health
        if (hitHealth)
        {
            // If the damage dealt is greater than 0, destroy this projectile
            damageDealt = hitHealth.TakeDamageInvincibility(power);
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

    public void SetLifeTime(float time)
    {
        lifeTime = time;
    }

    public void SetIgnoreLayers(LayerMask mask)
    {
        //ignoreLayerNumbers.Clear();
        ignoreLayers = mask;
        SetLayerNumbers();
    }
}
