using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class MechPartSpriteSwapper : MonoBehaviour
{
    [Tooltip("The number of sub sprites this mech part contains.")]
    [SerializeField]
    private int subSpriteCount = 0;
    [SerializeField]
    private SpriteRenderer[] subSpriteInstances;
    [Range(0f, 100f)]
    [SerializeField]
    private float topThreshold = 90f;
    [Range(0f, 100f)]
    [SerializeField]
    private float bottomThreshold = 0f;
    [SerializeField]
    private Sprite[] level0DamageSprites;
    [SerializeField]
    private Sprite[] level1DamageSprites;
    [SerializeField]
    private Sprite[] level2DamageSprites;

    private Health hp;

    // Validate that the arrays are the same size and that thresholds aren't crossing
    void OnValidate()
    {
        if (subSpriteInstances.Length != subSpriteCount)
        {
            Debug.LogWarning("Resized subSpriteInstances");
            Debug.LogWarning("Sprite swapper arrays must all be the same length.");
            System.Array.Resize(ref subSpriteInstances, subSpriteCount);
        }

        if (level0DamageSprites.Length != subSpriteCount)
        {
            Debug.LogWarning("Resized subSpriteInstances");
            Debug.LogWarning("Sprite swapper arrays must all be the same length.");
            System.Array.Resize(ref level0DamageSprites, subSpriteCount);
        }

        if (level1DamageSprites.Length != subSpriteCount)
        {
            Debug.LogWarning("Resized level1DamageSprites");
            Debug.LogWarning("Sprite swapper arrays must all be the same length.");
            System.Array.Resize(ref level1DamageSprites, subSpriteCount);
        }

        if (level2DamageSprites.Length != subSpriteCount)
        {
            Debug.LogWarning("Resized level2DamageSprites");
            Debug.LogWarning("Sprite swapper arrays must all be the same length.");
            System.Array.Resize(ref level2DamageSprites, subSpriteCount);
        }

        if(topThreshold == 0f)
        {
            topThreshold = 0.1f;
        }
        
        if (bottomThreshold == 100f)
        {
            bottomThreshold = 99.9f;
        }

        if (topThreshold <= bottomThreshold)
        {
            topThreshold = bottomThreshold + 0.1f;
        }
    }

    private void Start()
    {
        hp = GetComponent<Health>();
    }

    private void Update()
    {
        float currHealthPercent = (hp.GetHealth() / hp.GetMaxHealth()) * 100f;
        if (currHealthPercent >= topThreshold)
        {
            // Change to level 0 damage
            SwapSprites(level0DamageSprites);
        }
        else if (currHealthPercent < topThreshold && currHealthPercent > bottomThreshold)
        {
            // Change to level 1 damage
            SwapSprites(level1DamageSprites);
        }
        else if (currHealthPercent <= bottomThreshold)
        {
            // Change to level 2 damage
            SwapSprites(level2DamageSprites);
        }
    }

    private void SwapSprites(Sprite[] newSprites)
    {
        for(int i = 0; i < subSpriteCount; i++)
        {
            subSpriteInstances[i].sprite = newSprites[i];
        }
    }
}
