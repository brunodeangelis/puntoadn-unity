using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance { get { return _instance; } }
    public static event Action<Player> OnPlayerDeath;

    private Camera _mainCamera;
    public GameObject _cameraPivot;

    // These methods will be called on the object it hits.
    const string OnRaycastExitMessage = "OnRaycastExit";
    const string OnRaycastEnterMessage = "OnRaycastEnter";

    GameObject previous;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else
        {
            _instance = this;
        }

        _mainCamera = Camera.main;
        
    }


    private void Update()
    {
        RaycastHit hit;

        //Debug.DrawRay(_cameraPivot.transform.position, _cameraPivot.transform.forward * 100f, Color.magenta);
        if (Physics.Raycast(_cameraPivot.transform.position, _cameraPivot.transform.forward, out hit, 10f))
        {
            GameObject current = hit.collider.gameObject;
            if (previous != current)
            {
                SendMessageTo(previous, OnRaycastExitMessage);
                SendMessageTo(current, OnRaycastEnterMessage);
                previous = current;
            }
        } else
        {
            SendMessageTo(previous, OnRaycastExitMessage);
            previous = null;
        }
    }

    private void LateUpdate()
    {
        if (transform.position.y < -45)
        {
            OnPlayerDeath?.Invoke(this);
        }
    }

    void SendMessageTo(GameObject target, string message)
    {
        if (target)
            target.SendMessage(message, gameObject,
                    SendMessageOptions.DontRequireReceiver);
    }
}
