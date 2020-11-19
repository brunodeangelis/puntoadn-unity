using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToActivate = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        foreach (var gameObject in _objectsToActivate)
        {
            gameObject.SetActive(true);
        }
    }
}
