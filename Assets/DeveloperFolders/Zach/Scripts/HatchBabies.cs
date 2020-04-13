using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchBabies : DeathEvent{
    [SerializeField]
    GameObject[] eggs;

    public override void TriggerEvent(){
        foreach (var babe in eggs){
            var ani = babe.GetComponent<Animator>();
            ani.SetTrigger("Hatch");
            
        }
    }
}
