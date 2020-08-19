using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static event Action<Checkpoint> OnCheckpointHit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            OnCheckpointHit?.Invoke(this);
        }
    }
}
