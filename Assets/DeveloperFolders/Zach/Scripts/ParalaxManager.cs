using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxManager : MonoBehaviour
{
    [SerializeField]
    paralaxLayer[] layers;
    Transform cam;
    // Start is called before the first frame update

    Vector3 initCamPos;
    void Awake()
    {
        cam = Camera.main.transform;
        initCamPos = cam.position;

        for(int i = 0; i < layers.Length; i++){
            layers[i].initPos = layers[i].layer.position;
        }

    }

    // Update is called once per frame
    void Update(){
        float xDif = (cam.transform.position - initCamPos).x;
        for (int i = 0; i < layers.Length; i++){
            Vector3 pos = layers[i].layer.position;
            pos.SetX(xDif * (layers[i].speedFactor - 1));
            layers[i].layer.position = pos;
        }
    }
}

[System.Serializable]
public struct paralaxLayer{
    [SerializeField]
    public Transform layer;
    [SerializeField]
    public float speedFactor;
    [HideInInspector]
    public Vector3 initPos;
}