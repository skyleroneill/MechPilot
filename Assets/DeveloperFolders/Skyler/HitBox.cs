using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [Tooltip("The amount of hit point damage this attack will do.")]
    [SerializeField]
    protected int power = 1;
    [Tooltip("The layers that this projectile will ignore.")]
    [SerializeField]
    protected LayerMask ignoreLayers;
    [SerializeField]
    protected float lifeTime = 5f;

    protected List<int> ignoreLayerNumbers;
    protected Rigidbody2D rb;
    protected GameObject attacker;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        ignoreLayerNumbers = new List<int>();
        SetLayerNumbers();
    }

    private void SetLayerNumbers()
    {
        for (int i = 0; i < 32; i++)
        {
            if (ignoreLayers == (ignoreLayers | (1 << i)))
            {
                ignoreLayerNumbers.Add(i);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        HitBehaviour(col.gameObject);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        HitBehaviour(col.gameObject);
    }

    private void HitBehaviour(GameObject hit)
    {
        // Don't collide with the object that shot it
        if (attacker == hit) return;

        // Don't collide with objects on an ignoreLayer
        foreach (int layer in ignoreLayerNumbers)
        {
            if (hit.layer == layer) return;
        }

        // Damage the collided object if it has health
        if (hit.GetComponent<Health>())
        {
            hit.GetComponent<Health>().TakeDamageInvincibility(power);
        }
    }

    public void SetPower(int p)
    {
        power = p;
    }

    public void SetAttacker(GameObject s)
    {
        attacker = s;
    }

    public void SetLifeTime(float time)
    {
        lifeTime = time;
    }
}
