using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;        // The player's transform
    public float smoothing = 5f;    // The damping factor for camera movement
    public float lookAhead = 3f;    // How far ahead to look at the player

    private Vector3 offset;         // The camera's offset from the player
    private float direction;

    void Start()
    {
        offset = transform.position;
    }

    void Update()
    {
        direction = target.localScale.x / target.localScale.y;
        Vector3 targetCamPos = target.position + offset;
        targetCamPos.x += lookAhead*direction;

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    void FixedUpdate()
    {
        
    }
}
