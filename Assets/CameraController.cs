using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distanceUp = 1;
    public float distanceBack = 4;
    public float speed = 5;
    public float currentDist;

    // Start is called before the first frame update
    void Start()
    {
        currentDist = distanceBack;    
    }

    // Update is called once per frame
    void Update()
    {
        // right drag rotates the camera
        if (Input.GetMouseButton(1))
        {
            Vector3 angles = transform.eulerAngles;
            // look up and down by rotating around X-axis
            angles.x = Mathf.Clamp(angles.x + Input.GetAxis("Mouse Y") * speed, 0, 70);
            // spin the camera round 
            angles.y += Input.GetAxis("Mouse X") * speed;
            transform.eulerAngles = angles;
        }

        // zoom in/out with mouse wheel
        distanceBack = Mathf.Clamp(distanceBack - Input.GetAxis("Mouse ScrollWheel") * speed, 2, 10);

        // look at the target point
        transform.position = target.position
            + distanceUp * Vector3.up
            - currentDist * transform.forward;

        // check for collisions
        RaycastHit hit;
        if (Physics.Raycast(target.position + distanceUp * Vector3.up, -transform.forward, out hit, distanceBack))
        {
            // snap the camera right in to where the collision happened
            currentDist = hit.distance;
        }
        else
        {
            // relax the camera back to the desired distance
            currentDist = Mathf.MoveTowards(currentDist, distanceBack, Time.deltaTime);
        }
    }
}
