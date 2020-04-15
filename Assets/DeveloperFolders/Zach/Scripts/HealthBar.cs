using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform front;

    Transform target;

    // Start is called before the first frame update
    void Awake(){
        front = transform.Find("Back").Find("Front");
    }

    public void ConnectToObject(Transform obj){
        target = obj;

    }

    public void SetHealthBar(float percent){
        print("BAR " + percent);
        Vector3 newScale = front.transform.localScale;
        newScale.SetX(percent);
        front.transform.localScale = newScale;
        if (percent <= 0) Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = target.position + Vector3.up * 3;
    }
}
