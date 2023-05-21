using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgMove : MonoBehaviour
{
    public Transform CameraPos;
    public Transform BG;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 campos = CameraPos.position;
        Vector3 offSet = new Vector3(campos.x * 0.2f, campos.y * 0.8f, 1);
        BG.position = offSet;
    }
}
