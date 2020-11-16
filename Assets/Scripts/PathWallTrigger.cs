using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWallTrigger : MonoBehaviour
{
    [SerializeField] private Wall _wall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _wall.Up();
            Destroy(gameObject);
        }
    }
}
