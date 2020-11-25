using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera cameraToLookAt;

    void Start()
    {
        //transform.Rotate( 180,0,0 );
        if(cameraToLookAt == null)
        {
            cameraToLookAt = Camera.main;
        }
    }

    void LateUpdate()
    {
        //Vector3 v = cameraToLookAt.transform.position - transform.position;
        //v.x = v.z = 0.0f;
        //transform.LookAt(cameraToLookAt.transform.position - v);
        //transform.rotation = cameraToLookAt.transform.rotation;
        //transform.Rotate(0, 180, 0);

        transform.LookAt(
            transform.position + cameraToLookAt.transform.rotation * Vector3.forward,
            cameraToLookAt.transform.rotation * Vector3.up);

    }

}
