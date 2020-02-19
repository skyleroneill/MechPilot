using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProjectileEffect
{
    public GameObject projectile;
    public Transform firePoint;
    public int power;
    public float coolDown;
    public float speed;
    public float lifeTime;
    public float angleOffset;
    public float effectDelay;
    // TODO: Ignore layers
    [HideInInspector]
    public bool onCoolDown;
}

[System.Serializable]
public struct MeleeEffect
{
    public GameObject hitBox;
    public Transform firePoint;
    public int power;
    public float coolDown;
    public float lifeTime;
    public float effectDelay;
    public bool hitBoxAsChild;
    [HideInInspector]
    public bool onCoolDown;
}

[System.Serializable]
public struct MoveEffect
{
    public enum MoveType { Velocity, Force, Impulse }
    public Rigidbody2D movingRB;
    public Vector2 moveDirection;
    public MoveType moveType;
    public float coolDown;
    public float speed;
    public float effectDelay;
    public bool mustBeGrounded;
    public bool faceMoveDirection;
    public BoxCastParameters groundCheckParams;
    [HideInInspector]
    public bool onCoolDown;
}

[System.Serializable]
public struct BoxCastParameters
{
    public Transform origin;
    public Vector2 size;
    public float angle;
    public Vector2 direction;
    public float distance;
    public LayerMask layerMask;
}

[System.Serializable]
public struct KeyDownEffects
{
    public KeyCode downKey;
    public ProjectileEffect[] projectileEffects;
    public MeleeEffect[] meleeEffects;
    public MoveEffect[] moveEffects;
}

[System.Serializable]
public struct KeyUpEffects
{
    public KeyCode upKey;
    public ProjectileEffect[] projectileEffects;
    public MeleeEffect[] meleeEffects;
    public MoveEffect[] moveEffects;
}

[System.Serializable]
public struct KeyHoldEffects
{
    public KeyCode holdKey;
    public ProjectileEffect[] projectileEffects;
    public MeleeEffect[] meleeEffects;
    public MoveEffect[] moveEffects;
}

public class MechPartController : MonoBehaviour
{
    public bool debug = false;
    [SerializeField]
    private bool active = false;
    public bool rotatePartToCursor = true;
    public Vector2 partRotationConstraints;
    public KeyDownEffects[] keyDownEffects;
    public KeyUpEffects[] keyUpEffects;
    public KeyHoldEffects[] keyHoldEffects;

    private bool moving = false;
    private bool facingRight = true;
    private Animator anim;


    private enum KeyType { KeyDown, KeyUp, KeyHold }

    public void ToggleActivation()
    {
        active = !active;
    }

    public bool IsActive()
    {
        return active;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        facingRight = transform.root.localScale.x >= 0;
    }

    private void Update()
    {
        if (!active)
        {
            // HACK: Stop moving animation
            if (anim && !moving)
            {
                anim.SetBool("moving", false);
            }
            moving = false;

            return;
        }

        facingRight = transform.root.localScale.x >= 0;

        // Rotate to part to aim at cursor
        if (rotatePartToCursor)
        {
            RotateTransformToCursor(transform);
        }

        KeyDown();
        KeyUp();
        KeyHold();

        // HACK: Stop moving animation
        if (anim && !moving)
        {
            anim.SetBool("moving", false);
        }
        moving = false;
    }

    private void KeyDown()
    {
        int i;
        int j;
        for (i = 0; i < keyDownEffects.Length; i++)
        {
            // Perform effects if current down key was pressed
            if (Input.GetKeyDown(keyDownEffects[i].downKey))
            {
                // Projectile effects
                for (j = 0; j < keyDownEffects[i].projectileEffects.Length; j++)
                {
                    PerformProjectileEffect(keyDownEffects[i].projectileEffects[j], KeyType.KeyDown, i, j);
                }

                // Melee effects
                for (j = 0; j < keyDownEffects[i].meleeEffects.Length; j++)
                {
                    PerformMeleeEffect(keyDownEffects[i].meleeEffects[j], KeyType.KeyDown, i, j);
                }

                // Move effects
                for (j = 0; j < keyDownEffects[i].moveEffects.Length; j++)
                {
                    PerformMoveEffect(keyDownEffects[i].moveEffects[j], KeyType.KeyDown, i, j);
                }
            }
        }
    }

    private void KeyUp()
    {
        int i;
        int j;
        for (i = 0; i < keyUpEffects.Length; i++)
        {
            // Perform effects if current down key was pressed
            if (Input.GetKeyUp(keyUpEffects[i].upKey))
            {

                // Projectile effects
                for (j = 0; j < keyUpEffects[i].projectileEffects.Length; j++)
                {
                    PerformProjectileEffect(keyUpEffects[i].projectileEffects[j], KeyType.KeyUp, i, j);
                }

                // Melee effects
                for (j = 0; j < keyUpEffects[i].meleeEffects.Length; j++)
                {
                    PerformMeleeEffect(keyUpEffects[i].meleeEffects[j], KeyType.KeyUp, i, j);
                }

                // Move effects
                for (j = 0; j < keyUpEffects[i].moveEffects.Length; j++)
                {
                    PerformMoveEffect(keyUpEffects[i].moveEffects[j], KeyType.KeyUp, i, j);
                }
            }
        }
    }

    private void KeyHold()
    {
        int i;
        int j;
        for (i = 0; i < keyHoldEffects.Length; i++)
        {
            // Perform effects if current down key was pressed
            if (Input.GetKey(keyHoldEffects[i].holdKey))
            {

                // Projectile effects
                for (j = 0; j < keyHoldEffects[i].projectileEffects.Length; j++)
                {
                    PerformProjectileEffect(keyHoldEffects[i].projectileEffects[j], KeyType.KeyHold, i, j);
                }

                // Melee effects
                for (j = 0; j < keyHoldEffects[i].meleeEffects.Length; j++)
                {
                    PerformMeleeEffect(keyHoldEffects[i].meleeEffects[j], KeyType.KeyHold, i, j);
                }

                // Move effects
                for (j = 0; j < keyHoldEffects[i].moveEffects.Length; j++)
                {
                    PerformMoveEffect(keyHoldEffects[i].moveEffects[j], KeyType.KeyHold, i, j);
                }
            }
        }
    }

    private void PerformProjectileEffect(ProjectileEffect effect, KeyType type, int keyEffect, int projectileEffect)
    {
        // Don't do anything if on cool down
        if (effect.onCoolDown)
            return;

        // Perform cool down
        if (effect.coolDown > 0f && !effect.onCoolDown)
            StartCoroutine(ProjectileEffectCoolDownTime(type, keyEffect, projectileEffect));

        // Play fire animation
        if (anim)
        {
            anim.SetTrigger("fire");
        }

        // Delay effect for specified amount of time
        if(effect.effectDelay > 0)
        {
            StartCoroutine(WaitForProjectileDelay(effect));
            return;
        }

        SpawnProjectile(effect);
    }

    private void SpawnProjectile(ProjectileEffect effect)
    {
        effect.firePoint.localRotation = Quaternion.Euler(0f, 0f, facingRight ? 0f : 180f);

        Projectile newProjectile = Instantiate(effect.projectile,
                                               effect.firePoint.position,
                                               effect.firePoint.rotation).GetComponent<Projectile>();

        // Failed to create a new projectile
        if (!newProjectile)
            return;

        // Initialize the projectile
        Vector3 rotatedVector = Quaternion.AngleAxis(effect.angleOffset, Vector3.forward) * effect.firePoint.right;
        newProjectile.SetDirection(rotatedVector);
        newProjectile.SetPower(effect.power);
        newProjectile.SetSpeed(effect.speed);
        newProjectile.SetShooter(gameObject);
        newProjectile.SetLifeTime(effect.lifeTime);
        // TODO: Add ignore layers
    }

    private void PerformMeleeEffect(MeleeEffect effect, KeyType type, int keyEffect, int meleeEffect)
    {
        // Don't do anything if on cool down
        if (effect.onCoolDown)
            return;

        // Perform cool down
        if (effect.coolDown > 0f && !effect.onCoolDown)
            StartCoroutine(MeleeEffectCoolDownTime(type, keyEffect, meleeEffect));

        // Play fire animation
        if (anim)
        {
            anim.SetTrigger("fire");
        }

        // Delay effect for specified amount of time
        if (effect.effectDelay > 0)
        {
            StartCoroutine(WaitForMeleeDelay(effect));
            return;
        }

        SpawnMeleeHitBox(effect);
    }

    private void SpawnMeleeHitBox(MeleeEffect effect)
    {
        HitBox newHitBox;

        // Spawn hit box as child of fire point
        if (effect.hitBoxAsChild)
            newHitBox = Instantiate(effect.hitBox,
                                 effect.firePoint.position,
                                 effect.firePoint.rotation,
                                 effect.firePoint).GetComponent<HitBox>();
        // Spawn hit box not as child
        else
            newHitBox = Instantiate(effect.hitBox, 
                                 effect.firePoint.position,
                                 effect.firePoint.rotation).GetComponent<HitBox>();

        if (!newHitBox)
            return;

        newHitBox.SetPower(effect.power);
        newHitBox.SetLifeTime(effect.lifeTime);
        newHitBox.SetAttacker(gameObject);
        // TODO: Add ignore layers
    }

    private void PerformMoveEffect(MoveEffect effect, KeyType type, int keyEffect, int moveEffect)
    {
        if (!effect.movingRB || effect.onCoolDown)
        {
            return;
        }

        // Must the mech be grounded to perform this effect
        if (effect.mustBeGrounded)
        {
            // Skip iteration if nothing is hit by box cast
            RaycastHit2D hit = CheckOnGround(effect.groundCheckParams);
            if (!hit.collider)
                return;
        }

        // Perform cool down
        if (effect.coolDown > 0f && !effect.onCoolDown)
            StartCoroutine(MoveEffectCoolDownTime(type, keyEffect, moveEffect));

        // Play moving animation
        if (anim)
        {
            anim.SetBool("moving", true);
        }

        // Delay effect for specified amount of time
        if (effect.effectDelay > 0)
        {
            StartCoroutine(WaitForMoveDelay(effect));
            return;
        }

        MoveRB(effect);
    }

    private void MoveRB(MoveEffect effect)
    {
        moving = true;

        if (effect.faceMoveDirection)
        {
            transform.root.localScale =  new Vector3(effect.moveDirection.normalized.x, transform.root.localScale.y, transform.root.localScale.z);
        }

        // Set velocity move event
        if (effect.moveType == MoveEffect.MoveType.Velocity)
        {
            Vector2 direction = effect.moveDirection.normalized * effect.speed;
            direction += Vector2.up * effect.movingRB.velocity.y;
            effect.movingRB.velocity = direction;
        }
        else if (effect.moveType == MoveEffect.MoveType.Force)
        {
            effect.movingRB.AddForce(effect.moveDirection.normalized * effect.speed,
                                                               ForceMode2D.Force);
        }
        else if (effect.moveType == MoveEffect.MoveType.Impulse)
        {
            effect.movingRB.AddForce(effect.moveDirection.normalized * effect.speed,
                                                               ForceMode2D.Impulse);
        }
    }

    private RaycastHit2D CheckOnGround(BoxCastParameters castParams)
    {
        return Physics2D.BoxCast(castParams.origin.position, castParams.size, castParams.angle,
                                 castParams.direction, castParams.distance, castParams.layerMask.value);
    }

    private void RotateTransformToCursor(Transform trans)
    {
        Vector3 diff = facingRight ?
                       Camera.main.ScreenToWorldPoint(Input.mousePosition) - trans.position :
                       trans.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        diff.Normalize();
        float zRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        zRotation = Mathf.Clamp(zRotation , partRotationConstraints.x, partRotationConstraints.y);
        trans.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }

    IEnumerator WaitForProjectileDelay(ProjectileEffect effect)
    {
        yield return new WaitForSeconds(effect.effectDelay);
        SpawnProjectile(effect);
    }

    IEnumerator WaitForMeleeDelay(MeleeEffect effect)
    {
        yield return new WaitForSeconds(effect.effectDelay);
        SpawnMeleeHitBox(effect);
    }

    IEnumerator WaitForMoveDelay(MoveEffect effect)
    {
        yield return new WaitForSeconds(effect.effectDelay);
        MoveRB(effect);
    }

    IEnumerator ProjectileEffectCoolDownTime(KeyType type, int keyEffect, int projectileEffect)
    {
        switch(type){
            case KeyType.KeyDown:
                keyDownEffects[keyEffect].projectileEffects[projectileEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyDownEffects[keyEffect].projectileEffects[projectileEffect].coolDown);
                keyDownEffects[keyEffect].projectileEffects[projectileEffect].onCoolDown = false;
                break;

            case KeyType.KeyUp:
                keyUpEffects[keyEffect].projectileEffects[projectileEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyUpEffects[keyEffect].projectileEffects[projectileEffect].coolDown);
                keyUpEffects[keyEffect].projectileEffects[projectileEffect].onCoolDown = false;
                break;

            case KeyType.KeyHold:
                keyHoldEffects[keyEffect].projectileEffects[projectileEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyHoldEffects[keyEffect].projectileEffects[projectileEffect].coolDown);
                keyHoldEffects[keyEffect].projectileEffects[projectileEffect].onCoolDown = false;
                break;
        }     
    }

    IEnumerator MeleeEffectCoolDownTime(KeyType type, int keyEffect, int meleeEffect)
    {
        switch (type)
        {
            case KeyType.KeyDown:
                keyDownEffects[keyEffect].meleeEffects[meleeEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyDownEffects[keyEffect].meleeEffects[meleeEffect].coolDown);
                keyDownEffects[keyEffect].meleeEffects[meleeEffect].onCoolDown = false;
                break;

            case KeyType.KeyUp:
                keyUpEffects[keyEffect].meleeEffects[meleeEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyUpEffects[keyEffect].meleeEffects[meleeEffect].coolDown);
                keyUpEffects[keyEffect].meleeEffects[meleeEffect].onCoolDown = false;
                break;

            case KeyType.KeyHold:
                keyHoldEffects[keyEffect].meleeEffects[meleeEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyHoldEffects[keyEffect].meleeEffects[meleeEffect].coolDown);
                keyHoldEffects[keyEffect].meleeEffects[meleeEffect].onCoolDown = false;
                break;
        }
    }

    IEnumerator MoveEffectCoolDownTime(KeyType type, int keyEffect, int moveEffect)
    {
        switch (type)
        {
            case KeyType.KeyDown:
                keyDownEffects[keyEffect].moveEffects[moveEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyDownEffects[keyEffect].moveEffects[moveEffect].coolDown);
                keyDownEffects[keyEffect].moveEffects[moveEffect].onCoolDown = false;
                break;

            case KeyType.KeyUp:
                keyUpEffects[keyEffect].moveEffects[moveEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyUpEffects[keyEffect].moveEffects[moveEffect].coolDown);
                keyUpEffects[keyEffect].moveEffects[moveEffect].onCoolDown = false;
                break;

            case KeyType.KeyHold:
                keyHoldEffects[keyEffect].moveEffects[moveEffect].onCoolDown = true;
                yield return new WaitForSeconds(keyHoldEffects[keyEffect].moveEffects[moveEffect].coolDown);
                keyHoldEffects[keyEffect].moveEffects[moveEffect].onCoolDown = false;
                break;
        }
    }
}
