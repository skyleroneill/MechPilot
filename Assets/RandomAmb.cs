using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAmb : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;
    [SerializeField]
    float minWait;
    [SerializeField]
    float maxWait;

    void Start()
    {
        StartCoroutine("PlayRandClip");
    }

    IEnumerator PlayRandClip()
    {
        float waitTime = Random.Range(minWait, maxWait);
        yield return new WaitForSeconds(waitTime);
        int clipIndex = (int)(Random.value * clips.Length);
        GetComponent<AudioSource>().clip = clips[clipIndex];
        GetComponent<AudioSource>().Play();
        StartCoroutine("PlayRandClip");
    }
}
