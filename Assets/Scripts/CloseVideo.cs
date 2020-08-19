using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseVideo : MonoBehaviour
{
    public static event Action OnVideoEnd;

    public void EndVideo()
    {
        if (GameManager.Instance.isVideoPlaying)
        {
            OnVideoEnd?.Invoke();
        }
    }
}
