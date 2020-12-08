using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private bool _create;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (_create) GameManager._i.CreateTask(_text);
            else GameManager._i.DeleteTask(_text);
            Destroy(gameObject);
        }
    }
}
