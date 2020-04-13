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
    private Sprite[] subSpriteInstances;
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
}
