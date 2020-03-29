using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour{
    [SerializeField]
    GameObject obj;
    public void SpawnEvent(){
        var newObj = Instantiate(obj);
        newObj.transform.position = transform.position;
        newObj.transform.parent = null;
    }
}
