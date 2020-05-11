using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform front;

    Transform target;
    [SerializeField]
    bool followTarget = true;

    [SerializeField]
    bool canHide = true;

    [SerializeField]
    bool destroyOnZero = true;

    // Start is called before the first frame update
    void Awake(){
        front = transform.Find("Back").Find("Front");
        HideBar();
    }

    public void ConnectToObject(Transform obj){
        target = obj;

    }

    public void SetHealthBar(float percent){
        print("BAR " + percent);

        if (percent < 0) percent = 0;

        Vector3 newScale = front.transform.localScale;
        newScale.SetX(percent);
        
        front.transform.localScale = newScale;
        if (percent <= 0 && destroyOnZero) Destroy(gameObject);
        if (percent >= 1) HideBar();
        else ShowBar();
    }

    void HideBar()
    {
        if (!canHide) return;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void ShowBar()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!followTarget) return;
        transform.position = target.position + Vector3.up * 3;
    }
}
