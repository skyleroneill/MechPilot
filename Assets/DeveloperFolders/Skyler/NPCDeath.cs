using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    [Tooltip ("Spawns these on death")]
    private GameObject[] deathSpawns;
    [SerializeField]
    [Range(0, 100)]
    private int powerUpDropChance = 10;

    private Health hp;

    private void Start(){
        hp = GetComponent<Health>();
    }

    private void Update(){
        if(hp.GetHealth() <= 0){
            // If one was specified, then spawn the particle system
            if(particles)
                Instantiate(particles, transform.position, transform.rotation);

            // Spawn random power ups if random number is within specified percent
            if(deathSpawns.Length > 1 && Random.Range(0, 101) <= powerUpDropChance){
                Instantiate(deathSpawns[Random.Range(0, deathSpawns.Length)], transform.position, transform.rotation);
            }else if(deathSpawns.Length == 1 && Random.Range(1, 101) <= powerUpDropChance)
                Instantiate(deathSpawns[0], transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
