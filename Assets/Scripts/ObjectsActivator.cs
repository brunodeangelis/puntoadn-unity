using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToActivate = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var objectToActivate in _objectsToActivate)
            {
                objectToActivate.SetActive(true);
                Debug.Log(objectToActivate.name + " activated!");
            }

            Destroy(gameObject);
        }
    }
}
