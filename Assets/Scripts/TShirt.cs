using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShirt : MonoBehaviour
{
    [SerializeField] private bool _isCorrectTShirt;
    private bool _isOnRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isOnRange = true;
            Debug.Log("player entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isOnRange = false;
            Debug.Log("player left");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isOnRange)
            {
                if (_isCorrectTShirt)
                {
                    Debug.Log("apretaste en la remera correcta");
                }
            }
        }
    }
}
