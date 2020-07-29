using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward, Color.green, 1000);
        //Vector3 dir = target.position - transform.position;
        //Vector3 lookRotation = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
        //transform.rotation = Quaternion.Euler(0f, lookRotation.y, 0f);
        transform.LookAt(target, Vector3.up);
    }
}
