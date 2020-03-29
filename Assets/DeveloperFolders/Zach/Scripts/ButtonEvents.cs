using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour{
    public void StartHover(){
        GetComponent<Animator>().SetBool("Hover", true);
    }
    public void EndHover(){
        GetComponent<Animator>().SetBool("Hover", false);
    }

    public virtual void Click(){
        print("CLICK");
    }
}
