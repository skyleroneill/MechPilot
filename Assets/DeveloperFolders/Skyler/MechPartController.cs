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
    public string fireAnimEvent;
    public bool rotateAim;
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
    public string moveAnimEvent;
    public bool rotateAim;
    public bool mustBeGrounded;
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
    //public MeleeEffect[] meleeEffects;
    public MoveEffect[] moveEffects;
}

[System.Serializable]
public struct KeyUpEffects
{
    public KeyCode upKey;
    public ProjectileEffect[] projectileEffects;
    //public MeleeEffect[] meleeEffects;
    public MoveEffect[] moveEffects;
}

[System.Serializable]
public struct KeyHoldEffects
{
    public KeyCode holdKey;
    public ProjectileEffect[] projectileEffects;
    //public MeleeEffect[] meleeEffects;
    public MoveEffect[] moveEffects;
}

public class MechPartController : MonoBehaviour
{
    public bool debug = false;
    public bool active = false;
    public bool rotatePartToCursor = true;
    public Vector2 partOffset;
    public float partStartRotation = 0f;
    public KeyDownEffects[] keyDownEffects;
    public KeyUpEffects[] keyUpEffects;
    public KeyHoldEffects[] keyHoldEffects;

    private enum KeyType {KeyDown, KeyUp, KeyHold}

    private void Update()
    {
        if (!active)
            return;

        KeyDown();
        KeyUp();
        KeyHold();
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
                    if (keyDownEffects[i].projectileEffects[j].onCoolDown)
                        continue;

                    Projectile newProjectile = Instantiate(keyDownEffects[i].projectileEffects[j].projectile,
                                                           keyDownEffects[i].projectileEffects[j].firePoint.position,
                                                           keyDownEffects[i].projectileEffects[j].firePoint.rotation).GetComponent<Projectile>();

                    if (!newProjectile)
                        continue;

                    // Rotate to aim at cursor
                    if (keyDownEffects[i].projectileEffects[j].rotateAim)
                    {
                        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - keyDownEffects[i].projectileEffects[j].firePoint.position;
                        diff.Normalize();
                        keyDownEffects[i].projectileEffects[j].firePoint.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                    }

                    Vector3 rotatedVector = Quaternion.AngleAxis(keyDownEffects[i].projectileEffects[j].angleOffset, Vector3.forward) * keyDownEffects[i].projectileEffects[j].firePoint.right;
                    newProjectile.SetDirection(rotatedVector);
                    newProjectile.SetPower(keyDownEffects[i].projectileEffects[j].power);
                    newProjectile.SetSpeed(keyDownEffects[i].projectileEffects[j].speed);
                    newProjectile.SetShooter(gameObject);
                    newProjectile.SetLifeTime(keyDownEffects[i].projectileEffects[j].lifeTime);
                    // TODO: Add ignore layers?

                    StartCoroutine(ProjectileEffectCoolDownTime(KeyType.KeyDown, i, j));
                }

                // Melee effects
                //for (j = 0; j < keyDownEffects[i].projectileEffects.Length; j++)
                //{

                //}

                // Move effects
                for (j = 0; j < keyDownEffects[i].moveEffects.Length; j++)
                {
                    if (!keyDownEffects[i].moveEffects[j].movingRB || keyDownEffects[i].moveEffects[j].onCoolDown)
                        continue;

                    // Must the mech be grounded to perform this effect
                    if (keyDownEffects[i].moveEffects[j].mustBeGrounded)
                    {
                        // Skip iteration if nothing is hit by box cast
                        RaycastHit2D hit = CheckOnGround(keyDownEffects[i].moveEffects[j].groundCheckParams);
                        if (!hit.collider)
                            continue;
                    }

                    // Set velocity move event
                    if (keyDownEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Velocity)
                    {
                        Vector2 direction = keyDownEffects[i].moveEffects[j].moveDirection.normalized * keyDownEffects[i].moveEffects[j].speed;
                        direction += Vector2.up * keyDownEffects[i].moveEffects[j].movingRB.velocity.y;
                        keyDownEffects[i].moveEffects[j].movingRB.velocity = direction;
                    }
                    else if (keyDownEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Force)
                    {
                        keyDownEffects[i].moveEffects[j].movingRB.AddForce(keyDownEffects[i].moveEffects[j].moveDirection.normalized * keyDownEffects[i].moveEffects[j].speed,
                                                                           ForceMode2D.Force);
                    }
                    else if (keyDownEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Impulse)
                    {
                        keyDownEffects[i].moveEffects[j].movingRB.AddForce(keyDownEffects[i].moveEffects[j].moveDirection.normalized * keyDownEffects[i].moveEffects[j].speed,
                                                                           ForceMode2D.Impulse);
                    }
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
                    if (keyUpEffects[i].projectileEffects[j].onCoolDown)
                        continue;

                    Projectile newProjectile = Instantiate(keyUpEffects[i].projectileEffects[j].projectile,
                                                           keyUpEffects[i].projectileEffects[j].firePoint.position,
                                                           keyUpEffects[i].projectileEffects[j].firePoint.rotation).GetComponent<Projectile>();

                    if (!newProjectile)
                        continue;

                    // Rotate to aim at cursor
                    if (keyUpEffects[i].projectileEffects[j].rotateAim)
                    {
                        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - keyUpEffects[i].projectileEffects[j].firePoint.position;
                        diff.Normalize();
                        keyUpEffects[i].projectileEffects[j].firePoint.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                    }

                    Vector3 rotatedVector = Quaternion.AngleAxis(keyUpEffects[i].projectileEffects[j].angleOffset, Vector3.forward) * keyUpEffects[i].projectileEffects[j].firePoint.right;
                    newProjectile.SetDirection(rotatedVector);
                    newProjectile.SetPower(keyUpEffects[i].projectileEffects[j].power);
                    newProjectile.SetSpeed(keyUpEffects[i].projectileEffects[j].speed);
                    newProjectile.SetShooter(gameObject);
                    newProjectile.SetLifeTime(keyUpEffects[i].projectileEffects[j].lifeTime);
                    // TODO: Add ignore layers?

                    StartCoroutine(ProjectileEffectCoolDownTime(KeyType.KeyUp, i, j));
                }

                // Melee effects
                //for (j = 0; j < keyDownEffects[i].projectileEffects.Length; j++)
                //{

                //}

                // Move effects
                for(j = 0; j < keyUpEffects[i].moveEffects.Length; j++)
                {
                    if (!keyUpEffects[i].moveEffects[j].movingRB || keyUpEffects[i].moveEffects[j].onCoolDown)
                        continue;

                    // Must the mech be grounded to perform this effect
                    if (keyUpEffects[i].moveEffects[j].mustBeGrounded)
                    {
                        // Skip iteration if nothing is hit by box cast
                        RaycastHit2D hit = CheckOnGround(keyUpEffects[i].moveEffects[j].groundCheckParams);
                        if (!hit.collider)
                            continue;
                    }

                    // Set velocity move event
                    if (keyUpEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Velocity)
                    {
                        Vector2 direction = keyUpEffects[i].moveEffects[j].moveDirection.normalized * keyUpEffects[i].moveEffects[j].speed;
                        direction += Vector2.up * keyUpEffects[i].moveEffects[j].movingRB.velocity.y;
                        keyUpEffects[i].moveEffects[j].movingRB.velocity = direction;
                    }
                    else if (keyUpEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Force)
                    {
                        keyUpEffects[i].moveEffects[j].movingRB.AddForce(keyUpEffects[i].moveEffects[j].moveDirection.normalized * keyUpEffects[i].moveEffects[j].speed,
                                                                           ForceMode2D.Force);
                    }
                    else if (keyUpEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Impulse)
                    {
                        keyUpEffects[i].moveEffects[j].movingRB.AddForce(keyUpEffects[i].moveEffects[j].moveDirection.normalized * keyUpEffects[i].moveEffects[j].speed,
                                                                           ForceMode2D.Impulse);
                    }
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
                    if (keyHoldEffects[i].projectileEffects[j].onCoolDown)
                        continue;

                    Projectile newProjectile = Instantiate(keyHoldEffects[i].projectileEffects[j].projectile,
                                                           keyHoldEffects[i].projectileEffects[j].firePoint.position,
                                                           keyHoldEffects[i].projectileEffects[j].firePoint.rotation).GetComponent<Projectile>();

                    if (!newProjectile)
                        continue;

                    // Rotate to aim at cursor
                    if (keyHoldEffects[i].projectileEffects[j].rotateAim)
                    {
                        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - keyHoldEffects[i].projectileEffects[j].firePoint.position;
                        diff.Normalize();
                        keyHoldEffects[i].projectileEffects[j].firePoint.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                    }

                    Vector3 rotatedVector = Quaternion.AngleAxis(keyHoldEffects[i].projectileEffects[j].angleOffset, Vector3.forward) * keyHoldEffects[i].projectileEffects[j].firePoint.right;
                    newProjectile.SetDirection(rotatedVector);
                    newProjectile.SetPower(keyHoldEffects[i].projectileEffects[j].power);
                    newProjectile.SetSpeed(keyHoldEffects[i].projectileEffects[j].speed);
                    newProjectile.SetShooter(gameObject);
                    newProjectile.SetLifeTime(keyHoldEffects[i].projectileEffects[j].lifeTime);
                    // TODO: Add ignore layers?

                    StartCoroutine(ProjectileEffectCoolDownTime(KeyType.KeyHold, i, j));
                }

                // Melee effects
                //for (j = 0; j < keyHoldEffects[i].meleeEffects.Length; j++)
                //{

                //}

                // Move effects
                for (j = 0; j < keyHoldEffects[i].moveEffects.Length; j++)
                {
                    if (!keyHoldEffects[i].moveEffects[j].movingRB || keyHoldEffects[i].moveEffects[j].onCoolDown)
                        continue;

                    // Must the mech be grounded to perform this effect
                    if (keyHoldEffects[i].moveEffects[j].mustBeGrounded)
                    {
                        // Skip iteration if nothing is hit by box cast
                        RaycastHit2D hit = CheckOnGround(keyHoldEffects[i].moveEffects[j].groundCheckParams);
                        if (!hit.collider)
                            continue;
                    }

                    // Set velocity move event
                    if (keyHoldEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Velocity)
                    {
                        Vector2 direction = keyHoldEffects[i].moveEffects[j].moveDirection.normalized * keyHoldEffects[i].moveEffects[j].speed;
                        direction += Vector2.up * keyHoldEffects[i].moveEffects[j].movingRB.velocity.y;
                        keyHoldEffects[i].moveEffects[j].movingRB.velocity = direction;
                    }
                    else if(keyHoldEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Force)
                    {
                        keyHoldEffects[i].moveEffects[j].movingRB.AddForce(keyHoldEffects[i].moveEffects[j].moveDirection.normalized * keyHoldEffects[i].moveEffects[j].speed, 
                                                                           ForceMode2D.Force);
                    }
                    else if(keyHoldEffects[i].moveEffects[j].moveType == MoveEffect.MoveType.Impulse)
                    {
                        keyHoldEffects[i].moveEffects[j].movingRB.AddForce(keyHoldEffects[i].moveEffects[j].moveDirection.normalized * keyHoldEffects[i].moveEffects[j].speed,
                                                                           ForceMode2D.Impulse);
                    }

                    StartCoroutine(MoveEffectCoolDownTime(KeyType.KeyHold, i, j));
                }
            }
        }
    }

    private RaycastHit2D CheckOnGround(BoxCastParameters castParams)
    {
        return Physics2D.BoxCast(castParams.origin.position, castParams.size, castParams.angle,
                                 castParams.direction, castParams.distance, castParams.layerMask.value);
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

    //IEnumerator MeleeEffectCoolDownTime(MeleeEffect effect)
    //{
    //    effect.onCoolDown = true;
    //    yield return new WaitForSeconds(effect.coolDown);
    //    effect.onCoolDown = false;
    //}

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
